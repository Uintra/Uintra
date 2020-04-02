import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeedStoreService {

  private store = new BehaviorSubject(null);
  private state = this.store.asObservable();

  constructor() {
  }

  public set update(feed) {
    this.store.next(feed);
  }

  public get current(): any {
    return this.state;
  }
}
