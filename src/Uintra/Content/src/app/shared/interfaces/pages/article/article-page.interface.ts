export interface IArcticlePage {
  addToSitemap?: boolean;
  contentTypeAlias?: string;
  groupId?: any;
  id?: number;
  localScripts?: any;
  metaData?: any;
  name?: string;
  nodeType?: number;
  panels?: any;
  rightColumnPanels?: any;
  showInSubMenu?: any;
  title?: string;
  url?: string;
  utmConfiguration?: any;
  breadcrumbs: Array<IBreadcrumb>;
  subNavigation: ISubNavigation;
}

export interface IBreadcrumb {
  name: string;
  url: string;
  isClickable: boolean;
}

export interface ISubNavigation {
  active: boolean;
  currentItem: boolean;
  id: number;
  name: string;
  subItems: ISubNavigation[];
  url: string;
}
