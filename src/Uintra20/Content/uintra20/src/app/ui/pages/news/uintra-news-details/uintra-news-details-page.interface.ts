import { ActivityEnum } from "src/app/feature/shared/enums/activity-type.enum";
import { IOwner } from "src/app/feature/shared/interfaces/Owner";

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
}
export interface IHeaderInfo {
  activityId: string;
  dates: Array<string>;
  owner: IOwner;
  title: string;
  type: number;
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
