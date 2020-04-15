export const panels = [
  {
    id: "centralFeedPanel",
    path: "__dynamic__",
    loadChildren:
      "./ui/panels/central-feed/central-feed-panel.module#CentralFeedPanelModule",
    legacy: true
  },
  {
    id: "latestActivitiesPanel",
    path: "__dynamic__",
    loadChildren:
      "./ui/panels/latest-activities/latest-activities-panel.module#LatestActivitiesPanelModule",
    legacy: true
  },
  {
    id: "likesPanel",
    path: "__dynamic__",
    loadChildren: "./ui/panels/likes/likes-panel.module#LikesPanelModule",
    legacy: true
  },
  {
    id: "commentsPanel",
    path: "__dynamic__",
    loadChildren:
      "./ui/panels/comments/comments-panel.module#CommentsPanelModule",
    legacy: true
  },
  {
    id: 'userListPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/user-list/user-list-panel.module#UserListPanelModule',
    legacy: true
  },
  {
    id: 'userTagsPanel',
    path: '__dynamic__',
    loadChildren: './ui/panels/user-tags/user-tags-panel.module#UserTagsPanelModule',
    legacy: true
  }
];



