import { IButtonData } from 'src/app/shared/components/button/button.component';
import { UrlType } from 'src/app/shared/enum/url-type';
import { LinkTargetType } from 'src/app/shared/interface/link';

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