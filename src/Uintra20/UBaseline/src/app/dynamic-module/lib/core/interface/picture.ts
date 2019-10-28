export interface IPictureData {
    alt: string;
    height: number;
    width: number;
    src: string;
    sources: IPictureSource[]
  }

export interface IPictureSource {
    media: string;
    srcSet: string;
}
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