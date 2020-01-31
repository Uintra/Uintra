import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewEncapsulation
} from "@angular/core";
import { IPinedData } from "../pin-activity/pin-activity.component";
import { ISelectItem } from "../../../reusable/inputs/select/select.component";
import { IDatepickerData } from "../../../reusable/inputs/datepicker-from-to/datepiker-from-to.interface";
import { ITagData } from "../../../reusable/inputs/tag-multiselect/tag-multiselect.interface";
import { IOwner } from "src/app/feature/shared/interfaces/Owner";
import { INewsForm } from "./news-form.interface";
import { INewsCreateModel } from '../activity.interfaces';
import { ILocationResult } from '../../../reusable/ui-elements/location-picker/location-picker.interface';

@Component({
  selector: "app-news-form",
  templateUrl: "./news-form.component.html",
  styleUrls: ["./news-form.component.less"],
  encapsulation: ViewEncapsulation.None
})
export class NewsFormComponent implements OnInit {
  @Input() data: INewsCreateModel;

  @Input() members: Array<any>;
  @Input() creator: IOwner;
  @Input() tags: ITagData[];

  @Output() handleSubmit = new EventEmitter();
  @Output() handleCancel = new EventEmitter();

  newsData: INewsCreateModel;
  files: Array<any> = [];
  selectedTags: ITagData[] = [];
  isAccepted: boolean;
  owners: ISelectItem[];
  defaultOwner: ISelectItem;

  constructor() { }

  ngOnInit() {
    this.owners = this.getOwners();
    this.defaultOwner = this.owners.find(x => x.id === this.creator.id);
    this.setInitialData();
  }

  private setInitialData(): void {
    this.newsData = {
      ownerId: this.data.ownerId,
      title: "",
      description: "",
      publishDate: null,
      activityLocationEditModel: {}
    };
  }

  // File functions
  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }
  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => file[0] !== removedFile);
  }

  // Set date functions
  setDatePickerValue(value: IDatepickerData = {}) {
    this.newsData.publishDate = value.from;
    this.newsData.unpublishDate = value.to;
  }
  setPinValue(value: IPinedData) {
    this.newsData.endPinDate = value.pinDate;
    this.newsData.isPinned = value.isPinCheked;
    this.isAccepted = value.isAccepted;
  }
  setLocationValue(location: ILocationResult): void {
    this.newsData.activityLocationEditModel.address = location.address;
    this.newsData.activityLocationEditModel.shortAddress = location.shortAddress;
    
  }

  // Main submit function
  onSubmit() {
    this.newsDataBuilder();
    this.handleSubmit.emit(this.newsData);
  }
  private newsDataBuilder(): void {
    this.newsData.newMedia = this.getMediaIdsForResponse();
    this.newsData.tagIdsData = this.getTagsForResponse();
  }

  // TODO: move to service
  private getTagsForResponse(): string[] {
    return this.selectedTags.map(tag => tag.id);
  }
  private getMediaIdsForResponse(): string {
    return this.files.map(file => file[1]).join(";");
  }
  private getOwners(): ISelectItem[] {
    return [
      ...this.getMembers(this.members),
      {
        id: this.creator.id,
        text: this.creator.displayedName
      }
    ];
  }
  private getMembers(members = []): ISelectItem[] {
    return members.map(member => ({
      id: member.id,
      text: member.displayedName
    }));
  }
}
