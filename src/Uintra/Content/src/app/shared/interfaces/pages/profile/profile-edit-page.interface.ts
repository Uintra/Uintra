import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';

export interface IProfileEditPage {
  profile: IProfile;
  title: string;
  addToSitemap: boolean;
  availableTags: Array<ITagData>;
  contentTypeAlias: string;
  id: number;
  name: string;
  nodeType: number;
  pageSettings: any;
  panels: any;
  tags: Array<ITagData>;
  url: string;
}

export interface IProfile {
  id: string;
  firstName: string;
  lastName: string;
  phone: string;
  department: string;
  photo: string;
  photoId: string;
  email: string;
  profileUrl: string;
  mediaRootId: string;
  newMedia: string;
  memberNotifierSettings: IMemberNotifierSettings;
  tags: Array<ITagData>;
  availableTags: Array<ITagData>;
}

export interface IMemberNotifierSettings {
  uiNotifier: boolean;
  desktopNotifier: boolean;
  emailNotifier: boolean;
  popupNotifier: boolean;
}
