import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SocialService {
  private prefix = 'api/Social/';
  private routeTree = {
    update: `${this.prefix}Update`,
    delete: `${this.prefix}Delete`
  };

  constructor(
    private httpClient: HttpClient
  ) { }

  public update(model: any): Observable<any> {
    return this.httpClient.put(this.routeTree.update, model);
  }

  public delete(id: string): Observable<any> {
    return this.httpClient.delete(
      this.routeTree.delete,
      { params: new HttpParams().set('id', id), }
    );
  }
}
