import { IULink } from 'src/app/shared/interfaces/general.interface';
import { IMember } from 'src/app/shared/interfaces/pages/profile/profile-page.interface';
import { IOwner } from '../activity/activity.interfaces';

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
export interface IUserListRequest {
  text: string;
  page: number;
  groupId: string;
  orderingString: string;
  isInvite: boolean;
}
export interface IDeleteMemberRequest {
  userId: string;
  groupId: string
}
export interface IMemberStatusRequest {
  memberId: string;
  groupId: string
}
export interface IUserListData {
  details: {
    selectedColumns: IUserListSelectedColumn[];
    members: IUserListMember[];
    isLastRequest: boolean;
    currentMember: IOwner;
    isCurrentMemberGroupAdmin: boolean;
    groupId: string;
    isInvite: boolean;
  }
}
export interface IUserListSelectedColumn {
  id: number;
  alias: string;
  name: string;
  type: number;
  propertyName: string;
  supportSorting: boolean;
}
export interface IUserListMember {
  photo: string;
  displayedName: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  department: string;
  member: IOwner;
  profileUrl: IULink;
  isGroupAdmin: boolean;
  isCreator: boolean;
}
