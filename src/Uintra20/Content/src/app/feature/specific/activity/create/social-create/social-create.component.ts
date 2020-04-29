import {
  Component,
  OnInit,
  Input,
  ViewChild,
  HostListener,
  OnDestroy
} from '@angular/core';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { MqService } from 'src/app/shared/services/general/mq.service';
import { MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IUserAvatar } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar-interface';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import { ISocialCreateModel } from 'src/app/feature/specific/activity/activity.interfaces';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ISocialCreate } from 'src/app/shared/interfaces/components/social/create/social-create.interface';
import { RichTextEditorService } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.service';

@Component({
  selector: 'app-social-create',
  templateUrl: './social-create.component.html',
  styleUrls: ['./social-create.component.less']
})
export class SocialCreateComponent implements OnInit, OnDestroy {

  private $socialCreateSubscription: Subscription;
  @Input()
  public data: ISocialCreate;

  @ViewChild('dropdownRef', { static: false })
  public dropdownRef: DropzoneComponent;

  public deviceWidth: number;
  public availableTags: Array<ITagData> = [];
  public isPopupShowing = false;
  public tags: Array<ITagData> = [];
  public description = '';
  public inProgress = false;
  public userAvatar: IUserAvatar;
  public files: Array<any> = [];
  linkPreviewId: number;

  public get isSubmitDisabled() {
    if (ParseHelper.stripHtml(this.description).length > MAX_LENGTH || this.inProgress) {
      return true;
    }
    return !this.description && this.files.length === 0;
  }

  constructor(
    private socialContentService: ActivityService,
    private modalService: ModalService,
    private hasDataChangedService: HasDataChangedService,
    private mq: MqService,
    private translate: TranslateService,
    private RTEService: RichTextEditorService,
  ) { }

  public ngOnInit(): void {
    this.availableTags = this.data.data.tags;
    this.userAvatar = {
      name: this.data.data.creator.displayedName,
      photo: this.data.data.creator.photo
    };
    this.deviceWidth = window.innerWidth;
    this.getPlaceholder();
  }

  public ngOnDestroy(): void {
    if (this.$socialCreateSubscription) { this.$socialCreateSubscription.unsubscribe(); }
  }

  @HostListener('window:beforeunload')
  public doSomething() {
    return !this.hasDataChangedService.hasDataChanged;
  }

  @HostListener('window:resize', ['$event'])
  public getScreenSize(event?) {
    this.deviceWidth = window.innerWidth;
  }

  public onShowPopUp(): void {
    if (this.data.canCreate) {
      this.showPopUp();
    }
  }
  public onHidePopUp(): void {
    if (this.description || this.files.length) {
      if (confirm(this.translate.instant('common.AreYouSure'))) {
        this.resetForm();
        this.hidePopUp();
      }
    } else {
      this.resetForm();
      this.hidePopUp();
    }
  }

  public hidePopUp(): void {
    this.modalService.removeClassFromRoot('disable-scroll');
    this.isPopupShowing = false;
    this.hasDataChangedService.reset();
    this.RTEService.linkPreviewSource.next(null);
  }

  public showPopUp(): void {
    this.modalService.addClassToRoot('disable-scroll');
    this.isPopupShowing = true;
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

  public resetForm(): void {
    this.files = [];
    this.tags = [];
    this.description = "";
    this.linkPreviewId = null;
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

  public canCreatePosts(): boolean {
    if (this.data) {
      return (
        this.data.canCreate ||
        this.data.createEventsLink ||
        this.data.createNewsLink
      );
    }
  }

  public getPlaceholder(): string {
    return this.mq.isTablet(this.deviceWidth)
      ? 'socialsCreate.FormPlaceholder.lbl'
      : 'socialsCreate.MobileBtn.lbl';
  }

  addLinkPreview(linkPreviewId: number) {
    this.linkPreviewId = linkPreviewId;
  }
}
