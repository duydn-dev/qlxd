// đây là action
import { createAction, props } from '@ngrx/store';
export const USER_ROLE = '[USER_ROLE] USER';
export const REMOVE_ROLE = '[REMOVE_ROLE] USER';

export const setRole = createAction(
  USER_ROLE,
  props<{ role: String }>()
);
export const removeRole = createAction(
  REMOVE_ROLE
);