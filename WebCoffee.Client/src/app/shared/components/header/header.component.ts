import { Component, inject } from '@angular/core';

import { AuthService } from '../../../core/auth/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  readonly authService = inject(AuthService);

  logout(): void {
    this.authService.logout();
  }

}