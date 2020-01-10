export const pages = [
    {
        id: 'homePage',
        path: '__dynamic__',
        loadChildren: './ui/pages/home/home-page.module#HomePageModule'
    },
    {
        id: 'socialDetailsPage',
        path: '__dynamic__',
        loadChildren: './ui/pages/social-details/social-details-page.module#SocialDetailsPageModule',
        cache: false
    },
    {
        id: 'articlePage',
        path: '__dynamic__',
        loadChildren: './ui/pages/article/article-page.module#ArticlePageModule'
    }
];
