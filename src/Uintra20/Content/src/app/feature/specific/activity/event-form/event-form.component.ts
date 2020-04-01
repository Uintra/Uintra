import { Component, OnInit, Input, Output, EventEmitter, HostListener, OnChanges, DoCheck, AfterViewInit } from '@angular/core';
import { ISelectItem } from 'src/app/feature/reusable/inputs/select/select.component';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { PinActivityService } from '../pin-activity/pin-activity.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { TranslateService } from '@ngx-translate/core';
import { EventFormService } from './event-form.service';
import { RTEStripHTMLService } from '../rich-text-editor/helpers/rte-strip-html.service';
import { IDatepickerData } from '../datepicker-from-to/datepiker-from-to.interface';
import { ILocationResult } from 'src/app/feature/reusable/ui-elements/location-picker/location-picker.interface';
import { IPinedData } from '../pin-activity/pin-activity.component';
import * as moment from "moment";
import { IEventCreateModel, IEventsInitialDates, IPublishDatepickerOptions } from './event-form.interface';
import { ContentService } from 'src/app/shared/services/general/content.service';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html',
  styleUrls: ['./event-form.component.less']
})
export class EventFormComponent implements OnInit, AfterViewInit {

  @Input() data: any;
  @Input('edit') edit: any;
  @Input() inProgress: boolean;
  @Output() submit = new EventEmitter();
  @Output() cancel = new EventEmitter();
  @Output() hide = new EventEmitter();

  public eventsData: IEventCreateModel;
  public selectedTags: ITagData[] = [];
  public isAccepted: boolean;
  public owners: ISelectItem[];
  public defaultOwner: ISelectItem;
  public initialDates: IEventsInitialDates;
  public initialLocation: string;
  public locationTitle = "";
  public publishDatepickerOptions: IPublishDatepickerOptions;
  public files: Array<any> = [];
  public isShowValidation: boolean;

  constructor(
    private eventFormService: EventFormService,
    private pinActivityService: PinActivityService,
    private hasDataChangedService: HasDataChangedService,
    private stripHTML: RTEStripHTMLService,
    public translate: TranslateService,
    private contentService: ContentService
  ) { }

  public ngOnInit(): void {
    this.edit = this.edit !== undefined;
    this.eventsData = this.eventFormService.getEventDataInitialValue(this.data);
    this.setInitialData();

    this.publishDatepickerOptions = {
      showClose: true,
      minDate: this.eventsData.publishDate ? this.eventsData.publishDate : moment(),
      ignoreReadonly: true
    };

    if (this.eventsData.isPinned) {
      this.isAccepted = true;
    }
  }

  public ngAfterViewInit(): void {
    this.contentService.makeReadonly('.udatepicker-input');
  }

  @HostListener('window:beforeunload') checkIfDataChanged() {
    return !this.hasDataChangedService.hasDataChanged;
  }

  private setInitialData(): void {
    this.owners = this.eventFormService.getMembers(this.data.members);

    this.defaultOwner = this.data.creator
      ? this.data.members.find(x => x.id === this.data.creator.id)
      : null;

    this.selectedTags = this.data.selectedTags || [];

    this.initialDates = {
      publishDate: this.data.publishDate || undefined,
      from: this.data.startDate || undefined,
      to: this.data.endDate || undefined
    };

    this.initialLocation = (this.data.location && this.data.location.address) || null;
  }

  public changeOwner(owner: ISelectItem | string): void {
    if (typeof owner === "string") {
      this.eventsData.ownerId = owner;
    } else {
      this.eventsData.ownerId = owner.id;
    }
    if (this.defaultOwner.id !== this.eventsData.ownerId) {
      this.hasDataChangedService.onDataChanged();
    }
  }

