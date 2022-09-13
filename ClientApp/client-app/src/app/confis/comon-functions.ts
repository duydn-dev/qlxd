import { Constant } from "./constants";
export class ComonFunction {
    isHaveMenuPermisstion(routerId, currentUserRole){
        const role = Constant.menuData.find(n => n.routerId == routerId);
        if(!role) return false;
        if(role.permission.length <= 0) return true;
        else{
            return currentUserRole.some(n => Constant.menuData.map(g => g.permission).includes(n.roleCode)) 
        }
    }
}