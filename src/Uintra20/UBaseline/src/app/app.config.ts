export const config = {
    desktopMediaQuery: '(min-width: 768px)',
    api: "/umbraco/api",
    translateApi: "umbraco/api/localization/getAll",
    searchApi: "/umbraco/api/searchapi/",
    autoSuggestApi: "/umbraco/api/searchapi/autocomplete",
    pages: {
      searchPage: './ui/pages/search-result/search-result.module#SearchResultModule',
      newsOverviewPage: './ui/pages/news-overview/news-overview.module#NewsOverviewModule',
      newsDetailsPage: './ui/pages/news-details/news-details.module#NewsDetailsModule'
    }
}
