import { IUserTag } from "src/app/feature/project/specific/activity/activity.interfaces";

export interface IProfilePage {
  title: string;
  member: IMember;
}
export interface IMember {
  photo: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  department: string;
  tags: Array<IUserTag>;
}
