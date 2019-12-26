import { ActivityEnum } from 'src/app/feature/shared/enums/activity-type.enum';

export interface ISocialDetails {
    activityType: ActivityEnum;
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
export interface IOwner {
    displayedName: string;
    email: string;
    id: string;
    loginName: string;
    photo: string;
    photoId: number;
}
