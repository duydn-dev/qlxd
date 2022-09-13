import { environment } from '../../environments/environment';
export class ApiConfig {
    static readonly apiUrl:string = environment.apiUrl;
    static readonly userType:any[] = [
        {status: 0, name: "Không sử dụng"},
        {status: 1, name: "Đang sử dụng"}
    ];
    static readonly departmentStatus:any[] = [
        {status: 0, name: "Không sử dụng"},
        {status: 1, name: "Đang sử dụng"}
    ]
}