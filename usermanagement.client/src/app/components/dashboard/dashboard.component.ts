import { UserService } from './../../services/user.service';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { User } from '../../models/user';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  user: User = {} as User

  constructor(private userService: UserService){}

  ngOnInit() {
    this.userService.getUser().subscribe(
      (data) => {
        this.user = data; // Guardar los datos del usuario
      },
      (error) => {
        console.error('Error al obtener los datos del usuario', error);
      }
    );
  }

}
