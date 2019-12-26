import { Component, ViewEncapsulation, ViewChild } from "@angular/core";
import { IActivityCreatePanel } from "./activity-create-panel.interface";
import { ITagData } from "src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.interface";
import { DropzoneComponent } from "ngx-dropzone-wrapper";

import { MAX_LENGTH } from "./_constants.js";
import { CreateSocialService } from 'src/app/services/createActivity/create-social.service';

@Component({
  selector: "activity-create-panel",
  templateUrl: "./activity-create-panel.html",
  styleUrls: ["./activity-create-panel.less"],
  encapsulation: ViewEncapsulation.None
})
export class ActivityCreatePanel {
  @ViewChild("dropdownRef", { static: false }) dropdownRef: DropzoneComponent;

  data: IActivityCreatePanel;
  availableTags: Array<ITagData> = [];
  isPopupShowing: boolean = false;

  files: Array<any> = [];
  tags: Array<ITagData> = [];
  description: string = "";

  get isSubmitDisabled() {
    if (this.description && this.description.length > MAX_LENGTH) {
      return true;
    }
    return !this.description && this.files.length === 0;
  }

  constructor(private socialContentService: CreateSocialService) {}

  ngOnInit() {
    this.availableTags = Object.values(
      JSON.parse(JSON.stringify(this.data.tags.get().userTagCollection))
    );
  }

  onShowPopUp() {
    this.showPopUp();
  }
  onHidePopUp() {
    if (this.description || this.tags.length || this.files.length) {
      if (confirm("Are you sure?")) {
        this.resetForm();
        this.hidePopUp();
      }
    } else {
      this.hidePopUp();
    }
  }

  hidePopUp() {
    this.isPopupShowing = false;
  }
  showPopUp() {
    this.isPopupShowing = true;
  }

  addAttachment() {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
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

  getMediaIdsForResponse() {
    return this.files.map(file => file[1]).join(";");
  }
  getTagsForResponse() {
    return this.tags.map(tag => tag.id);
  }

  resetForm(): void {
    this.files = [];
    this.tags = [];
    this.description = '';
  }

  onSubmit() {
    this.socialContentService
      .submitSocialContent({
        description: this.description,
        OwnerId: "cb6969e1-ac68-4cae-88a3-8b1cbc453ef7",
        NewMedia: this.getMediaIdsForResponse(),
        TagIdsData: this.getTagsForResponse()
      })
      .then(response => {
        this.hidePopUp();
        this.socialContentService.refreshFeed();
      })
      .catch(err => {
        this.hidePopUp();
      })
      .finally(() => {
        this.resetForm();
      });
  }
}
