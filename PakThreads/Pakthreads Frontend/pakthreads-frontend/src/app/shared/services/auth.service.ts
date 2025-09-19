import { Injectable } from '@angular/core';

import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  exp: number;
  Type: string;
  ID: number;
  UserName: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly TOKEN_KEY = 'token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(
    this.hasValidToken()
  );

  constructor(private router: Router) {}

  saveToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
    this.isAuthenticatedSubject.next(true);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  private decodeToken(token: string): DecodedToken {
    try {
      return jwtDecode<DecodedToken>(token);
    } catch {
      return { exp: 0, Type: '', ID: 0, UserName: '' };
    }
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem('userDetails');
    sessionStorage.clear();
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/']);
  }

  isTokenExpired(token: string | null): boolean {
    if (!token) return true;
    const decodedToken = this.decodeToken(token);
    return Date.now() >= decodedToken.exp * 1000;
  }

  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  hasValidToken(): boolean {
    const token = this.getToken();
    return !!token && !this.isTokenExpired(token);
  }

  getUserRole(): string | null {
    const token = this.getToken();
    return token ? this.decodeToken(token).Type : null;
  }

  getUserId(): number | null {
    const token = this.getToken();
    return token ? this.decodeToken(token).ID : null;
  }

  getUserDetails(): any {
    return localStorage.getItem('userDetails');
  }

  getUserName(): string | null {
  const token = this.getToken();
  return token ? this.decodeToken(token).UserName : null;
}


  canActivate(): boolean {
    if (this.hasValidToken()) {
      return true;
    } else {
      this.router.navigate(['/']);
      return false;
    }
  }
}
