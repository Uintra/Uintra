import { ITagData } from "../../reusable/inputs/tag-multiselect/tag-multiselect.interface";
import { ActivityEnum } from "src/app/feature/shared/enums/activity-type.enum";
import { IULink } from 'src/app/feature/shared/interfaces/IULink';

export interface ISocialCreateModel {
  description: string;
  ownerId: string;
  newMedia: string;
  tagIdsData: string[];
  groupId?: string;
}

export interface INewsCreateModel {
  ownerId: string;
  title: string;
  description: string;
  publishDate: string;

  unpublishDate?: string;
  media?: {
    medias?: any[];
    otherFiles?: any[]
  };
  mediaRootId?: number;
  endPinDate?: string;
  tagIdsData?: string[];

  isPinned?: boolean;
  location?: {
    address?: string;
    shortAddress?: string;
  };
  newMedia?: string;
  tags?: ITagData[];
}

export interface ISocialEdit {
  ownerId: string;
  description: string;
  tags: Array<ITagData>;
  availableTags: Array<ITagData>;
  lightboxPreviewModel: ILightBoxPreviewModel;
  id: string;
  groupId?: string;
  links: IActivityLinks;
  name: string;
  tagIdsData: Array<string>;
  newMedia: string;
  media: string;
  mediaRootId: number;

  location?: {
    address?: string;
    shortAddress?: string;
  }
}

export interface ILightBoxPreviewModel {
  medias: Array<IMedia>;
  otherFiles: Array<IDocument>;
  hiddenImagesCount: number;
  additionalImages: number;
  filesToDisplay: number;
}

export interface ISocialDetails {
  activityType: ActivityEnum;
  activityName: string;
  canEdit: boolean;
  description: string;
  headerInfo: IHeaderInfo;
  id: string;
  isPinned: boolean;
  isReadOnly: boolean;
  publishDate: Date;
  links: IActivityLinks;
  location: ILocation;
}
export interface ILocation {
  address: string;
  shortAddress: string;
}
export interface IHeaderInfo {
  activityId: string;
  dates: Array<string>;
  owner: IOwner;
  title: string;
  type: number;
}
export interface IOwner {
  displayedName: string;
  email: string;
  id: string;
  loginName: string;
  photo: string;
  photoId: number;
}

export interface IUserTag {
  id: string;
  text: string;
}

export interface ISocialAttachment {
  id: string;
  key: string;
  url: string;
  name: string;
  previewUrl: string;
  isHidden: string;
  width: number;
  height: number;
  extension: string;
}

export interface IMedia extends ISocialAttachment {}
export interface IDocument extends ISocialAttachment {}



// TODO: maybe global interfaces
interface IActivityLinks {
  details: IULink;
  edit: IULink;
  feed: IULink;
  overview: IULink;
  create: IULink;
  owner: IULink;
  detailsNoId: IULink;
}
