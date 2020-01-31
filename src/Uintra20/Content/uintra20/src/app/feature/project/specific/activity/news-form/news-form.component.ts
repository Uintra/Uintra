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
import { INewsCreateModel } from "../create-activity.interface";
import { IDatepickerData } from "../../../reusable/inputs/datepicker-from-to/datepiker-from-to.interface";
import { ITagData } from "../../../reusable/inputs/tag-multiselect/tag-multiselect.interface";
import { IOwner } from "src/app/feature/shared/interfaces/Owner";

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

  isShowValidation: boolean;
  newsData: INewsCreateModel;
  files: Array<any> = [];
  selectedTags: ITagData[] = [];
  isAccepted: boolean;
  owners: ISelectItem[];
  defaultOwner: ISelectItem;

  constructor() {}

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
  setLocationValue(value: string): void {
    this.newsData.activityLocationEditModel.address = value;
  }

  // Main submit function
  onSubmit() {
    this.isShowValidation = true;

    if (this.validate()) {
      this.newsDataBuilder();
      this.handleSubmit.emit(this.newsData);
    } else {
      // TODO: scroll to invalid input
    }
  }

  private newsDataBuilder(): void {
    this.newsData.newMedia = this.getMediaIdsForResponse();
    this.newsData.tagIdsData = this.getTagsForResponse();
  }

  private validate(): boolean {
    const pinValid = this.newsData.isPinned ? this.isAccepted : true;
    return this.newsData.title && this.newsData.description && pinValid;
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
