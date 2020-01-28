import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";
import * as moment from "moment";

interface datePickerOptioms {
  minDate?: string;
  maxDate?: string;
  showClear?: boolean;
}

@Component({
  selector: "app-datepicker-from-to",
  templateUrl: "./datepicker-from-to.component.html",
  styleUrls: ["./datepicker-from-to.component.less"]
})
export class DatepickerFromToComponent implements OnInit {
  @Input() initialValues: { from: string; to: string } = null;
  @Output() setValue = new EventEmitter();

  fromDate = null;
  toDate = null;
  optFrom: datePickerOptioms = {
    showClear: true
  };
  optTo: datePickerOptioms = {
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
    this.setValue.emit(this.buildDateObject());
  }

  toDateChange() {
    this.optFrom = {
      ...this.optFrom,
      maxDate: this.toDate
    };
    this.setValue.emit(this.buildDateObject());
  }

  buildDateObject() {
    return {
      from: this.fromDate ? this.fromDate.format() : null,
      to: this.toDate ? this.toDate.format() : null
    };
  }

  resetDate() {}
}
