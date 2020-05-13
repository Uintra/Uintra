import { LinkTargetType, UrlType } from './link';

export interface IButtonData {
    type?: UrlType;
    name?: string;
    queryString?: string;
    target?: LinkTargetType;
    url?: string;
}

export class ButtonData implements IButtonData {

    constructor(
        public type?: UrlType,
        public name?: string,
        public target?: LinkTargetType,
        public url?: string,
        public queryString?: string
    ) {}

    static fromUrlWithName(url: string, name: string): IButtonData
    {
        return new ButtonData(UrlType.Node, name, '_self', url, '');
    }

    static fromUrl(url: string, sameName = true): IButtonData
    {
        return new ButtonData(UrlType.Node, sameName ? url : '', '_self', url, '');
    }
}