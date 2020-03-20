export interface ISearchRequestData {
  query: string;
  page: number;
  types: number[];
  onlyPinned: boolean;
}
export interface IFilterData {
  id: number;
  text: string;
}
