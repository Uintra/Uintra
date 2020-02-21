import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { CookieService } from "ngx-cookie-service";

export interface IMyLink {
  id: string;
  name: string;
  url: string;
  contentId?: number;
}

@Injectable({
  providedIn: "root"
})
export class MyLinksService {
  readonly api = "ubaseline/api/MyLinks";
  readonly sortStateProperty = "nav-my-links-state";
  readonly openStateProperty = "nav-my-links-open";

  sortState: Array<string>;

  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.updateSortState();
  }

  addMyLinks(): Observable<Array<IMyLink>> {
    return this.http
      .post<Array<IMyLink>>(this.api + `/Add`, {})
      .pipe(map(links => this.sortLinks(links)));
  }

  getMyLinks(): Observable<Array<IMyLink>> {
    return this.http
      .get<Array<IMyLink>>(this.api + `/List`)
      .pipe(map(links => this.sortLinks(links)));
  }

  removeMyLink(id: string) {
    return this.http
      .delete<Array<IMyLink>>(this.api + `/Remove?id=${id}`)
      .pipe(map(links => this.sortLinks(links)));
  }

  setOpenState(openState: boolean = false): void {
    this.cookieService.set(this.openStateProperty, openState.toString());
  }

  getOpenState(): boolean {
    const cookieData = this.cookieService.get(this.openStateProperty);
    return cookieData === "true";
  }

  private sortLinks(links: Array<IMyLink>): Array<IMyLink> {
    let arrayAfterSort = [];

    this.sortState.map(linkId => {
      const foundLink = this.findLinkById(links, linkId);
      if (foundLink) {
        links = this.removeLinkFromArray(links, foundLink);
        arrayAfterSort.push(foundLink);
      }
    });

    return [...arrayAfterSort, ...links];
  }

  public findLinkById(links, id) {
    return links.find(link => link.id === id);
  }
  public removeLinkFromArray(links, targetLink) {
    return links.filter(link => link !== targetLink);
  }

  updateSortState() {
    const cookieData = this.cookieService.get(this.sortStateProperty);
    this.sortState = cookieData ? JSON.parse(cookieData) : [];
  }

  setSortState(data: Array<IMyLink>) {
    this.sortState = data.map(item => item.id) || [];
    this.cookieService.set(
      this.sortStateProperty,
      JSON.stringify(this.sortState)
    );
  }
}
