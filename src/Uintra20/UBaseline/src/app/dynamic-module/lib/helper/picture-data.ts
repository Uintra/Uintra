import { IPictureData } from 'src/app/shared/components/picture/picture.component';

export interface IRect {
    width: number;
    height: number;
}
export class PictureData {
    static fromUrlWithRect(url: string, rect: IRect): IPictureData
    {
        return {alt: url, height: rect.height, width: rect.width, src: url, sources: []};
    }
}