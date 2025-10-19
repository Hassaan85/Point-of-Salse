import { Routes } from '@angular/router';
import { AUTH_ROUTES } from './features/auth/auth.routes';
import { ShellComponent } from './layout/shell.component';

export const routes: Routes = [
  { path: 'auth', children: AUTH_ROUTES },
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'auth/login' },
      // future feature routes here
    ],
  },
];
