import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  private prefix = 'ubaseline/api/memberProfile';
  private routeTree = {
    edit: `${this.prefix}/edit`,
    updateNotifierSettings: `${this.prefix}/UpdateNotificationSettings`,
    deletePhoto: `${this.prefix}deletePhoto`
  };

  constructor(private httpClient: HttpClient) { }

  public update(profile): Observable<Response> {
    return this.httpClient.put<Response>(this.routeTree.edit, profile);
  }

  public updateNotificationSettings(settings): Observable<Response> {
    return this.httpClient.put<Response>(this.routeTree.updateNotifierSettings, settings);
  }

  public deletePhoto(photoId): Observable<HttpEvent<Response>> {
    return this.httpClient.delete<Response>(this.routeTree.deletePhoto, photoId);
  }
}
