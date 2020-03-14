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
  tags: Array<ITagData>;
  dates: {
    publishDate: string;
    startDate: string;
    endDate: string;
  },
  location: {
    address: string;
    shortAddress: string;
  },
  isPinned: boolean;
  endPinDate?: string;
  media?: {
    medias?: any[];
    otherFiles?: any[];
  };
}
export interface IPublishDatepickerOptions {
  showClose: boolean;
  minDate?: string | boolean;
  maxDate?: string | boolean;
}
