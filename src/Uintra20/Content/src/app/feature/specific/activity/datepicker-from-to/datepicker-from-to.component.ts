import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Input,
  ViewEncapsulation
} from "@angular/core";
import * as moment from "moment";
import { IDatePickerOptions } from "src/app/shared/interfaces/DatePickerOptions";
import { IDatepickerData } from "./datepiker-from-to.interface";
import { PinActivityService } from '../pin-activity/pin-activity.service';

@Component({
  selector: "app-datepicker-from-to",
  templateUrl: "./datepicker-from-to.component.html",
  styleUrls: ["./datepicker-from-to.component.less"],
  encapsulation: ViewEncapsulation.None
})
export class DatepickerFromToComponent implements OnInit {
  @Input() initialValues: { from: string; to: string } = null;
  @Input() fromLabel: string;
  @Input() toLabel: string;
  @Input() isEvent: boolean;
  @Output() handleChange = new EventEmitter();

  fromDate = null;
  toDate = null;
  optFrom: IDatePickerOptions = {
    // Set if it is create news
    // minDate: moment(),
    useCurrent: false,
    showClose: true,
    ignoreReadonly: true
  };
  optTo: IDatePickerOptions = {
    showClear: false,
    useCurrent: false,
    showClose: true,
    ignoreReadonly: true
  };

  minDate: any;
  eventSubscription: any;

  constructor(private pinActivityService: PinActivityService) {}

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

    if (this.isEvent) {
      this.eventSubscription = this.pinActivityService.publishDates$.subscribe((dates: IDatepickerData) => {
        if (dates.from) {
          const minDate = moment(dates.from).clone();
          this.optFrom = {
            ...this.optFrom,
            minDate: minDate.hours(0).minutes(0).seconds(0),
          };
          if (moment(dates.from) > moment(this.fromDate)) {
            this.fromModelChanged(moment(dates.from));
            this.handleChange.emit(this.buildDateObject());
          }
        }
      });
    }
  }

  setOptionsInitialValues() {
    this.optFrom = {
      ...this.optFrom,
      minDate: this.minDate
    };
    this.optTo = {
      ...this.optTo,
      minDate: this.minDate
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
            minDate: this.fromDate
          };

    this.handleChange.emit(this.buildDateObject());
  }

  fromModelChanged(value) {
    if (value) {
      this.fromDate = moment(value.format());
      if (this.toDate < value && this.isEvent) {
        this.toDate = value.add(8, "hours");
      }
    }
  }
  toModelChanged(value) {
    if (value) {
      this.toDate = value;
    }
  }

  toDateChange() {
    this.optFrom = this.toDate && !this.isEvent
      ? {
          ...this.optFrom,
          maxDate: this.toDate
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

  ngOnDestroy() {
    if (this.isEvent) {
      this.eventSubscription.unsubscribe();
    }
  }
}
