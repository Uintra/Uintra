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
}

export interface IBreadcrumb {
  name: string;
  url: string;
  isClickable: boolean;
}