  public onTitleChange(e: any): void {
    if (this.eventsData.title !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.eventsData.title = e;
  }

  public onDescriptionChange(e: any): void {
    if (this.eventsData.description !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.eventsData.description = e;
  }

  public setDatePickerValue(value: IDatepickerData = {}): void {
    const test = moment(this.initialDates.from).format();
    if ((moment(this.initialDates.from).format() !== value.from && moment(this.initialDates.from).subtract(5, "seconds").format() !== value.from)
      || (moment(this.initialDates.to).format() !== value.to && (moment(this.initialDates.to).add(5, "seconds").format() !== value.to))) {
      this.hasDataChangedService.onDataChanged();
    }


    this.eventsData.startDate = value.from;
    this.eventsData.endDate = value.to;

    this.publishDatepickerOptions = {
      showClose: true,
      minDate: moment(value.from),
      ignoreReadonly: true
    };

  }

  public setPinValue(value: IPinedData): void {
    if (this.data.endPinDate !== this.eventsData.endPinDate) {
      this.hasDataChangedService.onDataChanged();
    }
    this.eventsData.endPinDate = value.pinDate;
    this.eventsData.isPinned = value.isPinCheked;
    this.isAccepted = value.isAccepted;
  }

  public setLocationValue(location: ILocationResult): void {
    this.eventsData.location.address = location.address;
    this.eventsData.location.shortAddress = location.shortAddress;
    this.hasDataChangedService.onDataChanged();
  }

  public onPublishDateChange(event: any): void {

    if (event) {

      this.eventsData.publishDate = event;
      this.pinActivityService.setPublishDates({ from: this.eventsData.publishDate });

      if (event && event._i && event._i !== this.initialDates.publishDate) {

        this.hasDataChangedService.onDataChanged();
      }
    }

  }

  public onLocationTitleChange(val: any): void {
    this.eventsData.locationTitle = val;
    this.hasDataChangedService.onDataChanged();
  }

  public onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
    this.hasDataChangedService.onDataChanged();
  }

  public onFileRemoved(removedFile: object): void {
    this.files = this.files.filter(file => file[0] !== removedFile);
    this.hasDataChangedService.onDataChanged();
  }

  public handleImageRemove(image: any): void {
    this.eventsData.media.medias = this.eventsData.media.medias.filter(m => m !== image);
    this.hasDataChangedService.onDataChanged();
  }

  public handleFileRemove(file: any): void {
    this.eventsData.media.otherFiles = this.eventsData.media.otherFiles.filter(m => m !== file);
    this.hasDataChangedService.onDataChanged();
  }

  public onSubscriptionCheckboxChange(val: any): void {
    this.eventsData.canSubscribe = val;
    this.hasDataChangedService.onDataChanged();
  }

  public onSubscriptionNoteChange(val: any): void {
    this.eventsData.subscribeNotes = val;
    this.hasDataChangedService.onDataChanged();
  }

  public onSubmit(): void {
    this.isShowValidation = true;

    if (this.validate()) {
      this.eventDataBuilder();
      this.submit.emit(this.eventsData);
    }
  }

  public onCancel(): void {
    this.cancel.emit();
  }

  public onHide(): void {
    this.hide.emit();
  }

  private eventDataBuilder(): void {
    this.eventsData.newMedia = this.eventFormService.getMediaIdsForResponse(this.files);
    this.eventsData.tagIdsData = this.eventFormService.getTagsForResponse(this.selectedTags);
  }

  private validate() {
    const pinValid = this.eventsData.isPinned ? this.isAccepted : true;
    const title = this.eventsData.title.trim();

    return (
      title
      && !this.validateDescription()
      && pinValid
      && this.eventsData.publishDate
      && this.eventsData.startDate
      && this.eventsData.endDate
    );
  }

  public validateDescription(): boolean {
    return this.stripHTML.isEmpty(this.eventsData.description);
  }

  public validateAccept(): boolean {
    if (!this.eventsData.isPinned) {
      return false;
    }

    if (!this.isAccepted) {
      return true;
    }

    return false;
  }

  public validateEmptyField(src: any): boolean {
    if (!src) {
      return true;
    }

    return false;
  }
}
