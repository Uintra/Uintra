export interface IEventsInitialDates {
  publishDate: string;
  startDate: string;
  endDate: string;
}
export interface IEventCreateModel {
  ownerId: string;
  title: string;
  description: string;
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
  media?: {
    medias?: any[];
    otherFiles?: any[];
  };
}
