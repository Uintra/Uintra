export const pages = [
  {
    id: "homePage",
    path: "__dynamic__",
    loadChildren: "./ui/pages/home/home-page.module#HomePageModule",
    cache: false
  },
  {
    id: "articlePage",
    path: "__dynamic__",
    loadChildren: "./ui/pages/article/article-page.module#ArticlePageModule",
    cache: false
  },
  {
    id: "notificationsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/notifications/notifications-page.module#NotificationsPageModule",
    cache: false
  },
  {
    id: "socialDetailsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/social/details/social-details-page.module#SocialDetailsPageModule",
    cache: false
  },
  {
    id: "socialEditPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/social/edit/social-edit-page.module#SocialEditPageModule",
    cache: false
  },
  {
    id: "profilePage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/profile/profile-details/profile-page.module#ProfilePageModule",
    cache: false
  },
  {
    id: "profileEditPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/profile/profile-edit/profile-edit-page.module#ProfileEditPageModule",
    cache: false
  },
  {
    id: "uintraNewsEditPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/news/uintra-news-edit/uintra-news-edit-page.module#UintraNewsEditPageModule",
    cache: false
  },
  {
    id: "uintraNewsDetailsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/news/uintra-news-details/uintra-news-details-page.module#UintraNewsDetailsPageModule",
    cache: false
  },
  {
    id: "uintraGroupsCreatePage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/create/uintra-groups-create-page.module#UintraGroupsCreatePageModule",
    cache: false
  },
  {
    id: "uintraGroupsEditPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/edit/uintra-groups-edit-page.module#UintraGroupsEditPageModule",
    cache: false
  },
  {
    id: "uintraGroupsMembersPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/members/uintra-groups-members-page.module#UintraGroupsMembersPageModule",
    cache: false
  },
  {
    id: "uintraGroupsRoomPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/room/uintra-groups-room-page.module#UintraGroupsRoomPageModule",
    cache: false
  },
  {
    id: "uintraGroupsDocumentsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/documents/uintra-groups-documents-page.module#UintraGroupsDocumentsPageModule",
    cache: false
  },
  {
    id: "uintraGroupsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/groups-page/uintra-groups-page.module#UintraGroupsPageModule",
    cache: false
  },
  {
    id: "uintraMyGroupsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/uintra-groups/my-groups-page/uintra-my-groups-page.module#UintraMyGroupsPageModule",
    cache: false
  },
  {
    id: 'uintraNewsCreatePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/news/uintra-news-create/uintra-news-create-page.module#UintraNewsCreatePageModule',
    cache: false
  },
  {
    id: 'eventCreatePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/event/create/event-create-page.module#EventCreatePageModule'
  },
  {
    id: 'pageNotFoundPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/page-not-found/page-not-found-page.module#PageNotFoundPageModule'
  },
  {
    id: 'forbiddenPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/forbidden/forbidden-page.module#ForbiddenPageModule'
  },
];
