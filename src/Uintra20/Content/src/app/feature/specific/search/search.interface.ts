export interface ISearchRequestData {
  query: string;
  page: number;
  types: number[];
  onlyPinned: boolean;
}
export interface IFilterData {
  id: number;
  name: string;
}
export interface IMapedFilterData {
  id: number;
  text: string;
}
export interface ISearchResult {
  id: string;
  title: string;
  description: string;
  panels: any[];
  url: string;
  type: string;
  publishedDate: string;
  startDate: string;
  endDate: string;
  isPinned: boolean;
  isPinActual: boolean;
}
export interface ISearchData {
  results: ISearchResult[];
  filterItems: IFilterData[];
  query: string;
  resultsCount: number;
  blockScrolling: boolean;
  allTypesPlaceholder: string;
}
