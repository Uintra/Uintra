export const config = {
    desktopMediaQuery: '(min-width: 768px)',
    api: "/ubaseline/api",
    translateApi: "ubaseline/api/localization/getAll",
    searchApi: "/ubaseline/api/searchapi/",
    autoSuggestApi: "/ubaseline/api/searchapi/autocomplete",
    pages: {
      searchPage: './ui/pages/search-result/search-result.module#SearchResultModule',
      newsOverviewPage: './ui/pages/news-overview/news-overview.module#NewsOverviewModule',
      newsDetailsPage: './ui/pages/news-details/news-details.module#NewsDetailsModule'
    }
}
