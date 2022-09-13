import { Action, createReducer, on } from '@ngrx/store';
import * as roleAction from '../actions/role.action';

export interface State {
    userRole: any;
}
export const initialState: State = {
    userRole : null
};

const roleReducer = createReducer(
    initialState,
    on(roleAction.setRole, (state, role:any) => ({ ...role })),
    on(roleAction.removeRole, state => {
        return null;
    }),
);

export function reducer(state: State | undefined, action: Action): any {
    return roleReducer(state, action);
}