import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { CookieService } from 'ngx-cookie-service';
import { IULink } from 'src/app/feature/shared/interfaces/general.interface';

export interface ISharedNavData {
  linksGroupTitle: string;
  links: Array<ISharedLink>;
}

export interface ISharedLink {
  name: string;
  url: IULink;
  target: "_self" | "_blank";
}

@Injectable({
  providedIn: "root"
})
export class SharedLinksService {
  readonly api = "ubaseline/api/IntranetNavigation";
  readonly openStateProperty = "nav-shared-links-open";

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  getSharedLinks(): Observable<Array<ISharedNavData>> {
    return this.http.get<Array<ISharedNavData>>(this.api + `/SystemLinks`);
  }

  setOpenState(openState: boolean = false): void {
    this.cookieService.set(this.openStateProperty, openState.toString());
  }

  getOpenState(): boolean {
    const cookieData = this.cookieService.get(this.openStateProperty);
    return cookieData === "true";
  }
}
