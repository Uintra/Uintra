import { Component, OnInit, ViewChild, HostListener, ElementRef } from '@angular/core';
import { ISocialCreate } from 'src/app/shared/interfaces/components/social/create/social-create.interface';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IUserAvatar } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar-interface';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { ActivityService } from '../../../../activity.service';
import { TranslateService } from '@ngx-translate/core';
import { RichTextEditorService } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.service';
import { Subscription } from 'rxjs';
import { ISocialCreateModel } from '../../../../activity.interfaces';
import { finalize } from 'rxjs/operators';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { NgxFocusTrapModule } from 'ngx-focus-trap';
@Component({
  selector: 'app-social-pop-up',
  templateUrl: './social-pop-up.component.html',
  styleUrls: ['./social-pop-up.component.less']
})
export class SocialPopUpComponent implements OnInit
{
  public data: ISocialCreate;

  @ViewChild('dropdownRef', { static: false }) public dropdownRef: DropzoneComponent;
  @ViewChild('ngxFocus', { static: false }) public modalWrap: NgxFocusTrapModule;

  @HostListener('window:beforeunload') public doSomething() {
    return !this.hasDataChangedService.hasDataChanged;
  }

  private $socialCreateSubscription: Subscription;
  public deviceWidth: number;
  public availableTags: Array<ITagData> = [];
  public isPopupShowing = false;
  public tags: Array<ITagData> = [];
  public description = '';
  public inProgress = false;
  public userAvatar: IUserAvatar;
  public files: Array<any> = [];
  public linkPreviewId: number;

  constructor(
    private socialContentService: ActivityService,
    private modalService: ModalService,
    private hasDataChangedService: HasDataChangedService,
    private translate: TranslateService,
    private RTEService: RichTextEditorService,
  ) { }

  public get isSubmitDisabled() {
    if (ParseHelper.stripHtml(this.description).length > MAX_LENGTH || this.inProgress) {
      return true;
    }
    return !this.description && this.files.length === 0;
  }

  ngOnInit() {
    this.availableTags = this.data.data.tags;
    this.userAvatar = {
      name: this.data.data.creator.displayedName,
      photo: this.data.data.creator.photo
    };
    this.deviceWidth = window.innerWidth;
  }

  public onSubmit(): void {
    this.inProgress = true;
    const requestModel: ISocialCreateModel = {
      description: this.description,
      ownerId: this.data.data.creator.id,
      newMedia: this.getMediaIdsForResponse(),
      tagIdsData: this.getTagsForResponse(),
      linkPreviewId: this.linkPreviewId
    };
    if (this.data.data.groupId) {
      requestModel.groupId = this.data.data.groupId;
    }
    this.$socialCreateSubscription = this.socialContentService
      .submitSocialContent(requestModel)
      .pipe(
        finalize(
          () => {
            this.inProgress = false;
            this.resetForm();
          })
      )
      .subscribe(
        next => {
          this.hidePopUp();
          this.socialContentService.refreshFeed();
        }, error => {
          this.hidePopUp();
        });
  }

  public addAttachment(): void {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
  }

  public onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
    this.hasDataChangedService.onDataChanged();
  }

  public onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => {
      const fileElement = file[0];
      return fileElement !== removedFile;
    });
  }

  public onTagsChange(e): void {
    this.tags = e;
  }

  public onDescriptionChange(e): void {
    this.description = e;
    if (e) {
      this.hasDataChangedService.onDataChanged();
    }
  }

  public getMediaIdsForResponse() {
    return this.files.map(file => file[1]).join(',');
  }

  public getTagsForResponse() {
    return this.tags.map(tag => tag.id);
  }

  public addLinkPreview(linkPreviewId: number) {
    this.linkPreviewId = linkPreviewId;
  }
  
  public onHidePopUp(): void {
    if (this.description || this.files.length) {
      if (confirm(this.translate.instant('common.AreYouSure'))) {
        this.resetForm();
        this.hidePopUp();

        //this.modalWrap.deactivateFocusTrap();
      }
    } else {
      this.resetForm();
      this.hidePopUp();

    }
  }

  public hidePopUp(): void {
    this.modalService.removeClassFromRoot('disable-scroll');
    this.modalService.removeComponentFromBody();
    this.hasDataChangedService.reset();
    this.RTEService.linkPreviewSource.next(null);
  }
  
  public resetForm(): void {
    this.files = [];
    this.tags = [];
    this.description = "";
    this.linkPreviewId = null;
  }
  
  public ngOnDestroy(): void {
    if (this.$socialCreateSubscription) { this.$socialCreateSubscription.unsubscribe(); }
  }
}
