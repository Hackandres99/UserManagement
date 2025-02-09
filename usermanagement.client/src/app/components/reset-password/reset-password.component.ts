import { Component } from '@angular/core';
import { Forgot } from '../../models/forgot';
import { ResetPasswordService } from '../../services/reset-password.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {

  forgot: Forgot = {} as Forgot
    constructor(private resetPasswordService: ResetPasswordService, private router: Router){}
  
    resetPassword(){
      return this.resetPasswordService.sendPassword(this.forgot).subscribe(
        res => {
          alert(res.message);
          this.router.navigate(['/login']);
        },
        error => {
          console.error('Error durante el reseteo de la contraseña: ', error)
        }
      )
    }

}
