import { IUserTag } from "src/app/feature/project/specific/activity/activity.interfaces";

export interface IProfilePage {
  title: string;
  member: IMember;
  link: ILinkModel;
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

export interface ILinkModel {
  originalUrl: string;
  baseUrl: string;
  params: Array<string>;
}
