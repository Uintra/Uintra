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
import { INewsForm } from "./news-form.interface";
import { INewsCreateModel } from "../create-activity.interface";

@Component({
  selector: "app-news-form",
  templateUrl: "./news-form.component.html",
  styleUrls: ["./news-form.component.less"],
  encapsulation: ViewEncapsulation.None
})
export class NewsFormComponent implements OnInit {
  @Input() data: INewsCreateModel;

  @Input() members: Array<any>;
  @Input() creator: any;
  @Input() tags: any[];

  @Output() handleSubmit = new EventEmitter();
  @Output() handleCancel = new EventEmitter();

  newsData: INewsCreateModel;

  files: Array<any> = [];
  selectedTags: any = [];
  isPinCheked: boolean;
  owners: ISelectItem[];
  defaultOwner: ISelectItem;
  pinDate: IPinedData;

  newsForm: INewsForm;
  location: string;

  constructor() {}

  ngOnInit() {
    this.owners = this.getOwners();
    this.defaultOwner = this.owners.find(x => x.id === this.creator.id);

    this.newsData = {
      ownerId: this.data.ownerId,
      title: "",
      description: "",
      publishDate: null
    };
  }

  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }

  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => {
      const fileElement = file[0];
      return fileElement !== removedFile;
    });
  }

  setDatePickerValue(value: any = {}) {
    this.newsData.publishDate = value.from;
    this.newsData.unpublishDate = value.to;
  }

  onSubmit() {
    this.newsData.newMedia = this.getMediaIdsForResponse();
    this.newsData.tagIdsData = this.getTagsForResponse();

    this.handleSubmit.emit(this.newsData);
  }

  editPinDate(pinDate: IPinedData) {
    this.pinDate = pinDate && pinDate.isPinCheked ? pinDate : null;
  }

  private getTagsForResponse() {
    return this.tags.map(tag => tag.id);
  }

  private getMediaIdsForResponse() {
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

  public handleAddressChanged(address): void {
    this.location = address;
  }
}
