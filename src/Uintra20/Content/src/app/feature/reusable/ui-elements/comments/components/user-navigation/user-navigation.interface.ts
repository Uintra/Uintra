export interface IUserNavigation {
    currentMember: ICurrentMember;
    items: Array<IItemNavigation>;
}

export interface IItemNavigation {
    name?: string;
    type?: number;
    url?: IUrl;
}

export interface IUrl {
    baseUrl?: string;
    originalUrl?: string;
    params?: Array<IDataUrlParameter>;
}

export interface IDataUrlParameter {
    name?: string;
    data?: string;
}

export interface ICurrentMember {
    displayedName?: string;
    email?: string;
    id?: string;
    inactive?: boolean;
    loginName?: string;
    photo?: string;
    photoId?: number;
    profileLink?: IUrl;
}
