import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DatepickerService {

  private publishDateState = new Subject();

  constructor() { }

  public getPublishDate() {
    return this.publishDateState.asObservable();
  }

  public setPublishDate(state: boolean) {
    this.publishDateState.next(state);
  }

  public handlePublishDateState(value: boolean) {
    if (value === null) {
      this.setPublishDate(true);

      return true;
    } else {
      this.setPublishDate(false);

      return false;
    }
  }
}
