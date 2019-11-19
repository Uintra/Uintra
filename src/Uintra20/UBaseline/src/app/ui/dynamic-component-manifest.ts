import { DynamicComponentManifest } from "../shared/dynamic-component-loader/dynamic-component.manifest";
import { specificComponents } from './project-specific-component-manifest';

const baseComponents: DynamicComponentManifest[] = [
  {
    componentId: 'cookie',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/cookie/cookie.module#CookieModule'
  },
  {
    componentId: 'heroPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/hero-panel/hero-panel.module#HeroPanelModule'
  },
  {
    componentId: 'articleStartPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/article-start-panel/article-start-panel.module#ArticleStartPanelModule'
  },
  {
    componentId: 'articleContinuedPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/article-continued-panel/article-continued-panel.module#ArticleContinuedPanelModule'
  },
  {
    componentId: 'iconPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/icon-panel/icon-panel.module#IconPanelModule'
  },
  {
    componentId: 'faqPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/faq-panel/faq-panel.module#FaqPanelModule'
  },
  {
    componentId: 'spotPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/spot-panel/spot-panel.module#SpotPanelModule'
  },
  {
    componentId: 'imagePanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/image-panel/image-panel.module#ImagePanelModule'
  },
  {
    componentId: 'videoPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/video-panel/video-panel.module#VideoPanelModule'
  },
  {
    componentId: 'quotePanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/lib/panel/quote-panel/quote-panel.module#QuotePanelModule'
  },
  {
    componentId: 'topImagePanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/top-image-panel/top-image-panel.module#TopImagePanelModule'
  },
  {
    componentId: 'autoSuggest',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/autosuggest-panel/autosuggest-panel.module#AutosuggestPanelModule'
  },
  {
    componentId: 'contactPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/contact-panel/contact-panel.module#ContactPanelModule'
  },
  {
    componentId: 'mailchimpPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/lib/panel/subscribe-panel/subscribe-panel.module#SubscribePanelModule'
  },
  {
    componentId: 'newsPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/news-panel/news-panel.module#NewsPanelModule'
  },
  {
    componentId: 'tablePanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/table-panel/table-panel.module#TablePanelModule'
  },
  {
    componentId: 'documentLibraryPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/lib/panel/document-library-panel/document-library-panel.module#DocumentLibraryPanelModule'
  },
  {
    componentId: 'linksPanel',
    path: 'dynamic-module',
    loadChildren: './dynamic-module/links-panel/links-panel.module#LinksPanelModule'
  }
];

export const dynamicComponents = [...baseComponents, ...specificComponents];
