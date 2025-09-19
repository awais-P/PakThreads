import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class authInterceptor implements HttpInterceptor {
  private authService = inject(AuthService);
  private router = inject(Router);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Trim request body
    if (req.body) {
      const trimObjectValues = (obj: any): any => {
        if (typeof obj !== 'object' || obj === null) return obj;
        if (Array.isArray(obj)) return obj.map(trimObjectValues);

        return Object.keys(obj).reduce((acc, key) => {
          const value = obj[key];
          acc[key] = typeof value === 'string' ? value.trim() : trimObjectValues(value);
          return acc;
        }, {} as any);
      };
      req = req.clone({ body: trimObjectValues(req.body) });
    }

    // Token expiry check
    const authHeader = req.headers.get('Authorization');
    if (authHeader && authHeader !== 'Bearer null') {
      const token = authHeader.split(' ')[1];
      if (this.authService.isTokenExpired(token)) {
        this.authService.logout();
        this.router.navigate(['/login']);
        return throwError(() => new Error('Token expired'));
      }
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.authService.logout();
          this.router.navigate(['/login']);
          return throwError(() => new Error('Unauthorized'));
        }
        return throwError(() => error);
      })
    );
  }
}
