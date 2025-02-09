import { User } from './../models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl: string = 'https://localhost:7008/api/Users/'

  constructor(private http: HttpClient) { }

  getUser(): Observable<User>{
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<User>(this.apiUrl+'me', {headers})
  }

  updateUser(user: User): Observable<void> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<void>(this.apiUrl + 'me', user, { headers });
  }
  

  deleteUser(): Observable<void> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<void>(this.apiUrl + 'me', { headers });
  }
  

  createUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl+'signup', user)
  }
}
