export const pages = [
    {
        id: 'homePage',
        path: '__dynamic__',
        loadChildren: './ui/pages/home/home-page.module#HomePageModule'
    },
    {
        id: 'bulletinDetailsPage',
        path: '__dynamic__',
        loadChildren: './ui/pages/social-details/module/social-details-page.module#SocialDetailsPageModule'
    },
    {
        id: 'articlePage',
        path: '__dynamic__',
        loadChildren: './ui/pages/article/article-page.module#ArticlePageModule'
    }
];
