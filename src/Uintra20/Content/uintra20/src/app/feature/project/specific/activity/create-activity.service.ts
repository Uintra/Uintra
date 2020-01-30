import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Subject } from "rxjs";
import {
  ISocialCreateModel,
  INewsCreateModel
} from "./create-activity.interface";

@Injectable({
  providedIn: "root"
})
export class CreateActivityService {
  private feedRefreshTrigger = new Subject();
  feedRefreshTrigger$ = this.feedRefreshTrigger.asObservable();

  constructor(private http: HttpClient) {}

  submitSocialContent(data: ISocialCreateModel) {
    return this.http
      .post("/ubaseline/api/social/createExtended", data)
      .toPromise();
  }

  submitNewsContent(data: INewsCreateModel) {
    //TODO Interface for type
    return this.http.post("/ubaseline/api/newsApi/create", data);
  }

  refreshFeed() {
    this.feedRefreshTrigger.next();
  }
}
