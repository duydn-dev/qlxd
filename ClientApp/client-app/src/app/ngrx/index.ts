import { ActionReducer, ActionReducerMap, MetaReducer } from '@ngrx/store';
import { localStorageSync } from 'ngrx-store-localstorage';
import * as fromUser from './reducers/user.reducer';
import * as fromSpinner from './reducers/spinner.reducer';
import * as fromRole from './reducers/role.reducer';

const environment:any = {
  production: false
}
export interface State {
    user: fromUser.State;
    spinner: fromSpinner.State;
    role: fromRole.State;
}
export const reducers: ActionReducerMap<State> = {
    user: fromUser.reducer,
    spinner: fromSpinner.reducer,
    role: fromRole.reducer
};

const reducerKeys = ['user', 'role'];
export function localStorageSyncReducer(reducer: ActionReducer<any>): ActionReducer<any> {
  return localStorageSync({keys: reducerKeys})(reducer);
}
// console.log all actions
export function debug(reducer: ActionReducer<any>): ActionReducer<any> {
    return function(state, action) {
      // console.log('state', state);
      // console.log('action', action);
   
      return reducer(state, action);
    };
  }
   
export const metaReducers: MetaReducer<State>[] = !environment.production ? [debug, localStorageSyncReducer] : [localStorageSyncReducer];