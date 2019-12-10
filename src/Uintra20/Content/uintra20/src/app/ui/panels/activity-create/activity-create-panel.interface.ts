import { UmbracoFlatPropertyModel } from '@ubaseline/next';

export interface IActivityCreatePanel {
  contentTypeAlias: UmbracoFlatPropertyModel;
  creator: UmbracoFlatPropertyModel;
  dates: UmbracoFlatPropertyModel;
  tabType: UmbracoFlatPropertyModel;
  tags: UmbracoFlatPropertyModel;
}
