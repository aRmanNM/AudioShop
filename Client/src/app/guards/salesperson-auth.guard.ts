import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../services/auth.service';
import {AuthGuard} from './auth.guard';

@Injectable({
  providedIn: 'root'
})
export class SalespersonAuthGuard extends AuthGuard {
  constructor(authService: AuthService, router: Router) {
    super(authService, router);
  }

  canActivate(): boolean {
    const isAuthenticated = super.canActivate();
    if (isAuthenticated) {
      return this.authService.isInRole('SalesPerson');
    }

    return false;
  }
}
