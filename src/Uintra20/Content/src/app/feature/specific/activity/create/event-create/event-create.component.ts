import { Component, OnInit, Input } from '@angular/core';
import { ISelectItem } from 'src/app/feature/reusable/inputs/select/select.component';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IEventsInitialDates, IEventCreateModel } from './event-create.interface';
import { PinActivityService } from '../../pin-activity/pin-activity.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { TranslateService } from '@ngx-translate/core';
import { EventFormService } from './event-create.service';
import { RTEStripHTMLService } from '../../rich-text-editor/helpers/rte-strip-html.service';

@Component({
  selector: 'app-event-create',
  templateUrl: './event-create.component.html',
  styleUrls: ['./event-create.component.less']
})
export class EventCreateComponent implements OnInit {
  @Input() data: any;
  @Input('edit') edit: any;

  eventsData: IEventCreateModel;
  selectedTags: ITagData[] = [];
  isAccepted: boolean;
  owners: ISelectItem[];
  defaultOwner: ISelectItem;
  initialDates: IEventsInitialDates;

  constructor(
    private eventFormService: EventFormService,
    private pinActivityService: PinActivityService,
    private hasDataChangedService: HasDataChangedService,
    private stripHTML: RTEStripHTMLService,
    public translate: TranslateService,
    ) { }

  ngOnInit() {
    this.edit = this.edit !== undefined;
    this.eventsData = this.eventFormService.getEventDataInitialValue(this.data);
    this.setInitialData();
  }

  private setInitialData(): void {
    this.owners = this.eventFormService.getOwners(this.data.members, this.data.creator);

    this.defaultOwner = this.data.creator
      ? this.data.members.find(x => x.id === this.data.creator.id)
      : null;

    // this.selectedTags = this.data.tags || [];

    this.initialDates = {
      publishDate: this.data.publishDate || undefined,
      startDate: this.data.startDate || undefined,
      endDate: this.data.endDate || undefined
    };

    // this.initialLocation =
    //   (this.data.location && this.data.location.address) || null;

    // if (this.newsData.isPinned) {
    //   this.isAccepted = true;
    // }
  }

  changeOwner(owner: ISelectItem | string) {
    if (typeof owner === "string") {
      this.eventsData.ownerId = owner;
    } else {
      this.eventsData.ownerId = owner.id;
    }
    if (this.defaultOwner.id !== this.eventsData.ownerId) {
      this.hasDataChangedService.onDataChanged();
    }
  }

  onTitleChange(e) {
    if (this.eventsData.title != e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.eventsData.title = e;
  }

  onDescriptionChange(e) {
    if (this.eventsData.description != e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.eventsData.description = e;
  }

  isDescriptionEmpty() {
    return this.stripHTML.isEmpty(this.eventsData.description);
  }
}
