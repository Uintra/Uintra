import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import { ITagData } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IUserAvatar } from 'src/app/feature/project/reusable/ui-elements/user-avatar/user-avatar-interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { ActivityService } from 'src/app/feature/project/specific/activity/activity.service';
import { ModalService } from 'src/app/services/general/modal.service';
import { MAX_LENGTH } from 'src/app/constants/activity/create/activity-create-const';

@Component({
  selector: 'app-social-create',
  templateUrl: './social-create.component.html',
  styleUrls: ['./social-create.component.less']
})
export class SocialCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  @ViewChild('dropdownRef', { static: false }) dropdownRef: DropzoneComponent;
  availableTags: Array<ITagData> = [];
  isPopupShowing = false;
  files: Array<any> = [];
  tags: Array<ITagData> = [];
  description = '';
  inProgress = false;
  userAvatar: IUserAvatar;
  panelData: any; // TODO change any type

  get isSubmitDisabled() {
    if (ParseHelper.stripHtml(this.description).length > MAX_LENGTH || this.inProgress) {
      return true;
    }
    return !this.description && this.files.length === 0;
  }

  constructor(
    private socialContentService: ActivityService, 
    private modalService: ModalService) { }

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.availableTags = Object.values(
      JSON.parse(JSON.stringify(this.data.tags.get().userTagCollection))
    );
    this.userAvatar = {
      name: this.panelData.creator.displayedName,
      photo: this.panelData.creator.photo
    };
  }

  onShowPopUp() {
    this.showPopUp();
  }
  onHidePopUp() {
    if (this.description || this.tags.length || this.files.length) {
      if (confirm('Are you sure?')) {
        this.resetForm();
        this.hidePopUp();
      }
    } else {
      this.hidePopUp();
    }
  }

  hidePopUp() {
    this.modalService.removeClassFromRoot('disable-scroll');
    this.isPopupShowing = false;
  }

  showPopUp() {
    this.modalService.addClassToRoot('disable-scroll');
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
    return this.files.map(file => file[1]).join(';');
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
    this.inProgress = true;
    this.socialContentService
      .submitSocialContent({
        description: this.description,
        ownerId: this.panelData.creator.id,
        newMedia: this.getMediaIdsForResponse(),
        tagIdsData: this.getTagsForResponse()
      })
      .then(response => {
        this.hidePopUp();
        this.socialContentService.refreshFeed();
      })
      .catch(err => {
        this.hidePopUp();
      })
      .finally(() => {
        this.inProgress = false;
        this.resetForm();
      });
  }
}
