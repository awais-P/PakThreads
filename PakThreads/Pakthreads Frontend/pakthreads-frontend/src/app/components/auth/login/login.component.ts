import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiCallService } from '../../../shared/services/api-call.service';
import { AuthService } from '../../../shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

   public _apiCall = inject(ApiCallService);
   public authService = inject(AuthService);

  loginForm: FormGroup;
  hidePassword: boolean = true;
  isLoading = false;
  errorMessage: string = '';


  constructor(private fb: FormBuilder, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  togglePassword(): void {
    this.hidePassword = !this.hidePassword;
  }

  onLogin(): void {
  if (this.loginForm.invalid) return;

  const loginPayload = {
    email: this.loginForm.get('email')?.value,
    password: this.loginForm.get('password')?.value
  };

  this.errorMessage = '';

  this._apiCall.PostWithoutToken(loginPayload, 'User/UserLogin').subscribe({
    next: (res) => {

      console.log('API response:', res);

      if (res.responseCode === 200 && res.data?.token) {
        this.authService.saveToken(res.data.token);
        console.log('login successful')

console.log('Navigating to /timeline/home-page');
this.router.navigate(['/timeline/home-page']).then(success => {
  console.log('Navigation success?', success);
}).catch(err => {
  console.error('Navigation error:', err);
});
      } else {
        this.errorMessage = res.responseMessage || 'Login failed. Please try again.';
      }
    },
    error: (err) => {
      this.isLoading = false;
      this.errorMessage = err.message || 'Something went wrong. Please try again.';
      console.error('Login error:', err);
    }
  });
}



  goToSignup(): void {
    this.router.navigate(['/auth/signup']);
  }
}