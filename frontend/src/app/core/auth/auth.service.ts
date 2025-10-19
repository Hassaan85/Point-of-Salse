import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

export interface LoginRequest { username: string; password: string; }
export interface UserInfo { userId: number; username: string; fullName: string; roleName: string; }
export interface LoginResponse { token: string; user: UserInfo; }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private readonly tokenKey = 'pos_jwt_token';
  user = signal<UserInfo | null>(null);

  get token(): string | null { return localStorage.getItem(this.tokenKey); }

  login(req: LoginRequest) {
    return this.http.post<LoginResponse>('/api/auth/login', req).pipe(
      tap(res => {
        localStorage.setItem(this.tokenKey, res.token);
        this.user.set(res.user);
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.user.set(null);
  }
}
