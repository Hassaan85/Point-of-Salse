import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../../core/auth/auth.service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatCardModule],
  template: `
    <div class="center">
      <mat-card class="card">
        <mat-card-header>
          <mat-card-title>Welcome back</mat-card-title>
          <mat-card-subtitle>Sign in to continue</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <form class="login-form" [formGroup]="form" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="full">
              <mat-label>Username</mat-label>
              <input matInput formControlName="username" autocomplete="username" />
            </mat-form-field>

            <mat-form-field appearance="outline" class="full">
              <mat-label>Password</mat-label>
              <input matInput type="password" formControlName="password" autocomplete="current-password" />
            </mat-form-field>

            <button mat-raised-button color="primary" type="submit" class="full" [disabled]="form.invalid || loading()">Login</button>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .center { min-height: calc(100vh - 64px); display: grid; place-items: center; padding: 16px; }
    .card { width: 100%; max-width: 420px; }
    .login-form { display: grid; gap: 16px; }
    .full { width: 100%; }
  `]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  loading = signal(false);

  form = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  onSubmit() {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.auth
      .login(this.form.getRawValue() as any)
      .subscribe({
        next: () => {
          this.loading.set(false);
          location.assign('/dashboard');
        },
        error: () => this.loading.set(false),
      });
  }
}
