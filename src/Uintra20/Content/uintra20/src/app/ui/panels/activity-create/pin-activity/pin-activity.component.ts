import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IDatePickerOptions } from 'src/app/feature/shared/interfaces/idatePickerOptions';
import * as moment from "moment";

@Component({
  selector: 'app-pin-activity',
  templateUrl: './pin-activity.component.html',
  styleUrls: ['./pin-activity.component.less']
})
export class PinActivityComponent implements OnInit {
  @Input() isPinCheked: boolean;
  @Output() dateChange = new EventEmitter();
  options: IDatePickerOptions;
  pinDate = null;

  constructor() { }

  ngOnInit() {
    this.options = {
      showClear: true,
      minDate: moment().format()
    };
  }

  onDateChange() {
    this.dateChange.emit(this.pinDate ? this.pinDate.format() : "");
  }
}
