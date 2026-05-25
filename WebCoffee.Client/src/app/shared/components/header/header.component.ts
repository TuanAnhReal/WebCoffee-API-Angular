import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/auth/services/auth.service';
import { AvatarComponent } from '../../../shared/avatar/avatar.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [AvatarComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Lấy user hiện tại
  get currentUser() {
    return this.authService.currentUser();
  }

  // Lấy tên hiển thị
  get displayName(): string {
    return this.currentUser?.[
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
    ] ?? 'Admin';
  }

  // Logout
  async logout(): Promise<void> {
    try {
      // Nếu AuthService có revoke()
      if (this.authService.revoke) {
        await this.authService.revoke().toPromise();
      }
    } catch {
      // Có lỗi vẫn logout local
    } finally {

      // Nếu có clearSession()
      if (this.authService.clearSession) {
        this.authService.clearSession();
      } else {
        // fallback logout cũ
        this.authService.logout();
      }

      this.router.navigate(['/login']);
    }
  }
}