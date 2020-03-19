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
  },
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
export interface IPublishDatepickerOptions {
  showClose: boolean;
  minDate?: string | boolean;
  maxDate?: string | boolean;
}
