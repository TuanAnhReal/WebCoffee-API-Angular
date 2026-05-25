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
  // AUTH
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
          if (response.success && response.data) {
            this.tokenService.setTokens(
              response.data.token,
              response.data.refreshToken
            );
            this.restoreUserSession();
          }
        })
      );
  }

  /**
   * Gọi API thu hồi refresh token trên server.
   * Backend đọc username từ Bearer token nên không cần truyền body.
   */
  revoke(): Observable<ApiResponse<null>> {
    return this.apiService.post<ApiResponse<null>>(
      '/auth/revoke',
      {}
    );
  }

  /**
   * Xoá toàn bộ session cục bộ (token + signal user).
   * Dùng chung cho cả logout thường và sau khi revoke xong.
   */
  clearSession(): void {
    this.tokenService.removeTokens();
    this.currentUser.set(null);
  }

  /**
   * Đăng xuất đầy đủ:
   * 1. Gọi API revoke để thu hồi refresh token trên server
   * 2. Dù thành công hay lỗi → luôn clear session local + redirect
   */
  logout(): void {
    this.revoke().pipe(
      catchError(() => {
        // Token hết hạn hoặc server lỗi → vẫn tiếp tục đăng xuất
        return [];
      }),
      finalize(() => {
        this.clearSession();
        this.router.navigate(['/login']);
      })
    ).subscribe();
  }

  // =========================
  // SESSION
  // =========================

  restoreUserSession(): void {

    const token = this.tokenService.getAccessToken();

    if (!token) {
      this.currentUser.set(null);
      return;
    }

    try {

      const decoded = jwtDecode<UserPayload>(token);

      const isExpired = decoded.exp * 1000 < Date.now();

      if (isExpired) {
        this.clearSession(); // không gọi logout() để tránh gọi revoke lúc token đã expired
        return;
      }

      this.currentUser.set(decoded);

    } catch {
      this.clearSession();
    }
  }

  // =========================
  // TOKEN REFRESH
  // =========================

  refreshToken(): Observable<ApiResponse<LoginResponse>> {

    const refreshToken = this.tokenService.getRefreshToken();

    return this.apiService.post<ApiResponse<LoginResponse>>(
      '/auth/refresh-token',
      { refreshToken }
    ).pipe(
      tap((response) => {
        if (response.success && response.data) {
          this.tokenService.setTokens(
            response.data.token,
            response.data.refreshToken
          );
          this.restoreUserSession();
        }
      })
    );
  }

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

        const newAccessToken = response.data.token;

        const clonedRequest = request.clone({
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

  // =========================
  // ROLE
  // =========================

  hasRole(role: string): boolean {

    const user = this.currentUser();

    if (!user) return false;

    return (
      user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      === role
    );
  }
}