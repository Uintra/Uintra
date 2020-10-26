import { IULink } from '../../general.interface';

export interface INotificationsPage {
    addToSitemap?: boolean;
    contentTypeAlias?: string;
    id?: number;
    name?: string;
    nodeType?: number;
    notifications?: any;
    notificationsPopUpCount?: number;
    title?: string;
    url?: string;
}

export interface INotificationsData {
    id: string;
    date: string;
    isNotified: boolean;
    isViewed: boolean;
    type: number;

    notifier: {
        id: string;
        displayedName: string;
        photo: string;
        photoId: number;
        email: string;
        loginName: string;
        inactive: boolean;
    };

    value: {
        message: string;
        url: IULink;
        notifierId: string;
        isPinned: boolean;
        isPinActual: boolean;
        desktopMessage: string;
        desktopTitle: string;
        isDesktopNotificationEnabled: boolean;
    };
}

export interface INotificationsListData {
    notificationPageUrl: string;
    notifications: Array<INotificationsData>;
}
