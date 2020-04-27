import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  ISearchRequestData,
  IAutocompleteItem,
  ISearchData,
  IDeleteMemberRequest,
  IMemberStatusRequest,
  IUserListRequest,
  IUserListData
} from './search.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  private prefix = '/ubaseline/api/';

  constructor(private http: HttpClient) { }

  public autocomplete = (query: string): Observable<IAutocompleteItem[]> =>
    this.http.post<IAutocompleteItem[]>(`${this.prefix}search/autocomplete`, { query });

  public search = (data: ISearchRequestData): Observable<ISearchData> =>
    this.http.post<ISearchData>(`${this.prefix}search/search`, data)

  public userListSearch = (data: IUserListRequest): Observable<IUserListData> =>
    this.http.post<IUserListData>(`${this.prefix}UserList/GetUsers`, data)

  public changeMemberStatus = (data: IMemberStatusRequest): Observable<any> =>
    this.http.put(`${this.prefix}userlist/assign`, data)

  public deleteMember = (data: IDeleteMemberRequest): Observable<any> =>
    this.http.delete(`${this.prefix}userList/ExcludeUserFromGroup?groupId=${data.groupId}&userId=${data.userId}`)
}
