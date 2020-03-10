import { UmbracoFlatPropertyModel } from "@ubaseline/next";
import { IOwner } from "src/app/feature/specific/activity/activity.interfaces";

export interface ICentralFeedPanel {
  contentTypeAlias: string;
  tabs: UmbracoFlatPropertyModel;
  groupId: any;
}

export interface IPublicationsResponse {
  type: number;
  feed: IPublication[];
  tabSettings: {
    type: number;
    hasSubscribersFilter: boolean;
    hasPinnedFilter: boolean;
  };
  blockScrolling: boolean;
  isReadOnly: boolean;
}

export interface IPublication {
  id: string;
  canEdit: boolean;
  isPinned: boolean;
  isPinActual: boolean;
  title: string;
  owner: IOwner;
  dates: string[];
  links: any;
  location: any;
  isReadOnly: boolean;
  type: string;
  activityType: number;
  likes: any;
  likedByCurrentUser: boolean;
  commentsCount: number;
  mediaPreview: any;
  description: string;
  groupInfo: any;
  isGroupMember: boolean;
}

export interface IFilterState {
  key: string;
  title: string;
  isActive: boolean;
}
