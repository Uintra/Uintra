import { Component, OnInit, Output, EventEmitter, Input, ViewEncapsulation } from "@angular/core";
import * as moment from "moment";
import { IDatePickerOptions } from "src/app/feature/shared/interfaces/DatePickerOptions";
import { IDatepickerData } from './datepiker-from-to.interface';

@Component({
  selector: "app-datepicker-from-to",
  templateUrl: "./datepicker-from-to.component.html",
  styleUrls: ["./datepicker-from-to.component.less"],
  encapsulation: ViewEncapsulation.None
})
export class DatepickerFromToComponent implements OnInit {
  @Input() initialValues: { from: string; to: string } = null;
  @Output() handleChange = new EventEmitter();

  fromDate = null;
  toDate = null;
  optFrom: IDatePickerOptions = {
    minDate: moment(),
    showClear: true
  };
  optTo: IDatePickerOptions = {
    showClear: true
  };

  constructor() {}

  ngOnInit(): void {
    if (this.initialValues) {
      this.fromDate = moment(this.initialValues.from);
      this.toDate = moment(this.initialValues.to);
    } else {
      this.fromDate = moment();
    }
  }

  fromDateChange() {
    this.optTo = {
      ...this.optTo,
      minDate: this.fromDate
    };
    this.handleChange.emit(this.buildDateObject());
  }

  toDateChange() {
    this.optFrom = {
      ...this.optFrom,
      maxDate: this.toDate
    };
    this.handleChange.emit(this.buildDateObject());
  }

  buildDateObject(): IDatepickerData {
    return {
      from: this.fromDate ? this.fromDate.format() : null,
      to: this.toDate ? this.toDate.format() : null
    };
  }

  resetDate() {}
}
