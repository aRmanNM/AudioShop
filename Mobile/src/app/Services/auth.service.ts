import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import {environment} from 'src/environments/environment';
import {User} from '../Models/User';

interface UserToLogin {
    email: string;
    password: string;
}

interface UserToRegister {
    name: string;
    email: string;
    password: string;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    baseUrl = environment.apiUrl + 'auth/';
    jwtHelper = new JwtHelperService();
    decodedToken: any;

    constructor(private http: HttpClient) {
        this.decodeToken();
    }

    login(userToLogin: UserToLogin) {
        return this.http.post<User>(this.baseUrl + 'login', userToLogin).pipe(
            map((res: User) => {
                if (res) {
                    localStorage.setItem('token', res.token);
                    this.decodedToken = this.jwtHelper.decodeToken(res.token);
                }
            })
        );
    }

    register(newUser: UserToRegister) {
        return this.http.post<User>(this.baseUrl + 'register', newUser);
    }

    checkUserExists(email) {
        return this.http.get<boolean>(this.baseUrl + 'emailexists', {
            params: {email},
        });
    }

    loggedIn() {
        const token = localStorage.getItem('token');
        return !this.jwtHelper.isTokenExpired(token);
    }

    logout() {
        localStorage.removeItem('token');
    }

    decodeToken() {
        const token = localStorage.getItem('token');
        if (token) {
            this.decodedToken = this.jwtHelper.decodeToken(token);
        }
    }
}
