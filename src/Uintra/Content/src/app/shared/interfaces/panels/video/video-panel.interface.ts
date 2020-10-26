import { IPictureData } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/picture/picture.component';
import { IPanelSettings } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings';

export interface IVideoPanel {
  contentTypeAlias: string;
  title?: string;
  description?: string;
  video?: IVideoPickerData;
  panelSettings?: IPanelSettings;
  anchor: string;
}
export interface IVideoViewModel {
  video: object;
  thumbnail: IPictureData;
  title: string;
  description: string;
  params: string;
}
export interface IVideoPickerData {
  desktop?: IVideoPickerVideoData;
  mobile?: IVideoPickerVideoData;
}
export interface IVideoPickerVideoData {
  loop: boolean;
  thumbnail: string;
  type: number;
  url: string;
  videoCode: string;
}