import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CreateActivityService {
  private feedRefreshTrigger = new Subject();
  feedRefreshTrigger$ = this.feedRefreshTrigger.asObservable();

  constructor(
    private http: HttpClient
  ) { }

  submitSocialContent(data) { //TODO Interface for type
    return this.http.post('/ubaseline/api/social/createExtended', data).toPromise();
  }

  submitNewsContent(data) { //TODO Interface for type
    return this.http.post('/ubaseline/api/newsApi/create', data).toPromise();
  }

  refreshFeed() {
    this.feedRefreshTrigger.next();
  }
}
