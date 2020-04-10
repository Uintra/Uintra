import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ISearchRequestData, IAutocompleteItem, ISearchData, IDeleteMemberRequest, IMemberStatusRequest, IUserListRequest, IUserListData } from './search.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(
    private http: HttpClient,
  ) { }

  autocomplete(query: string): Observable<IAutocompleteItem[]> {
    return this.http.post<IAutocompleteItem[]>("/ubaseline/api/search/autocomplete", {query: query});
  }

  search(data: ISearchRequestData): Observable<ISearchData> {
    return this.http.post<ISearchData>("/ubaseline/api/search/search", data)
  }

  userListSearch(data: IUserListRequest): Observable<IUserListData> {
    return this.http.post<IUserListData>("/ubaseline/api/UserList/GetUsers", data)
  }

  changeMemberStatus(data: IMemberStatusRequest) {
    return this.http.put("/ubaseline/api/userlist/assign", data);
  }

  deleteMember(data : IDeleteMemberRequest) {
    return this.http.delete(`/ubaseline/api/userList/ExcludeUserFromGroup?groupId=${data.groupId}&userId=${data.userId}`);
  }
}
