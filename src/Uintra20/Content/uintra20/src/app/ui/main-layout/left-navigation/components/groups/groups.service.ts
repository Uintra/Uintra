import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { CookieService } from 'ngx-cookie-service';
import { IUlinkWithTitle } from 'src/app/feature/shared/interfaces/IULink';
import { groupsApi } from 'src/app/constants/general/general.const';

export interface IGroupsData {
  groupPageItem: IUlinkWithTitle;
  items: IUlinkWithTitle[];
}
export interface IGroupModel {
  title: string;
  description: string;
  newMedia: string;
  media: string[] | null;
  id?: string;
}
export interface ICreateGroupResponse {
  id: string;
  title: string;
  description: string;
  createdDate: string;
  updatedDate: string;
  creatorId: string;
  imageId?: string;
  isHidden: boolean;
  groupTypeId: number;
}

@Injectable({
  providedIn: "root"
})
export class GroupsService {
  readonly openStateProperty = "nav-group-links-open";

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  getGroupsLinks(): Observable<IGroupsData> {
    return this.http.get<IGroupsData>(groupsApi + `/LeftNavigation`);
  }

  createGroup(groupCreateModel: IGroupModel): Observable<ICreateGroupResponse> {
    return this.http.post<ICreateGroupResponse>(groupsApi + '/Create', groupCreateModel)
  }

  editGroup(groupEditModel: IGroupModel): Observable<ICreateGroupResponse> {
    return this.http.post<ICreateGroupResponse>(groupsApi + '/Edit', groupEditModel)
  }

  hideGroup(id: string) {
    return this.http.post<any>(groupsApi + `/Hide?groupId=${id}`, {});
  }

  setOpenState(openState: boolean = false): void {
    this.cookieService.set(this.openStateProperty, openState.toString());
  }

  getOpenState(): boolean {
    const cookieData = this.cookieService.get(this.openStateProperty);
    return cookieData === "true";
  }
}
