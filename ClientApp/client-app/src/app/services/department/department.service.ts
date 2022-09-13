import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from '../base/base-service.service';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  constructor(
    private _baseService: BaseService
    ) {
  }

  getPagging(request: any): Observable<any>{
    return this._baseService.getWithQuery("api/department", 'request', JSON.stringify(request));
  }
  getById(id:String): Observable<any>{
    return this._baseService.get("api/department",id);
  }
  create(department:any){
    return this._baseService.post("api/department", department);
  }
  edit(id: String, department:any){
    return this._baseService.put("api/department",id, department);
  }
  delete(id: String){
    return this._baseService.delete("api/department", id);
  }
}
