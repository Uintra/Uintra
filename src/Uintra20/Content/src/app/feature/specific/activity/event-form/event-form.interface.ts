import { ActivityEnum } from '../../../../shared/enums/activity-type.enum';
import { IHeaderInfo, ILocation, IActivityLinks } from '../activity.interfaces';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';

export interface IEventsInitialDates {
  publishDate: string;
  from: string;
  to: string;
}

export interface IEventCreateModel {
  ownerId: string;
  title: string;
  description: string;
  publishDate: string;
  startDate: string;
  endDate: string;
  location: {
    address: string;
    shortAddress: string;
  };
  pinAllowed: boolean;
  isPinned: boolean;
  tagIdsData?: string[];
  locationTitle?: string;
  canSubscribe?: boolean;
  subscribeNotes?: string;
  endPinDate?: string;
  newMedia?: string;
  media?: {
    medias?: any[];
    otherFiles?: any[];
  };
  groupId?: string;
}

export interface IEventDetails {
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
  locationTitle: string;
}

export interface IPublishDatepickerOptions {
  showClose: boolean;
  format?: string;
  minDate?: any;
  maxDate?: any;
  ignoreReadonly?: boolean;
  debug?: boolean;
}

export interface IEventForm {
  allowedMediaExtensions?: string;
  canEditOwner?: boolean;
  creator?: any;
  endDate?: string;
  groupId?: string;
  links?: any;
  members?: any;
  pinAllowed?: boolean;
  publishDate?: string;
  startDate?: string;
  selectedTags: any;
  location: any;
  endPinDate: any;
  availableTags: Array<ITagData>;
  tags?: Array<ITagData>;
}
