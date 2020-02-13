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
export interface ICreateGroupModel {
  title: string;
  description: string;
  newMedia: string;
  media: string | null;
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

  createGroup(groupCreateModel: ICreateGroupModel): Observable<ICreateGroupResponse> {
    return this.http.post<ICreateGroupResponse>(groupsApi + '/Create', groupCreateModel)
  }

  setOpenState(openState: boolean = false): void {
    this.cookieService.set(this.openStateProperty, openState.toString());
  }

  getOpenState(): boolean {
    const cookieData = this.cookieService.get(this.openStateProperty);
    return cookieData === "true";
  }
}
