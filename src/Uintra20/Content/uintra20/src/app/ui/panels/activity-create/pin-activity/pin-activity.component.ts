import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IDatePickerOptions } from 'src/app/feature/shared/interfaces/idatePickerOptions';
import * as moment from "moment";

export interface IPinedData {
  isAccepted: boolean;
  pinDate: string;
}
@Component({
  selector: 'app-pin-activity',
  templateUrl: './pin-activity.component.html',
  styleUrls: ['./pin-activity.component.less']
})
export class PinActivityComponent implements OnInit {
  @Input() isPinCheked: boolean;
  @Output() dateChange = new EventEmitter<IPinedData>();
  options: IDatePickerOptions;
  pinDate = null;
  pinedDateValue: IPinedData = {
    isAccepted: false,
    pinDate: ""
  };

  constructor() { }

  ngOnInit() {
    this.options = {
      showClear: true,
      minDate: moment().format()
    };
  }

  onDateChange() {
    this.pinedDateValue.pinDate = this.pinDate ? this.pinDate.format() : "";
    this.dateChange.emit(this.pinedDateValue);
  }
  onAcceptedChange() {
    this.dateChange.emit(this.pinedDateValue);
  }
}
