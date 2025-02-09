import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-update-data',
  templateUrl: './update-data.component.html',
  styleUrl: './update-data.component.css'
})
export class UpdateDataComponent implements OnInit {

  user: User = {} as User
  constructor(private userService: UserService, private router: Router){}

  ngOnInit(): void {
    this.loadUserData();
  }

  loadUserData(): void {
    this.userService.getUser().subscribe(
      (userData: User) => {
        this.user = userData;
      },
      (error) => {
        console.error('Error al cargar los datos del usuario', error);
      }
    );
  }

  updateData(): void {
    this.userService.updateUser(this.user).subscribe(
      () => {
        alert('Datos actualizados con éxito');
        this.router.navigate(['/dashboard']); // Redirige a Dashboard después de actualizar
      },
      (error) => {
        console.error('Error al actualizar los datos: ', error);
        alert('Ocurrió un error al actualizar los datos');
      }
    );
  }
}
