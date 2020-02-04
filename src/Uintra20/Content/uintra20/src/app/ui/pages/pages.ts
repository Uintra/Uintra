export const pages = [
  {
    id: 'homePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/home/home-page.module#HomePageModule',
    cache: false
  },
  {
    id: 'articlePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/article/article-page.module#ArticlePageModule',
    cache: false
  },
  {
    id: 'notificationsPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/notifications/notifications-page.module#NotificationsPageModule',
    cache: false
  },
  {
    id: 'socialDetailsPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/social/details/social-details-page.module#SocialDetailsPageModule',
    cache: false
  },
  {
    id: 'socialEditPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/social/edit/social-edit-page.module#SocialEditPageModule',
    cache: false
  },
  {
    id: 'profilePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/profile/profile-page.module#ProfilePageModule',
    cache: false
  },
  {
    id: 'profileEditPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/profile-edit/profile-edit-page.module#ProfileEditPageModule',
    cache: false
  },
  {
    id: 'uintraNewsEditPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/uintra-news-edit/uintra-news-edit-page.module#UintraNewsEditPageModule',
    cache: false
  },
];
