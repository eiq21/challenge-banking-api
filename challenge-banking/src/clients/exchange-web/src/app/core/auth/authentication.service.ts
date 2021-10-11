import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { BehaviorSubject, from, Observable } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';
import { AuthRequest } from './auth-request.interface';
import { AuthResponse } from './auth-response.interface';
export const TOKEN_KEY = 'x-token';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  API_IDENTITY_URL: string = environment.serverIdentityUrl;
  authState = new BehaviorSubject(false);
  isAuthenticated: BehaviorSubject<boolean | null> = new BehaviorSubject<
    boolean | null
  >(null);
  currentAccessToken = '';
  constructor(private http: HttpClient, private router: Router) {
    this.loadToken();
  }

  async loadToken() {
    const token =
      sessionStorage.getItem(TOKEN_KEY) || localStorage.getItem(TOKEN_KEY);

    if (token) {
      this.currentAccessToken = token;
      this.isAuthenticated.next(true);
    } else this.isAuthenticated.next(false);
  }

  login(credentials: AuthRequest): Observable<any> {
    console.log('credentials:::', credentials);
    return this.http
      .post<AuthResponse>(
        `${this.API_IDENTITY_URL}identities/authenticate`,
        credentials
      )
      .pipe(
        map((data: AuthResponse) => data.token),
        switchMap((token) => {
          localStorage.setItem(TOKEN_KEY, token);
          return from(token);
        }),
        tap((_) => {
          this.isAuthenticated.next(true);
        })
      );
  }

  logout() {
    this.isAuthenticated.next(false);
    sessionStorage.removeItem(TOKEN_KEY);
    localStorage.getItem(TOKEN_KEY);
    this.router.navigate(['/auth']);
  }
}
