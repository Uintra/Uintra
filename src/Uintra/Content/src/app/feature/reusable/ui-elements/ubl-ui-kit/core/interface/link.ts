import { IButtonData } from './button';

export interface ILink {
    url: string;
    innerRoute: boolean;
    target: LinkTargetType;
    queryParams: {[key: string]: string}
    label: string;
}

export enum UrlType {
    Node = 0,
    Media = 1,
    External = 2,
}

export type LinkTargetType = '_blank' | '_self' | '_parent' | '_top';

export class Link implements ILink {
    queryParams = {};

    constructor(
        public url: string,
        public label: string,
        public innerRoute: boolean,
        public target: LinkTargetType,
    ) {

        if (innerRoute)
        {
            const {path, params} = this.splitUrlToPathWithParams(url);
            this.url = path;
            this.queryParams = params;
        }
    }


    static fromButtonData(data: IButtonData)
    {
        if (!data) throw new Error(`Can't create Link from ${data}`);

        const innerRoute = data.type !== UrlType.External ? true : false;
        const target = data.target && data.target || '_self';

        return new Link(data.url, data.name, innerRoute, target);
    }

    private splitUrlToPathWithParams(url: string)
    {
        let path = '';
        const params = {};
        const parts = url.split('?');
        path = parts[0];

        if (parts[1])
        {
            parts[1].split('&').forEach(segment => {
                const [key, val] = segment.split('=');
                params[key] = val
            });
        }

        return {path, params}
    }
}