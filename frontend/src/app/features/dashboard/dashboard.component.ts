import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  template: `
    <div class="grid">
      <mat-card class="tile">
        <mat-icon color="primary">point_of_sale</mat-icon>
        <div class="value">$12,340</div>
        <div class="label">Today's Sales</div>
      </mat-card>
      <mat-card class="tile">
        <mat-icon color="primary">inventory_2</mat-icon>
        <div class="value">1,248</div>
        <div class="label">Products</div>
      </mat-card>
      <mat-card class="tile">
        <mat-icon color="primary">group</mat-icon>
        <div class="value">5,612</div>
        <div class="label">Customers</div>
      </mat-card>
    </div>
  `,
  styles: [`
    .grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(240px,1fr)); gap: 16px; }
    .tile { padding: 24px; display: grid; gap: 8px; align-content: start; }
    .value { font-size: 28px; font-weight: 700; }
    .label { color: rgba(0,0,0,.6); }
  `]
})
export class DashboardComponent {}
