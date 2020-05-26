import { Component, OnInit, Output, EventEmitter, Input, ViewEncapsulation, OnDestroy } from '@angular/core';
import * as moment from 'moment';
import { IDatePickerOptions } from 'src/app/shared/interfaces/DatePickerOptions';
import { IDatepickerData } from './datepiker-from-to.interface';
import { PinActivityService } from '../pin-activity/pin-activity.service';
import { Subscription } from 'rxjs';
import { DatepickerService } from './datepicker.service';

@Component({
  selector: 'app-datepicker-from-to',
  templateUrl: './datepicker-from-to.component.html',
  styleUrls: ['./datepicker-from-to.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class DatepickerFromToComponent implements OnInit, OnDestroy {
  @Input()
  public initialValues: { from: string; to: string } = null;
  @Input()
  public fromLabel: string;
  @Input()
  public toLabel: string;
  @Input()
  public isEvent: boolean;
  @Input()
  public isEventEdit: boolean;
  @Input()
  public eventPublishDate: string;
  @Input()
  public isNews: boolean;
  @Output()
  public handleChange = new EventEmitter();

  public fromDate = null;
  public toDate = null;
  public optFrom: IDatePickerOptions = {
    // Set if it is create news
    // minDate: moment(),
    format: 'DD/MM/YYYY HH:mm',
    useCurrent: false,
    showClose: true,
    ignoreReadonly: true,
    debug: true
  };
  public optTo: IDatePickerOptions = {
    format: 'DD/MM/YYYY HH:mm',
    showClear: false,
    useCurrent: false,
    showClose: true,
    ignoreReadonly: true,
    debug: true
  };

  public minDate: any;
  public eventSubscription: Subscription;
  public isPublishDateInvalid: boolean;

  constructor(
    private pinActivityService: PinActivityService,
    private datepickerService: DatepickerService) { }

  public ngOnInit(): void {
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
          const minDate = this.isEventEdit
            ? moment(dates.from).clone() < moment() ? moment() : moment(dates.from).clone()
            : moment(dates.from).clone();
          this.optFrom = {
            ...this.optFrom,
            minDate: minDate.hours(0).minutes(0).seconds(0),
          };
          if (moment(dates.from) > moment(this.fromDate)) {
            this.fromModelChanged(moment(dates.from));
            this.handleChange.emit(this.buildDateObject());
          }
        }
        if (dates.to) {
          this.toModelChanged(moment(dates.to))
        }
      });
    }
  }

  public setOptionsInitialValues(): void {
    this.optFrom = {
      ...this.optFrom,
      minDate: this.minDate.clone().hours(0).minutes(0).seconds(0)
    };
    this.optTo = {
      ...this.optTo,
      minDate: this.minDate.clone().hours(0).minutes(0).seconds(0)
    };
  }

  public fromDateChange(): void {
    this.toDate && !this.fromDate
      ? {
        ...this.optTo,
        minDate: false
      }
      : {
        ...this.optTo,
        minDate: this.fromDate.clone().hours(0).minutes(0).seconds(0)
      };

    this.handleChange.emit(this.buildDateObject());
  }

  fromModelChanged(value) {
    this.isPublishDateInvalid = this.datepickerService.handlePublishDateState(value);

    if (value) {
      if (this.eventPublishDate) {
        this.fromDate = moment(value.format()) < moment(this.eventPublishDate) ? moment(this.eventPublishDate) : moment(value.format());
      } else {
        this.fromDate = moment(value.format());
      }

      if (this.toDate < value) {
        if (this.isEvent) {
          this.toDate = value.add(8, 'hours');
        }
        if (this.isNews) {
          this.toDate = null;
        }
      }

      this.optTo = {
        ...this.optTo,
        minDate: this.fromDate.clone().hours(0).minutes(0).seconds(0)
      };
    }
  }
  toModelChanged(value) {
    if (value) {
      this.toDate = moment(value) < moment(this.fromDate) ? moment(this.fromDate) : moment(value);
    }
  }

  toDateChange() {
    this.optFrom = {
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

  public ngOnDestroy(): void {
    if (this.isEvent || this.eventSubscription != null) {
      this.eventSubscription.unsubscribe();
    }
  }
}
