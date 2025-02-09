import { AuthService } from './../../services/auth.service';
import { LoginService } from './../../services/login.service';
import { Component } from '@angular/core';
import { Login } from '../../models/login';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  login: Login = {} as Login
  constructor(private loginService: LoginService, private authService: AuthService, private router: Router){}

  signin(){
    return this.loginService.createLoginToken(this.login).subscribe(
      res => {
        const token = res.token
        if (token) {
          this.authService.signin(token)
          this.router.navigate(['/dashboard']);
        }
      },
      error => {
        alert('No se pudo iniciar sesión, Usuario no encontrado.')
        console.error('Error durante el login: ', error)
      }
    )
  }
  
}
