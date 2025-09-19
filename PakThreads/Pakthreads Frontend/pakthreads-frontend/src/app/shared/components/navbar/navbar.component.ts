import { Component, OnInit } from '@angular/core';
import { ApiCallService } from '../../services/api-call.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent  {
  searchText: string = '';
  defaultImg: string = 'assets/images/no-image-found.jpg';

  user = {
    profileImageUrl: '',
    userName: ''
  };

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    this.user.userName = this.authService.getUserName() ?? 'Guest';
  }

  onSearch() {
    if (this.searchText.trim()) {
      this.router.navigate(['/timeline/home-page'], {
        queryParams: { search: this.searchText.trim() }
      });
    }
  }

  clearSearch() {
    this.searchText = '';
    this.router.navigate(['/timeline/home-page']);
  }

  goToProfile() {
  const username = this.user.userName; 
  this.router.navigate(['/account/profile', username]);
}

}