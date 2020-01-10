import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CreateSocialService {
  private feedRefreshTrigger = new Subject();
  feedRefreshTrigger$ = this.feedRefreshTrigger.asObservable();

  constructor(
    private http: HttpClient
  ) { }

  submitSocialContent(data) {
    return this.http.post('/ubaseline/api/social/createExtended', data).toPromise();
  }

  refreshFeed() {
    this.feedRefreshTrigger.next();
  }
}
