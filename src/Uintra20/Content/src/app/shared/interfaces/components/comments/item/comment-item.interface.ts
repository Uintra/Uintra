import { SafeHtml } from '@angular/platform-browser';

export interface ICommentItem {
    activityId?: string;
    canDelete?: boolean;
    canEdit?: boolean;
    commentViewId?: string;
    createdDate?: string;
    creator?: any;
    creatorProfileUrl?: any;
    elementOverviewId?: string;
    id?: string;
    isReply?: boolean;
    likeModel?: any;
    likedByCurrentUser?: boolean;
    likes?: any;
    linkPreview?: any;
    modifyDate?: any;
    replies?: any;
    text?: SafeHtml;
}
