export const pages = [
  {
    id: 'homePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/home/home-page.module#HomePageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'articlePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/article/article-page.module#ArticlePageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'notificationsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/notifications/notifications-page.module#NotificationsPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'socialDetailsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/social/details/social-details-page.module#SocialDetailsPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'socialEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/social/edit/social-edit-page.module#SocialEditPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'profilePage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/profile/profile-details/profile-page.module#ProfilePageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'profileEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/profile/profile-edit/profile-edit-page.module#ProfileEditPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'uintraNewsEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/news/uintra-news-edit/uintra-news-edit-page.module#UintraNewsEditPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'uintraNewsDetailsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/news/uintra-news-details/uintra-news-details-page.module#UintraNewsDetailsPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'uintraGroupsCreatePage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/create/uintra-groups-create-page.module#UintraGroupsCreatePageModule',
    cache: false,
  },
  {
    id: 'uintraGroupsEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/edit/uintra-groups-edit-page.module#UintraGroupsEditPageModule',
    cache: false,
  },
  {
    id: 'uintraGroupsMembersPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/members/uintra-groups-members-page.module#UintraGroupsMembersPageModule',
    cache: false,
  },
  {
    id: 'uintraGroupsRoomPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/room/uintra-groups-room-page.module#UintraGroupsRoomPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'uintraGroupsDocumentsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/documents/uintra-groups-documents-page.module#UintraGroupsDocumentsPageModule',
    cache: false,
  },
  {
    id: 'uintraGroupsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/groups-page/uintra-groups-page.module#UintraGroupsPageModule',
    cache: false
  },
  {
    id: 'uintraMyGroupsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/my-groups-page/uintra-my-groups-page.module#UintraMyGroupsPageModule',
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
    loadChildren: './ui/pages/event/create/event-create-page.module#EventCreatePageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'eventEditPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/event/edit/event-edit-page.module#EventEditPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'eventDetailsPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/event/details/event-details-page.module#EventDetailsPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'searchPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/search/search-page.module#SearchPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'pageNotFoundPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/page-not-found/page-not-found-page.module#PageNotFoundPageModule',
    cache: false,
    legacy: true
  },
  {
    id: 'forbiddenPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/forbidden/forbidden-page.module#ForbiddenPageModule',
    cache: false,
    legacy: true
  },
];

