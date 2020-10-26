import { IUserTag } from 'src/app/feature/specific/activity/activity.interfaces';

export interface IProfilePage {
  addToSitemap: boolean;
  contentTypeAlias: string;
  editProfileLink: any;
  errorLink: any;
  id: number;
  name: string;
  nodeType: number;
  pageSettings: any;
  panels: any;
  profile: any;
  requiresRedirect: boolean;
  statusCode: number;
  tags: any;
  url: string;
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


