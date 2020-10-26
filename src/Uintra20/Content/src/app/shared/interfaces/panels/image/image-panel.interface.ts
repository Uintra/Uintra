import { LinkTargetType } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link';

export interface IImagePanel {
  contentTypeAlias: string;
  image: IImage;
  title?: string;
  description?: string;
  link?: IImagePanelLink;
  anchor?: string;
  panelSettings?: any;
}
export interface IImage {
  alt: string;
  height: number;
  sources: ISource[];
  src:  string;
  width: number;
}
export interface ISource {
  media: string; 
  srcSet: string;
};
export interface IImagePanelLink {
  name: string;
  target: LinkTargetType;
  type: number;
  url: string;
}