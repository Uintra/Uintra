import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";

export interface ISharedNavData {
  linksGroupTitle: string;
  links: Array<ISharedLink>;
}

export interface ISharedLink {
  name: string;
  url: {
    originalUrl: string;
    baseUrl: string;
    params: Array<object>;
  };
  target: "_self" | "_blank";
}

@Injectable({
  providedIn: "root"
})
export class SharedLinksService {
  readonly api = "ubaseline/api/IntranetNavigation";

  constructor(private http: HttpClient) {}

  getSharedLinks(): Observable<Array<ISharedNavData>> {
    return this.http.get<Array<ISharedNavData>>(this.api + `/SystemLinks`);
  }
}
