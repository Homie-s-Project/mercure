import {IRoleModel} from "./IRoleModel";

export interface UserModel {
  userId: number;
  lastName: string;
  firstName: string;
  email: string;
  role: IRoleModel;
}


