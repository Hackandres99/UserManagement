import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  user: User = {} as User
  selectedFile: File | null = null;
  avatarsPath: string = `https://localhost:7008/avatars/`

  constructor(private userService: UserService){}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      this.userService.uploadAvatar(this.selectedFile).subscribe(
        res => {
          this.user.avatarImagePath = `${this.avatarsPath}${res.avatarImagePath}`
          this.userService.setAvatarImagePath(this.user.avatarImagePath);
        },
        error => {
          console.log("error al cargar la imagen: "+ error)
        }
      )
    }
  }

  ngOnInit() {
    this.userService.getUser().subscribe(
      (data) => {
        if(data.avatarImagePath === null) 
          data.avatarImagePath = `${this.avatarsPath}defaultAvatar.png`
        else 
          data.avatarImagePath = `${this.avatarsPath}${data.avatarImagePath}`
        this.user = data
        this.userService.setAvatarImagePath(this.user.avatarImagePath)
      },
      (error) => {
        console.error('Error al obtener los datos del usuario', error);
      }
    );
  }

}
