export const panels = [
  {
    id: 'centralFeedPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/central-feed/central-feed-panel.module#CentralFeedPanelModule'
  },
  {
    id: 'latestActivitiesPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/latest-activities/latest-activities-panel.module#LatestActivitiesPanelModule'
  },
  {
    id: 'likesPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/likes/likes-panel.module#LikesPanelModule'
  },
  {
    id: 'commentsPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/comments/comments-panel.module#CommentsPanelModule'
  },
  {
    id: 'userListPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/user-list/user-list-panel.module#UserListPanelModule'
  },
  {
    id: 'userTagsPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/user-tags/user-tags-panel.module#UserTagsPanelModule'
  },
  {
    id: 'textPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/text/text-panel.module#TextPanelModule'
  },
  {
    id: 'quotePanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/quote/quote-panel.module#QuotePanelModule'
  },
  {
    id: 'tablePanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/table/table-panel.module#TablePanelModule'
  },
  {
    id: 'imagePanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/image/image-panel.module#ImagePanelModule'
  },
  {
    id: 'linksPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/links/links-panel.module#LinksPanelModule'
  },
  {
    id: 'videoPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/video/video-panel.module#VideoPanelModule'
  },
  {
    id: 'faqPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/faq/faq-panel.module#FaqPanelModule'
  },
  {
    id: 'spotPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/spot/spot-panel.module#SpotPanelModule'
  },
  {
    id: 'contactPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/contact/contact-panel.module#ContactPanelModule'
  },
  {
    id: 'documentLibraryPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/document-library/document-library-panel.module#DocumentLibraryPanelModule'
  },
  {
    id: 'comingEventsPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/coming-events/coming-events-panel.module#ComingEventsPanelModule'
  }
];
