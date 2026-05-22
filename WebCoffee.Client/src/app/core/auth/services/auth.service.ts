import {
  Injectable,
  computed,
  inject,
  signal
} from '@angular/core';

import { Router } from '@angular/router';

import {
  Observable,
  tap,
  throwError,
  catchError,
  switchMap,
  finalize
} from 'rxjs';

import {
  HttpHandlerFn,
  HttpRequest
} from '@angular/common/http';

import { jwtDecode } from 'jwt-decode';

import { ApiService } from '../../services/api.service';
import { TokenService } from './token.service';

import {
  LoginRequest,
  LoginResponse,
  UserPayload
} from '../models/auth.model';

import { ApiResponse } from '../../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiService = inject(ApiService);
  private readonly tokenService = inject(TokenService);
  private readonly router = inject(Router);

  // =========================
  // STATE
  // =========================

  currentUser = signal<UserPayload | null>(null);

  isAuthenticated = computed(() => {
    return this.currentUser() !== null;
  });

  private isRefreshing = false;

  constructor() {
    this.restoreUserSession();
  }

  // =========================
  // LOGIN
  // =========================

  login(
    request: LoginRequest
  ): Observable<ApiResponse<LoginResponse>> {

    return this.apiService
      .post<ApiResponse<LoginResponse>>(
        '/auth/login',
        request
      )
      .pipe(
        tap((response) => {

          console.log('LOGIN RESPONSE:', response);

          if (response.success && response.data) {

            this.tokenService.setTokens(
              response.data.accessToken,
              response.data.refreshToken
            );

            this.restoreUserSession();

            console.log(
              'CURRENT USER:',
              this.currentUser()
            );
          }
        })
      );
  }

  // =========================
  // LOGOUT
  // =========================

  logout(): void {

    this.tokenService.removeTokens();

    this.currentUser.set(null);

    this.router.navigate(['/login']);
  }

  // =========================
  // RESTORE SESSION
  // =========================

  restoreUserSession(): void {

    const token = this.tokenService.getAccessToken();

    console.log('ACCESS TOKEN:', token);

    if (!token) {
      this.currentUser.set(null);
      return;
    }

    try {

      const decoded =
        jwtDecode<UserPayload>(token);

      console.log('DECODED TOKEN:', decoded);

      const isExpired =
        decoded.exp * 1000 < Date.now();

      if (isExpired) {

        this.logout();
        return;
      }

      this.currentUser.set(decoded);

    } catch (error) {

      console.error(
        'DECODE TOKEN ERROR',
        error
      );

      this.logout();
    }
  }

  // =========================
  // REFRESH TOKEN
  // =========================

  refreshToken(): Observable<ApiResponse<LoginResponse>> {

    const refreshToken =
      this.tokenService.getRefreshToken();

    return this.apiService.post<
      ApiResponse<LoginResponse>
    >('/auth/refresh-token', {
      refreshToken
    }).pipe(
      tap((response) => {

        if (response.success && response.data) {

          this.tokenService.setTokens(
            response.data.accessToken,
            response.data.refreshToken
          );

          this.restoreUserSession();
        }
      })
    );
  }

  // =========================
  // HANDLE 401
  // =========================

  handleRefreshToken(
    request: HttpRequest<any>,
    next: HttpHandlerFn
  ) {

    if (this.isRefreshing) {
      return throwError(() => new Error('Refreshing token'));
    }

    this.isRefreshing = true;

    return this.refreshToken().pipe(

      switchMap((response) => {

        const newAccessToken =
          response.data.accessToken;

        const clonedRequest =
          request.clone({
            setHeaders: {
              Authorization: `Bearer ${newAccessToken}`
            }
          });

        return next(clonedRequest);
      }),

      catchError((error) => {

        this.logout();

        return throwError(() => error);
      }),

      finalize(() => {
        this.isRefreshing = false;
      })
    );
  }
}