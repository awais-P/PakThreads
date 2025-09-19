import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiCallService } from '../../../shared/services/api-call.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})


export class SignupComponent {

  public _apiCall = inject(ApiCallService);
  
  signupForm: FormGroup;
  hidePassword: boolean = true;
  isLoading = false;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private router: Router) {
    this.signupForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  togglePassword(): void {
    this.hidePassword = !this.hidePassword;
  }

  onSignup(): void {
    if (this.signupForm.invalid) return;

    const signUpPayload: any = {
        userName: this.signupForm.get('userName')?.value,
        email: this.signupForm.get('email')?.value,
        password: this.signupForm.get('password')?.value,
        isSiteUser: true
      };
      
    this.isLoading = true;
    this.errorMessage = '';

    this._apiCall.PostWithoutToken(signUpPayload, 'User/CreateUser').subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/auth/login']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.message || 'Signup failed. Please try again.';
        console.error('Signup error:', err);
      }
    });
  }


  // his._apiService.PostCallWithoutToken(signInPayload, 'User/UserLogin').subscribe((response) => {
  //       if (response.responseCode == 200) {
  //         this.onLoginSucces(response, signInPayload);
  //       } else {
  //         this.onLoginFailure(response,signInPayload)
  //       }
  //     }, (error) => {
  //       this.errorHandlerService.handleHttpError(error as HttpErrorResponse);
  //       this.isLoading = false;
  //     });
  //   };

  // };



  goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}
