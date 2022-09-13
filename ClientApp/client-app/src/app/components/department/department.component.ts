import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ApiConfig } from 'src/app/confis/api-config';
import { DepartmentService } from 'src/app/services/department/department.service';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.css']
})
export class DepartmentComponent implements OnInit {

  // variable 
  search: any = {
    textSearch: "",
    pageSize: 20,
    pageIndex: 1
  }
  departments: any[] = [];
  totalData: number = 0;
  isShowModal:boolean = false;
  isSubmit:boolean = false;
  departmentForm: FormGroup;
  departmentId: string;
  departmentStatus: any[] = [];
  get form() { return this.departmentForm.controls; }

  // init funtion
  constructor(
    private _fb: FormBuilder,
    private confirmationService: ConfirmationService,
    private _messageService: MessageService,
    private _departmentService: DepartmentService
  ) { }

  ngOnInit(): void {
    this.departmentStatus = ApiConfig.departmentStatus;
    this.departmentForm = this._fb.group({
      departmentId: this._fb.control(null),
      departmentName: this._fb.control(null, [Validators.required]),
      status: this._fb.control(null, [Validators.required]),
    });
    this.getFilter();
  }

  // user-definded method
  getFilter(){
    this._departmentService.getPagging(this.search).subscribe(response => {
      this.departments = response.responseData.data;
      this.totalData = response.responseData.totalData;
    })
  }
  searchData(){
    this.search.pageIndex = 1;
    this.getFilter();
  }
  onPageChange(event:any){
    this.search.pageIndex = (event.page + 1);
    this.getFilter();
  }
  openEditForm(departmentId = null){
    this.isSubmit = false;
    this.departmentForm.reset();
    if(!departmentId){
      this.isShowModal = true;
    }
    else{
      this.departmentId = departmentId;
      this._departmentService.getById(departmentId).subscribe(response => {
        this.departmentForm.patchValue({...response.responseData});
        this.isShowModal = true;
      })
    }
  }
  openDeleteForm(departmentId){
    this.isSubmit = false;
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn muốn xóa ?',
      header: '',
      accept: () => {
         this._departmentService.delete(departmentId).subscribe(
          response => {
            console.log(response);
            if(!response.success){
              this._messageService.add({ severity: 'error', summary: 'Lỗi', detail: response.message });
            }
            else{
              this.getFilter();
              this._messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
            }
          })
      },
      reject: () => {
      }
    });
  }
  save(){
    this.isSubmit = true;
    if (this.departmentForm.invalid) {
      return;
    }
    if(this.departmentId){
      this._departmentService.edit(this.departmentId, this.departmentForm.value).subscribe(response => {
        if(response.success){
          this.getFilter();
          this._messageService.add({severity:'success', summary:'Thành công', detail:'chỉnh sửa phòng ban thành công !'});
          this.isShowModal = false;
        }
        else{
          this._messageService.add({severity:'error', summary:'Lỗi', detail: response.message});
        }
      })
    }
    else{
      this._departmentService.create(this.departmentForm.value).subscribe(response => {
        if(response.success){
          this.getFilter();
          this._messageService.add({severity:'success', summary:'Thành công', detail:'Thêm mới phòng ban thành công !'});
          this.isShowModal = false;
        }
        else{
          this._messageService.add({severity:'error', summary:'Lỗi', detail: response.message});
        }
      })
    }
  }
}
