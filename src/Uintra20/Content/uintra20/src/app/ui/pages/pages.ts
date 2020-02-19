export const pages = [
  {
    id: "homePage",
    path: "__dynamic__",
    loadChildren: "./ui/pages/home/home-page.module#HomePageModule",
  },
  {
    id: "articlePage",
    path: "__dynamic__",
    loadChildren: "./ui/pages/article/article-page.module#ArticlePageModule",
  },
  {
    id: "notificationsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/notifications/notifications-page.module#NotificationsPageModule",
  },
  {
    id: "socialDetailsPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/social/details/social-details-page.module#SocialDetailsPageModule",
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
    loadChildren: "./ui/pages/profile/profile-page.module#ProfilePageModule",
  },
  {
    id: "profileEditPage",
    path: "__dynamic__",
    loadChildren:
      "./ui/pages/profile-edit/profile-edit-page.module#ProfileEditPageModule",
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
  },
  {
    id: 'uintraGroupsCreatePage',
    path: '__dynamic__',
    loadChildren: './ui/pages/uintra-groups/create/uintra-groups-create-page.module#UintraGroupsCreatePageModule'
  },
  {
    id: 'uintraGroupsEditPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/uintra-groups/edit/uintra-groups-edit-page.module#UintraGroupsEditPageModule'
  },
  {
    id: 'uintraGroupsMembersPage',
    path: '__dynamic__',
    loadChildren: './ui/pages/uintra-groups/members/uintra-groups-members-page.module#UintraGroupsMembersPageModule'
  },
];

