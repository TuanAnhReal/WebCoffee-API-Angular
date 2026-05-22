import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../core/auth/services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatSnackBarModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  hidePassword = true;
  isLoading = false;

  loginForm = this.fb.nonNullable.group({
    tenDangNhap: ['', [Validators.required]],
    matKhau: ['', [Validators.required, Validators.minLength(6)]]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    
    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: (res) => {
        this.isLoading = false;
        
        // Điều hướng thông minh dựa trên ReturnUrl
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

this.router.navigateByUrl(returnUrl).then(res => {
  console.log('NAVIGATION:', res);
});
        console.log('RETURN URL:', returnUrl);

        
        this.snackBar.open('Đăng nhập thành công', 'Đóng', { duration: 3000 });
      },
      error: (err) => {
        this.isLoading = false;
        // Interceptor lỗi sẽ lo việc hiển thị Snackbar sau, nhưng có thể fallback ở đây
        this.loginForm.get('matKhau')?.reset(); 
      }
    });
  }
}