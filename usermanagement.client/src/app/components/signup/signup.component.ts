import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {

  user: User = {} as User
    constructor(private userService: UserService, private router: Router){}
  
    signup(){
      return this.userService.createUser(this.user).subscribe(res => {
        console.log('Usuario registrado:', res);
        this.router.navigate(['/dashboard']);
      },
      error => {
        console.error('Error durante el registro: ', error);
      })
    }
}
