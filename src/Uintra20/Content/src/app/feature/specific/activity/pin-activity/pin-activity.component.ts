import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { IDatePickerOptions } from "src/app/shared/interfaces/DatePickerOptions";
import * as moment from "moment";
import { IDatepickerData } from "../datepicker-from-to/datepiker-from-to.interface";
import { PinActivityService } from "./pin-activity.service";
import { ContentService } from 'src/app/shared/services/general/content.service';

export interface IPinedData {
  isPinChecked: boolean;
  isAccepted: boolean;
  pinDate: string;
}
@Component({
  selector: "app-pin-activity",
  templateUrl: "./pin-activity.component.html",
  styleUrls: ["./pin-activity.component.less"]
})
export class PinActivityComponent implements OnInit {
  @Input() isPinChecked: boolean;
  @Input() isAccepted: boolean;
  @Input() endPinDate: string;
  @Input() noMaxDate: any;
  @Input() isEvent: any;
  @Output() handleChange = new EventEmitter<IPinedData>();

  @Input() publishDate: string = null;
  @Input() unpublishDate: string = null;

  options: IDatePickerOptions;
  pinDate: moment.Moment = null;
  pinedDateValue: IPinedData = {
    isPinChecked: false,
    isAccepted: false,
    pinDate: ""
  };

  constructor(
    private pinActivityService: PinActivityService,
    private contentService: ContentService
  ) {
    this.pinActivityService.publishDates$.subscribe((dates: IDatepickerData) => {
      this.options = this.isEvent ? {
        ...this.options,
        minDate: dates.from ? moment(dates.from).hours(0).minutes(0).seconds(0) : false,
        maxDate: dates.to && !this.noMaxDate ? moment(dates.to).hours(23).minutes(59).seconds(59) : false,
        format: "DD/MM/YYYY HH:mm"
      } : {
        ...this.options,
        minDate: dates.from ? moment(dates.from).hours(0).minutes(0).seconds(0) : false,
        maxDate: dates.to && !this.noMaxDate ? moment(dates.to).hours(23).minutes(59).seconds(59) : false,
        format: "DD/MM/YYYY HH:mm"
      }
      if (dates.from && moment(dates.from) > moment(this.pinDate)) {
        this.pinDate = moment(dates.from);
      }
    });
  }

  public ngOnInit(): void {
    this.isEvent = this.isEvent !== undefined;
    this.noMaxDate = this.noMaxDate !== undefined;
    this.pinedDateValue = {
      isPinChecked: this.isPinChecked,
      isAccepted: this.isAccepted,
      pinDate: this.endPinDate
    };

    this.options = {
      showClose: true,
      ignoreReadonly: true
    };

    this.pinDate = this.endPinDate
      ? moment(this.endPinDate)
      : moment();
  }

  public onDateChange(): void {
    this.pinedDateValue.pinDate = this.pinDate ? this.pinDate.format() : "";
    if (this.publishDate && moment(this.pinedDateValue.pinDate) < moment(this.publishDate)) {
      this.pinDate = moment(this.publishDate);
      this.pinedDateValue.pinDate = this.pinDate.format();
    }
    if (this.unpublishDate && moment(this.pinedDateValue.pinDate) > moment(this.unpublishDate)) {
      this.pinDate = moment(this.unpublishDate);
      this.pinedDateValue.pinDate = this.pinDate.format();
    }
    this.handleChange.emit(this.pinedDateValue);
  }

  public onAcceptedChange(): void {
    this.pinedDateValue.isPinChecked = this.isPinChecked;

    this.isPinChecked
      ? this.handleChange.emit(this.pinedDateValue)
      : this.handleChange.emit(this.rollbackModel);

    setTimeout(() => this.contentService.makeReadonly('.udatepicker-input'), 0);
  }


  private get rollbackModel(): IPinedData {
    return {
      isPinChecked: false,
      isAccepted: undefined,
      pinDate: null
    };
  }
}
