import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class SocialService {
  private readonly apiPrefix = "api/Social";
  private readonly apiRouteTree = {
    update: `${this.apiPrefix}/Update`,
    delete: `${this.apiPrefix}/Delete`
  };

  constructor(private httpClient: HttpClient) {}

  public update(model: any): Observable<any> {
    return this.httpClient.put(this.apiRouteTree.update, model);
  }

  public delete(id: string): Observable<any> {
    return this.httpClient.delete(this.apiRouteTree.delete, {
      params: new HttpParams().set("id", id)
    });
  }
}
