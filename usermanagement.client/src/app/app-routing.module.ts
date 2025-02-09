import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { dashboardGuard } from './guards/dashboard.guard';
import { UpdateDataComponent } from './components/update-data/update-data.component';
import { UpdatePasswordComponent } from './components/update-password/update-password.component';

const routes: Routes = [
  {path:'', component: LoginComponent, canActivate: [authGuard]},
  {path:'signup', component: SignupComponent, canActivate: [authGuard]},
  {path:'reset-password', component: ResetPasswordComponent, canActivate: [authGuard]},
  {path: 'dashboard', component: DashboardComponent, canActivate: [dashboardGuard]},
  {path: 'update-data', component: UpdateDataComponent, canActivate: [dashboardGuard]},
  {path: 'update-password', component: UpdatePasswordComponent, canActivate:[dashboardGuard]},
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
