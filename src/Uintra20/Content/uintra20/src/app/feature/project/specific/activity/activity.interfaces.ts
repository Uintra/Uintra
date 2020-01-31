import { ITagData } from "../../reusable/inputs/tag-multiselect/tag-multiselect.interface";
import { ActivityEnum } from "src/app/feature/shared/enums/activity-type.enum";

export interface ISocialCreateModel {
  description: string;
  ownerId: string;
  newMedia: string;
  tagIdsData: string[];
}

export interface INewsCreateModel {
  ownerId: string;
  title: string;
  description: string;
  publishDate: string;

  unpublishDate?: string;
  media?: Array<any>;
  mediaRootId?: number;
  endPinDate?: string;
  tagIdsData?: string[];

  isPinned?: boolean;
  activityLocationEditModel?: {
    address?: string;
    shortAddress?: string;
  };
  newMedia?: string;
}

export interface ISocialEdit {
  ownerId: string;
  description: string;
  tags: Array<ITagData>;
  availableTags: Array<ITagData>;
  lightboxPreviewModel: ILightBoxPreviewModel;
  id: string;
  name: string;
  tagIdsData: Array<string>;
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

interface IULink {
  originalUrl: string;
  baseUrl: string;
  params: Array<{ name: string; value: string; }>;
}
