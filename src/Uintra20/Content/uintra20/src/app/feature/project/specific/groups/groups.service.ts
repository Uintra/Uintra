import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { CookieService } from 'ngx-cookie-service';
import { IULink } from 'src/app/feature/shared/interfaces/general.interface';
import { groupsApi } from 'src/app/constants/general/general.const';
import { IGroupsData, IGroupDetailsHeaderData, IGroupModel } from './groups.interface';

@Injectable({
  providedIn: "root"
})
export class GroupsService {
  readonly openStateProperty = "nav-group-links-open";

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  getGroupsLinks(): Observable<IGroupsData> {
    return this.http.get<IGroupsData>(groupsApi + `/LeftNavigation`);
  }

  getGroupDetailsLinks(id: string): Observable<IGroupDetailsHeaderData> {
    return this.http.get<IGroupDetailsHeaderData>(groupsApi + `/Header?groupId=${id}`);
  }

  createGroup(groupCreateModel: IGroupModel): Observable<IULink> {
    return this.http.post<IULink>(groupsApi + '/Create', groupCreateModel)
  }

  editGroup(groupEditModel: IGroupModel): Observable<IULink> {
    return this.http.post<IULink>(groupsApi + '/Edit', groupEditModel)
  }

  hideGroup(id: string): Observable<IULink> {
    return this.http.post<IULink>(groupsApi + `/Hide?id=${id}`, {});
  }

  getGroups(isMyGroups: boolean, pageNumber: number) {
    return this.http.get(`/ubaseline/api/Group/List?isMyGroupsPage=${isMyGroups}&page=${pageNumber}`).toPromise();
  }

  setOpenState(openState: boolean = false): void {
    this.cookieService.set(this.openStateProperty, openState.toString());
  }

  getOpenState(): boolean {
    const cookieData = this.cookieService.get(this.openStateProperty);
    return cookieData === "true";
  }
}
