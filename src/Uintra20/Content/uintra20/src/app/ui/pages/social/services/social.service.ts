import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ISocialEdit } from '../edit/social-edit-page.interface';

@Injectable({
  providedIn: "root"
})
export class SocialService {
  private readonly apiPrefix = "api/Social";
  private readonly apiRouteTree = {
    update: `${this.apiPrefix}/Update`,
    delete: `${this.apiPrefix}/Delete`
  };

  constructor(private httpClient: HttpClient) { }

  public update(model: ISocialEdit): Observable<Response> {
    return this.httpClient.put<Response>(this.apiRouteTree.update, model);
  }

  public delete(id: string): Observable<Response> {
    return this.httpClient.delete<Response>(this.apiRouteTree.delete, {
      params: new HttpParams().set("id", id)
    });
  }
}
