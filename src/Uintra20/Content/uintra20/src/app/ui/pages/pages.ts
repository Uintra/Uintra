export const pages = [
    {
        id: 'homePage',
        path: '__dynamic__',
        loadChildren: './ui/pages/home/home-page.module#HomePageModule'
    },

    {
        id: 'articlePage',
        path: '__dynamic__',
        loadChildren: './ui/pages/article/article-page.module#ArticlePageModule'
    }
];
