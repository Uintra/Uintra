import { IULink } from 'src/app/shared/interfaces/general.interface';

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
export interface IAutocompleteItem {
  title: string;
  url: IULink;
  item: {
    title: string;
    type: string;
    photo: string;
    email: string;
  }
}
export interface IMapedAutocompleteItem {
  title: string;
  url: IULink;
  isActive: boolean;
  item: {
    title: string;
    type: string;
    photo: string;
    email: string;
  }
}
