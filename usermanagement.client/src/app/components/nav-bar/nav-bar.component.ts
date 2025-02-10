import { UserService } from './../../services/user.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent implements OnInit, OnDestroy {
  
  isAuthenticated: boolean = false;
  private authSubscription!: Subscription;
  avatarPath: string = '';

  constructor(
    private authService: AuthService, 
    private userService: UserService, 
    private router: Router) {}

  ngOnInit() {
    this.authSubscription = this.authService.isAuthenticated$.subscribe(isAuth => {
      this.isAuthenticated = isAuth;
    });

    this.userService.avatarImagePath$.subscribe(path => {
      if (path) this.avatarPath = path
    })

  }

  logout() {
    this.authService.logout()
    this.router.navigate(['/'])
  }

  deleteAccount(): void {
    const confirmDelete = window.confirm("¿Estás seguro de que quieres eliminar tu cuenta?")
    
    if (confirmDelete) {
      this.userService.deleteUser().subscribe(
        () => {
          alert('Cuenta eliminada con éxito')
          this.authService.logout()
          this.router.navigate(['/'])
        },
        (error) => {
          console.error('Error al eliminar la cuenta: ', error)
          alert('Ocurrió un error al eliminar la cuenta')
        }
      );
    } else {
      console.log('Eliminación de cuenta cancelada')
    }
  }

  ngOnDestroy() {
    if (this.authSubscription) {
      this.authSubscription.unsubscribe()
    }
  }
}
