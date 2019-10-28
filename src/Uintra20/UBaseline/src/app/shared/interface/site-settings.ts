import { IPictureData } from '../components/picture/picture.component';

export interface ISiteSettings {
    headerSettings: ISiteHeaderSettings;
    footerSettings: ISiteFooterSettings;
    cookieSettings: ISiteCookieSettings;
    pageTitleSeparator: string;
}

export interface ISiteHeaderSettings {
    headerSubLinks: string;
    siteTitle: string;
    siteLogo: IPictureData;
}

export interface ISiteFooterSettings {
    footerLogo: IPictureData;
    footerItems: any[];
}

export interface ISiteCookieSettings {
    cookieTitle: string;
    cookieDescription: string;
}