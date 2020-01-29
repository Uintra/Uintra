import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IDatePickerOptioms } from 'src/app/feature/shared/interfaces/idatePickerOptioms';
import * as moment from "moment";

@Component({
  selector: 'app-pin-activity',
  templateUrl: './pin-activity.component.html',
  styleUrls: ['./pin-activity.component.less']
})
export class PinActivityComponent implements OnInit {
  @Input() isPinCheked: boolean;
  @Output() dateChaneg = new EventEmitter();
  options: IDatePickerOptioms;
  pinDate = null;

  constructor() { }

  ngOnInit() {
    this.options = {
      showClear: true,
      minDate: moment().format()
    };
  }

  onDateChange() {
    this.dateChaneg.emit(this.pinDate ? this.pinDate.format() : "");
  }
}
