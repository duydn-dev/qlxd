import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { State } from 'src/app/ngrx';
import * as userActions from '../../ngrx/actions/login.action';
import * as roleActions from '../../ngrx/actions/role.action';
declare var $:any;
import { Constant } from 'src/app/confis/constants';

@Component({
  selector: 'app-menu-left',
  templateUrl: './menu-left.component.html',
  styleUrls: ['./menu-left.component.css']
})
export class MenuLeftComponent implements OnInit {

  menuData:any[] = [];
  constructor(
    private _router: Router,
    private _store : Store<State>,
  ) { }

  ngOnInit(): void {
    this.menuData = Constant.menuData;
  }

  menuClick(url){
    if(url) this._router.navigate([url]);
  }
  logout(){
    this._store.dispatch(userActions.logout());
    this._store.dispatch(roleActions.removeRole());
    this._router.navigate(["/login"]);
  }
}
