import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../core/auth/auth.service';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule],
  template: `
    <mat-toolbar color="primary">
      <span class="brand" routerLink="/">POS</span>
      <span class="spacer"></span>
      <ng-container *ngIf="isLoggedIn(); else loginLink">
        <button mat-button (click)="logout()"><mat-icon>logout</mat-icon> Logout</button>
      </ng-container>
      <ng-template #loginLink>
        <a mat-button routerLink="/auth/login"><mat-icon>login</mat-icon> Login</a>
      </ng-template>
    </mat-toolbar>
    <div class="content">
      <router-outlet></router-outlet>
    </div>
  `,
  styles: [`
    .spacer { flex: 1 1 auto; }
    .brand { font-weight: 600; cursor: pointer; }
    .content { padding: 16px; }
  `]
})
export class ShellComponent {
  private auth = inject(AuthService);
  isLoggedIn = computed(() => !!this.auth.token);

  logout() {
    this.auth.logout();
  }
}
