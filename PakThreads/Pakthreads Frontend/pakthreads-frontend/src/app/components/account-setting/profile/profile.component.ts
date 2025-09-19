import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ApiCallService } from '../../../shared/services/api-call.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  username: string = '';
  user: any = {}; // Loaded user info
  defaultImg = 'assets/images/no-image-found.jpg';
  profileForm!: FormGroup;

  private readonly api = inject(ApiCallService);
  private readonly fb = inject(FormBuilder);
   public _apiCall = inject(ApiCallService);

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.username = this.route.snapshot.paramMap.get('username') || '';
    this.initForm();
    this.loadUser();
    // Fetch and display user data using this.username
  }

  initForm(): void {
    this.profileForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      gender: [''],
      phoneNumber: [''],
      userBio: [''],
      dateOfBirth: [''],
      country: ['']
    });
  }

  loadUser(): void {
    this.api.GetWithToken('User/GetUserDetails').subscribe(res => {
      if (res.responseCode === 200) {
        this.user = res.data;
        this.profileForm.patchValue(this.user);
      }
    });
  }

  saveProfile(): void {
    const payload = this.profileForm.value;
    this.api.PostWithToken(payload, 'User/SaveAndUpdateUserImage').subscribe(res => {
      if (res.responseCode === 200) {
        alert('Profile saved successfully!');
      } else {
        alert('Failed to save profile.');
      }
    });
  }


uploadProfileImage(event: Event): void {
  const file = (event.target as HTMLInputElement)?.files?.[0];
  if (!file) return;

  if (file.size > 2 * 1024 * 1024) {
    alert('File size should be less than 2MB.');
    return;
  }

  const formData = new FormData();
  formData.append('profileImage', file);

  this._apiCall
    .PostWithToken(formData, 'User/UploadProfileImage') // Adjust API path if needed
    .subscribe({
      next: (res: any) => {
        if (res.responseCode === 200) {
          alert('Profile picture updated.');
          this.loadUser(); // reload the updated user data
        }
      },
      error: (err) => {
        console.error('Error uploading image:', err);
      }
    });
}



}

