import { UrlType } from 'src/app/shared/enums/url-type';
import { LinkTargetType } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link';

export interface ITextPanelData {
  description?: String,
  link?: IButtonData;
  panelSettings?: any;
  anchor: string;
}

export interface IButtonData {
  type?: UrlType;
  name?: string;
  queryString?: string;
  target?: LinkTargetType;
  url?: string;
}