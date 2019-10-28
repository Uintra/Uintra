import { DefaultUrlSerializer } from '@angular/router';

export interface ILink {
    url: string;
    anchor: string;
    title: string;
    innerRoute: boolean;
}

export type LinkTargetType = '_blank' | '_self' | '_parent' | '_top';

export function trustUrl(url: string) {
    const urlSerializer = new DefaultUrlSerializer();
    const [ path ] = url.split('?');
    const { queryParams } = urlSerializer.parse(url);

    return { path, queryParams };
}