import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiCallService {
  private http = inject(HttpClient);
  private authService = inject(AuthService);

  // Localhost for development
  private baseRoute = 'https://localhost:7177/api/'; 





  // POST without token
  PostWithoutToken(payload: any, endpoint: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = this.baseRoute + endpoint;
    return this.http.post<any>(url, payload, { headers }).pipe(
      catchError((error) => {
      console.error('❌ API Call Error:', error); // <== ADD THIS IF NOT THERE
      return throwError(() => new Error('Something went wrong. Please try again later.'));
    })
  );
}

  // POST with token
  PostWithToken(payload: any, endpoint: string): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    });
    const url = this.baseRoute + endpoint;
    return this.http.post<any>(url, payload, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  // GET without token
  GetWithoutToken(endpoint: string): Observable<any> {
    const url = this.baseRoute + endpoint;
    return this.http.get<any>(url).pipe(
      catchError(this.handleError)
    );
  }

  // GET with token
  GetWithToken(endpoint: string): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    const url = this.baseRoute + endpoint;
    return this.http.get<any>(url, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  // Error handling
  private handleError(error: HttpErrorResponse) {
    if (error.status === 400) {
      return throwError(() => new Error('Bad request or user already exists.'));
    } else if (error.status === 401) {
      return throwError(() => new Error('Unauthorized request.'));
    } else {
      return throwError(() => new Error('Something went wrong. Please try again later.'));
    }
  }
}
