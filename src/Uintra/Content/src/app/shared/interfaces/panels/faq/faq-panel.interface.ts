import { IPanelSettings } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings';

export interface IAccordionPanel {
  contentTypeAlias: string;
  title?: string,
  description?: string;
  items?: IAccordionItem[];
  panelSettings?: IPanelSettings;
}
export interface IAccordionItem {
  question: string;
  answer: string;
  contentTypeAlias: string;
  anchor?: string;
  id?: string;
  name?: string;
  nodeType?: number;
  url?: string;
}