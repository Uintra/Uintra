import { IOwner } from 'src/app/feature/specific/activity/activity.interfaces';

export interface ICentralFeedPanel {
    addToSitemap?: boolean;
    contentTypeAlias?: string;
    groupId?: any;
    id?: number;
    isFiltersOpened?: boolean;
    itemsPerRequest?: number;
    name?: any;
    nodeType?: number;
    tabs?: Array<ICentralFeedTab>;
    url?: string;
}

export interface ICentralFeedTab {
    filters?: Array<IFilter>;
    isActive?: boolean;
    links?: any;
    title?: string;
    type?: number;
}

export interface IFilter {
    isActive?: boolean;
    key?: string;
    title?: string;
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
