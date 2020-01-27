import { Component, OnInit, Output, EventEmitter } from "@angular/core";

interface datePickerOptioms {
  minDate?: string;
  maxDate?: string;
}

@Component({
  selector: "app-datepicker-from-to",
  templateUrl: "./datepicker-from-to.component.html",
  styleUrls: ["./datepicker-from-to.component.less"]
})
export class DatepickerFromToComponent implements OnInit {
  @Output() setValue = new EventEmitter();

  fromDate = null;
  toDate = null;
  optFrom: datePickerOptioms = {};
  optTo: datePickerOptioms = {};

  constructor() {}

  ngOnInit(): void {
    this.fromDate = new Date();
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
}
