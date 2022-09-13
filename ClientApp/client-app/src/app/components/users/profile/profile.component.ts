import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { UserService } from 'src/app/services/user/user.service';
import {ApiConfig} from '../../../confis/api-config';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup
  user:any = {}
  isEdit: boolean = false;
  apiUrl:string = null;
  get form() { return this.profileForm.controls; }
  
  constructor(
    private activatedRoute: ActivatedRoute,
    private _userService: UserService,
    private _fb: FormBuilder,
    private _messageService: MessageService,
  ) {
    this.profileForm = this._fb.group({
      address : this._fb.control(null),
      avatar : this._fb.control(null),
      createdBy : this._fb.control(null),
      createdDate : this._fb.control(null),
      departmentId : this._fb.control(null),
      email : this._fb.control(null, [Validators.email]),
      fullName : this._fb.control(null),
      modifiedBy : this._fb.control(null),
      modifiedDate : this._fb.control(null),
      numberPhone : this._fb.control(null),
      passWord : this._fb.control(null),
      status : this._fb.control(null),
      userId : this._fb.control(null),
      userName : this._fb.control(null),
      userPositionId  : this._fb.control(null),
    }); 
    this.activatedRoute.params.subscribe(params => {
      const userId = params['userId'];
      this.getUserProfile(userId);
  });
  }

  getUserProfile(userId){
    this._userService.getUserProfile(userId).subscribe(response => {
      if(response.success){
        this.user = response.responseData;
        this.profileForm.patchValue(this.user);
      }
    })
  }
  onUploadAvatar(event){
    const images = event.target.files;
    if(images && images.length > 0){
      const image = images[0];
      this._userService.updateAvatar(image, this.user.userId).subscribe(response => {
        if(response.success){
          this._messageService.add({ severity: 'success', summary: 'Thành công', detail: "Cập nhật ảnh đại diện thành công !" })
          this.getUserProfile(this.user.userId);
        }
        else{
          this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "Không thể cập nhật ảnh đại diện, vui lòng thử lại !" })
        }
      })
    }
  }
  updateProfile(){
    const formValue = this.profileForm.value;
    this._userService.editUser(formValue).subscribe(response => {
      if(response.success){
        this._messageService.add({ severity: 'success', summary: 'Thành công', detail: "Cập nhật thông tin thành công !" })
        this.getUserProfile(this.user.userId);
      }
      else{
        this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: "Không thể cập nhật thông tin, vui lòng thử lại !" })
      }
    })
  }
  ngOnInit(): void {
    this.apiUrl = ApiConfig.apiUrl;
  }

}
