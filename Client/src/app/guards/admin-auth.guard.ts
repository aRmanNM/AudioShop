import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {AuthGuard} from './auth.guard';
import {AuthService} from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard extends AuthGuard {
  constructor(authService: AuthService, router: Router) {
    super(authService, router);
  }

  canActivate(): boolean {
    const isAuthenticated = super.canActivate();
    if (isAuthenticated) {
      return this.authService.isInRole('Admin');
    }

    return false;
  }

}
