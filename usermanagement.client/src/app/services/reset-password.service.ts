import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Forgot } from '../models/forgot';

interface ForgotResponse {
  message: string
}

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {
  
  private apiUrl = 'https://localhost:7008/api/Reset/password';
  
  constructor(private http: HttpClient) { }

  sendPassword(forgot: Forgot): Observable<ForgotResponse> {
      return this.http.post<ForgotResponse>(this.apiUrl, forgot)
    }
}
