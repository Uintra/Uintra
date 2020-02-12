import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IDatepickerData } from '../../../reusable/inputs/datepicker-from-to/datepiker-from-to.interface';

@Injectable({
  providedIn: 'root'
})
export class PinActivityService {

  constructor() { }

  private publishDates = new Subject<IDatepickerData>();
  publishDates$ = this.publishDates.asObservable();

  setPublishDates(dates: IDatepickerData) {
      this.publishDates.next(dates);
  }
}
