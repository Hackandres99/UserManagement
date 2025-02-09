import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authTokenSubject = new BehaviorSubject<boolean>(false);

  constructor() {
    // Verifica si ya existe un token al cargar la aplicación
    const token = localStorage.getItem('authToken');
    this.authTokenSubject.next(!!token); // True si existe un token
  }

  // Método para obtener el estado de autenticación
  get isAuthenticated$() {
    return this.authTokenSubject.asObservable();
  }

  // Método para iniciar sesión y actualizar el estado
  signin(token: string) {
    localStorage.setItem('authToken', token);
    this.authTokenSubject.next(true); // Cambiar estado de autenticación
  }

  // Método para cerrar sesión
  logout() {
    localStorage.removeItem('authToken');
    this.authTokenSubject.next(false); // Cambiar estado de autenticación
  }
  
}
