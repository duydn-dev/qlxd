import { ChangeDetectorRef, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from './ngrx';
import * as $ from 'jquery';
import { MessageService, PrimeNGConfig } from 'primeng/api'; 
import { Router } from '@angular/router';
import * as roleAction from './ngrx/actions/role.action';
import { UserService } from './services/user/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'client-app';
  isLogin: boolean = false;
  isNotFound: boolean = false;
  progressSpinnerDlg: boolean = false;
  constructor(
    private primengConfig: PrimeNGConfig,
    private _store : Store<any>,
    private _messageService: MessageService,
    private _router: Router,
    private _userService: UserService,
    private cdRef:ChangeDetectorRef
  ) {
    const user:any = JSON.parse(localStorage.getItem('user'));
    if(!user){
      this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "thời gian hoạt động đã hết, hệ thống sẽ tự chuyển hướng về trang đăng nhập !" });
      setTimeout(() => {
          this.removeKeyNull();
          this._router.navigate(["login"]);
      }, 2000);
    }
  }
  ngAfterViewChecked(){
    this.primengConfig.ripple = true;
    this._store.subscribe(n => {
      this.removeKeyNull();
      this.isLogin = (n && n.user) ? n.user.isLogin : false;
      this.isNotFound = (n && n.user) ? n.user.isNotFound : false;
      this.progressSpinnerDlg = n.spinner?.isLoading;
      this.cdRef.detectChanges();
    })
  }
  removeKeyNull(){
    const user:any = JSON.parse(localStorage.getItem('user'));
    if(user == null || user == "null"){
      localStorage.removeItem('user');
      localStorage.removeItem('role');
    }
    else if (user && user.user && user.user.expire && new Date(user.user.expire) < new Date()){
      localStorage.removeItem('user');
      localStorage.removeItem('role');
    }
    return;
  }
}
