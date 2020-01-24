import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  private prefix = '/memberProfile/';
  private routeTree = {
    edit: `${this.prefix}edit`,
    updateNotifierSettings: `${this.prefix}updateNotifierSettings`,
    deletePhoto: `${this.prefix}deletePhoto`
  };

  constructor(private httpClient: HttpClient) { }

  public update(profile): Observable<any> {
    return this.httpClient.put(this.routeTree.edit, profile);
  }

  public updateNotificationSettings(settings): Observable<any> {
    return this.httpClient.put(this.routeTree.updateNotifierSettings, settings);
  }

  public deletePhoto(photoId): Observable<any> {
    return this.httpClient.delete(this.routeTree.deletePhoto, photoId);
  }
}
