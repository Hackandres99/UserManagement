import { Component } from '@angular/core';
import { UpdatePass } from '../../models/update-pass';
import * as CryptoJS from 'crypto-js';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrl: './update-password.component.css'
})
export class UpdatePasswordComponent {

  passwords: UpdatePass = {} as UpdatePass;
  user: User = {} as User;

  constructor(private userService: UserService, private router: Router) {}

  updatePassword() {
    // Encriptamos las contraseñas
    const encryptedCurrentPassword = CryptoJS.SHA256(this.passwords.currentPassword).toString(CryptoJS.enc.Hex);

    // Primero obtenemos los datos del usuario
    this.userService.getUser().subscribe(
      (userData: User) => {
        this.user = userData;

        // Verificamos si la contraseña actual es correcta
        if (encryptedCurrentPassword === this.user.accountPassword) {
          // Si es correcta, actualizamos la contraseña
          this.user.accountPassword = this.passwords.newPassword;

          // Llamamos al servicio para actualizar el usuario
          this.userService.updateUser(this.user).subscribe(
            () => {
              alert("Contraseña actualizada correctamente");
              this.router.navigate(['/dashboard']);
            },
            error => {
              console.error("Error al actualizar la contraseña", error);
            }
          );
        } else {
          alert("La contraseña actual no es correcta");
        }
      },
      error => {
        console.error('Error al obtener los datos del usuario', error);
      }
    );
  }
}
