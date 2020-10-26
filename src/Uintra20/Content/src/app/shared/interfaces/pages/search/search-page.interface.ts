import { IFilterData, ISearchResult } from 'src/app/feature/specific/search/search.interface';

export interface ISearchPage {
    addToSitemap?: boolean;
    allTypesPlaceholder?: string;
    blockScrolling?: boolean;
    contentTypeAlias?: string;
    filterItems?: IFilterData[];
    id?: number;
    name?: string;
    nodeType?: number;
    query?: string;
    results?: ISearchResult[];
    resultsCount?: number;
    url?: string;
}
