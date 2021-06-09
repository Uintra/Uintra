export const pages = [
  {
    id: 'homePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/home/home-page.module#HomePageModule',
  },
  {
    id: 'articlePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/article/article-page.module#ArticlePageModule',
  },
  {
    id: 'notificationsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/notifications/notifications-page.module#NotificationsPageModule',
  },
  {
    id: 'socialDetailsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/social/details/social-details-page.module#SocialDetailsPageModule',
  },
  {
    id: 'socialEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/social/edit/social-edit-page.module#SocialEditPageModule',
  },
  {
    id: 'profilePage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/profile/profile-details/profile-page.module#ProfilePageModule',
  },
  {
    id: 'profileEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/profile/profile-edit/profile-edit-page.module#ProfileEditPageModule',
  },
  {
    id: 'uintraNewsEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/news/uintra-news-edit/uintra-news-edit-page.module#UintraNewsEditPageModule',
  },
  {
    id: 'uintraNewsDetailsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/news/uintra-news-details/uintra-news-details-page.module#UintraNewsDetailsPageModule',
  },
  {
    id: 'uintraGroupsCreatePage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/create/uintra-groups-create-page.module#UintraGroupsCreatePageModule',
  },
  {
    id: 'uintraGroupsEditPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/edit/uintra-groups-edit-page.module#UintraGroupsEditPageModule',
  },
  {
    id: 'uintraGroupsMembersPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/members/uintra-groups-members-page.module#UintraGroupsMembersPageModule',
  },
  {
    id: 'uintraGroupsRoomPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/room/uintra-groups-room-page.module#UintraGroupsRoomPageModule',
  },
  {
    id: 'uintraGroupsDocumentsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/documents/uintra-groups-documents-page.module#UintraGroupsDocumentsPageModule',
  },
  {
    id: 'uintraGroupsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/groups-page/uintra-groups-page.module#UintraGroupsPageModule',
  },
  {
    id: 'uintraMyGroupsPage',
    path: '__dynamic__',
    loadChildren:
      './ui/pages/uintra-groups/my-groups-page/uintra-my-groups-page.module#UintraMyGroupsPageModule',
  },
  {
    id: 'uintraNewsCreatePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/news/uintra-news-create/uintra-news-create-page.module#UintraNewsCreatePageModule',
  },
  {
    id: 'eventCreatePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/event/create/event-create-page.module#EventCreatePageModule',
  },
  {
    id: 'eventEditPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/event/edit/event-edit-page.module#EventEditPageModule',
  },
  {
    id: 'eventDetailsPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/event/details/event-details-page.module#EventDetailsPageModule',
  },
  {
    id: 'searchPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/search/search-page.module#SearchPageModule',
  },
  {
    id: 'pageNotFoundPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/page-not-found/page-not-found-page.module#PageNotFoundPageModule',
  },
  {
    id: 'forbiddenPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/forbidden/forbidden-page.module#ForbiddenPageModule',
  },
];

