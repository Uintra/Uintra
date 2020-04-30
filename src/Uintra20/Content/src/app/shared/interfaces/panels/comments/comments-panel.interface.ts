import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';

export interface ICommentsPanel {
    addToSitemap?: boolean;
    canEdit?: boolean;
    contentTypeAlias?: string;
    details?: any;
    errorLink?: any;
    groupHeader?: any;
    id?: number;
    isGroupMember?: boolean;
    name?: string;
    nodeType?: number;
    pageSettings?: any;
    panels?: any;
    requiresRedirect?: boolean;
    statusCode?: number;
    tags?: Array<ITagData>;
    url?: string;
    comments?: any;
    entityId?: any;
    commentsType?: any;
}
export interface ICommentData {
    entityType: number;
    entityId: string;
}
