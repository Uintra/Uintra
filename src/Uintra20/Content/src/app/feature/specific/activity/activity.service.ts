import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Subject} from "rxjs";
import {
  ISocialCreateModel,
  INewsCreateModel,
  ISocialEdit
} from "./activity.interfaces";

@Injectable({
  providedIn: "root"
})
export class ActivityService {
  private feedRefreshTrigger = new Subject();
  feedRefreshTrigger$ = this.feedRefreshTrigger.asObservable();

  constructor(private http: HttpClient) {}

  submitSocialContent(data: ISocialCreateModel) {
    return this.http
      .post("/ubaseline/api/social/createExtended", data)
      .toPromise();
  }
  updateSocial(model: ISocialEdit) {
    return this.http.put("/ubaseline/api/social/Update", model);
  }


  public deleteSocial(id: string) {
    return this.http.delete("/ubaseline/api/social/Delete", {
      params: new HttpParams().set("id", id)
    });
  }

  updateNews(model: INewsCreateModel) {
    return this.http.put("/ubaseline/api/newsApi/edit", model);
  }

  submitNewsContent(data: INewsCreateModel) {
    return this.http.post("/ubaseline/api/newsApi/create", data);
  }

  createEvent(data) {
    return this.http.post("/ubaseline/api/events/create", data);
  }

  updateEvent(data) {
    return this.http.put("/ubaseline/api/events/edit", data);
  }

  hideEvent(id, isNotificationNeeded) {
    return this.http.post(`/ubaseline/api/events/hide?id=${id}&isNotificationNeeded=${isNotificationNeeded}`, {})
  }

  refreshFeed() {
    this.feedRefreshTrigger.next();
  }
}
