import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Input,
  ViewEncapsulation
} from "@angular/core";
import * as moment from "moment";
import { IDatePickerOptions } from "src/app/feature/shared/interfaces/DatePickerOptions";
import { IDatepickerData } from "./datepiker-from-to.interface";

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
    // Set if it is create news
    // minDate: moment(),
    useCurrent: false,
    showClose: true
  };
  optTo: IDatePickerOptions = {
    showClear: true,
    useCurrent: false,
    showClose: true
  };

  minDate: any;

  constructor() {}

  ngOnInit(): void {
    this.fromDate =
      this.initialValues && this.initialValues.from
        ? moment(this.initialValues.from)
        : moment();

    this.toDate =
      this.initialValues && this.initialValues.to
        ? moment(this.initialValues.to)
        : null;

    this.minDate =
      this.initialValues && this.initialValues.from
        ? moment(this.initialValues.from)
        : moment();

    this.setOptionsInitialValues();
  }

  setOptionsInitialValues() {
    this.optFrom = {
      ...this.optFrom,
      minDate: this.minDate.subtract(1, "minutes")
    };
    this.optTo = {
      ...this.optTo,
      minDate: this.minDate.subtract(1, "minutes")
    };
  }

  fromDateChange() {
    this.optTo =
      this.toDate && !this.fromDate
        ? {
            ...this.optTo,
            minDate: false
          }
        : {
            ...this.optTo,
            minDate: this.fromDate.subtract(1, "minutes")
          };

    this.handleChange.emit(this.buildDateObject());
  }

  toDateChange() {
    this.optFrom = this.toDate
      ? {
          ...this.optFrom,
          maxDate: this.toDate.add(1, "minutes")
        }
      : {
          ...this.optFrom,
          maxDate: false
        };

    this.handleChange.emit(this.buildDateObject());
  }

  buildDateObject(): IDatepickerData {
    return {
      from: this.fromDate ? this.fromDate.format() : null,
      to: this.toDate ? this.toDate.format() : null
    };
  }
}
