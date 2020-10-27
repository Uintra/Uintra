import { IPanelSettings } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings';
import { LinkTargetType } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link';

export interface ILinksPanel {
  contentTypeAlias: string;
  title: string;
  links: ILinksPanelLink[];
  panelSettings?: IPanelSettings;
  anchor?: string;
}
export interface ILinksPanelLink {
  name: string;
  target: LinkTargetType;
  type: number;
  url: string;
}
