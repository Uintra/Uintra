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
import { INewsCreateModel, IOwner } from "../activity.interfaces";
import { ILocationResult } from "../../../reusable/ui-elements/location-picker/location-picker.interface";

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
  initialDates: {
    from: string;
    to: string;
  };
  initialLocation: string;

  constructor() {}

  ngOnInit() {
    this.owners = this.getOwners();

    this.defaultOwner = this.creator
      ? this.owners.find(x => x.id === this.creator.id)
      : null;
    this.setInitialData();
  }

  private setInitialData(): void {
    this.newsData = {
      ownerId: this.data.ownerId,
      title: this.data.title || "",
      description: this.data.description || "",
      publishDate: undefined,
      location: {
        address: (this.data.location && this.data.location.address) || null,
        shortAddress:
          (this.data.location && this.data.location.shortAddress) || null
      },
      endPinDate: this.data.endPinDate || null,
      isPinned: this.data.isPinned || false,
      media: this.data.media || null
    };

    this.selectedTags = this.data.tags || [];
    this.initialDates = {
      from: this.data.publishDate || undefined,
      to: this.data.unpublishDate || undefined
    };

    this.initialLocation =
      (this.data.location && this.data.location.address) || null;

    if (this.newsData.isPinned) {
      this.isAccepted = true;
    }
  }

  // File functions
  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }
  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => file[0] !== removedFile);
  }
  public handleImageRemove(image): void {
    this.newsData.media.medias =
      this.newsData.media.medias.filter(m => m !== image);
  }
  public handleFileRemove(file): void {
    this.newsData.media.otherFiles =
      this.newsData.media.otherFiles.filter(m => m !== file);
  }

  // Data set functions
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
    this.newsData.location.address = location.address;
    this.newsData.location.shortAddress = location.shortAddress;
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

  private validate(): boolean {
    const pinValid = this.newsData.isPinned ? this.isAccepted : true;
    return this.newsData.title && this.newsData.description && pinValid;
  }

  private newsDataBuilder(): void {
    this.newsData.newMedia = this.getMediaIdsForResponse();
    this.newsData.tagIdsData = this.getTagsForResponse();
  }

  // TODO: move to service
  private getTagsForResponse(): string[] {
    return this.selectedTags ? this.selectedTags.map(tag => tag.id) : [];
  }
  private getMediaIdsForResponse(): string {
    return this.files.map(file => file[1]).join(";");
  }
  private getOwners(): ISelectItem[] {
    const owners = this.getMembers(this.members);
    if (this.creator) {
      owners.push({
        id: this.creator.id,
        text: this.creator.displayedName
      });
    }

    return owners;
  }
  private getMembers(members = []): ISelectItem[] {
    return members.map(member => ({
      id: member.id,
      text: member.displayedName
    }));
  }
}
