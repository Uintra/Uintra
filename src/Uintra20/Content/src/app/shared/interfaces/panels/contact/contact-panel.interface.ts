import { IPanelSettings } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings';
import { IPictureData } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/picture';

export interface IContactPanel {
  contentTypeAlias: string;
  contacts: IContactPanelItem[];
  title?: string;
  panelSettings?: IPanelSettings;
  anchor?: string;
  utmConfiguration?: any;
  id?: number;
  name?: string;
  nodeType?: number;
  url?: string;
}
export interface IContactPanelItem {
  contentTypeAlias: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  location?: string;
  name?: string;
  phone?: string;
  photo?: IPictureData;
  position?: string;
  id?: number;
  nodeType?: number;
  url?: string;
}