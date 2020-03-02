import { IULink } from "../../general.interface";
import { IUserTag } from "src/app/feature/specific/activity/activity.interfaces";

export interface IProfilePage {
  title: string;
  member: IMember;
  link: IULink;
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
