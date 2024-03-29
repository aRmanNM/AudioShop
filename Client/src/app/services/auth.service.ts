import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {JwtHelperService} from '@auth0/angular-jwt';
import {UserRegister} from '../models/user-register';
import {User} from '../models/user';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {VerificationData} from '../models/verification-data';
import {UserLogin} from '../models/user-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {
    this.decodedToken = this.jwtHelper.decodeToken(localStorage.getItem('token'));
  }

  login(userLogin: UserLogin): Observable<any> {
    return this.http.post<User>(this.baseUrl + 'login', userLogin, {
      params: {
        role: 'salesperson'
      }
    }).pipe(
      map((res: User) => {
        if (res) {
          localStorage.setItem('token', res.token);
          this.decodedToken = this.jwtHelper.decodeToken(res.token);
        }
      })
    );
  }

  register(userRegister: UserRegister): Observable<any> {
    if (!userRegister.age) {
      userRegister.age = 1;
    }

    if (!userRegister.gender) {
      userRegister.gender = 1;
    }

    return this.http.post<User>(this.baseUrl + 'register', userRegister, {
      params: {
        role: 'salesperson'
      }
    }).pipe(map((res: User) => {
      if (res) {
        localStorage.setItem('token', res.token);
        this.decodedToken = this.jwtHelper.decodeToken(res.token);
      }
    }));
  }

  verifyPhone(verificationData: VerificationData): Observable<any> {
    return this.http.post(this.baseUrl + 'verifyphone', verificationData);
  }

  verifyToken(verificationData: VerificationData): Observable<any> {
    return this.http.post(this.baseUrl + 'verifytoken', verificationData).pipe(
      map((res: User) => {
        if (res) {
          localStorage.setItem('token', res.token);
          this.decodedToken = this.jwtHelper.decodeToken(res.token);
        }
      })
    );
  }

  checkUserNameExists(userName: string): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'userexists', {
      params: {userName},
    });
  }

  loggedIn(): boolean {
    const token = localStorage.getItem('token');
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);
    }
    return false;
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isInRole(role: string): boolean {
    return this.decodedToken.role.toLowerCase() === role.toLowerCase() ? true : false;
  }

  updatePassword(oldPassword: string, newPassword: string): Observable<any> {
    return this.http.put(this.baseUrl + 'updatePassword', {oldPassword, newPassword});
  }
}
