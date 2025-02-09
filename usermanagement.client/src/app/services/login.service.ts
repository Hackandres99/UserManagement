import { Login } from './../models/login';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

interface LoginResponse {
  token: string;
}


@Injectable({
  providedIn: 'root'
})


export class LoginService {

  private apiUrl = 'https://localhost:7008/api/Login/signin';

  constructor(private http: HttpClient) { }

  createLoginToken(login: Login): Observable<LoginResponse> {
      return this.http.post<LoginResponse>(this.apiUrl, login)
    }
}
