import { IPanelSettings } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings';
import { ILink } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link';
import { IPictureData } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/picture';

export interface ISpotPanel {
  contentTypeAlias: string;
  columnsCount: number;
  items: any[];
  title?: string;
  description?: string;
  media?: any;
  link?: Object;
  panelSettings?: IPanelSettings;
  anchor?: string;
}
export interface IDefaultSpotData {
  date: string;
  description: string;
  link: ILink;
  media: {image: any, video: any};
  title: string;
}
export interface IModalVideo {
  title: string;
  description: string;
  video: any;
  params?: string;
}
export interface INoImageSpotData {
  title: string;
  description: string;
  link: ILink;
}
export interface ISingleSpotData {
  items: ISingleSpotItem[];
}
export interface ISingleSpotItem {
  title: string;
  description: string;
  link: ILink;
  media: ISingleSpotMedia;
}
export interface ISingleSpotMedia {
  image: IPictureData;
}
