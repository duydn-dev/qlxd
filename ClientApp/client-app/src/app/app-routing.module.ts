import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DepartmentComponent } from './components/department/department.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { DecentralizationComponent } from './components/users/decentralization/decentralization.component';
import { LoginComponent } from './components/users/login/login.component';
import { ProfileComponent } from './components/users/profile/profile.component';
import { UserComponent } from './components/users/user/user.component';
import { AuthGuard } from './guard/auth-guard';


const routes: Routes = [
  { path: '', component: DashboardComponent, canActivate: [AuthGuard], data: {routerId : 'dashboard', requireAuth: true} },
  { path: 'users', component: UserComponent, canActivate: [AuthGuard], data: {routerId : 'users', requireAuth: true} },
  { path: 'profile/:userId', component: ProfileComponent, canActivate: [AuthGuard], data: {routerId : 'profile', requireAuth: true} },
  { path: 'department', component: DepartmentComponent, canActivate: [AuthGuard], data: {routerId : 'department', requireAuth: true} },
  { path: 'decentralization/:userId', component: DecentralizationComponent, canActivate: [AuthGuard], data: {routerId : 'decentralization', requireAuth: true} },
  { path: 'login', component: LoginComponent , data: {routerId: null, requireAuth: false}},
  { path: '404', component: NotfoundComponent, data: {routerId: null, requireAuth: false}},
  { path: '**', redirectTo: '/404', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy', useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
