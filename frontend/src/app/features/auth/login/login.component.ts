import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../core/auth/auth.service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <form class="login-form" [formGroup]="form" (ngSubmit)="onSubmit()">
      <h2>Sign in</h2>
      <mat-form-field appearance="outline" class="full">
        <mat-label>Username</mat-label>
        <input matInput formControlName="username" autocomplete="username" />
      </mat-form-field>

      <mat-form-field appearance="outline" class="full">
        <mat-label>Password</mat-label>
        <input matInput type="password" formControlName="password" autocomplete="current-password" />
      </mat-form-field>

      <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid || loading()">Login</button>
    </form>
  `,
  styles: [`
    .login-form { max-width: 360px; margin: 80px auto; display: grid; gap: 16px; }
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
      .subscribe({ next: () => this.loading.set(false), error: () => this.loading.set(false) });
  }
}
