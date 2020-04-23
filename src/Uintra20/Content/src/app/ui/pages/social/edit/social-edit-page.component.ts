import { Component, ViewEncapsulation, HostListener, OnDestroy } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import ParseHelper from "../../../../shared/utils/parse.helper";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { RouterResolverService } from "src/app/shared/services/general/router-resolver.service";
import { Observable, Subscription } from "rxjs";
import { HasDataChangedService } from "src/app/shared/services/general/has-data-changed.service";
import { CanDeactivateGuard } from "src/app/shared/services/general/can-deactivate.service";
import { ActivityService } from "src/app/feature/specific/activity/activity.service";
import { ISocialEdit } from 'src/app/feature/specific/activity/activity.interfaces';
import { TranslateService } from '@ngx-translate/core';
import { ISocialEditPage } from 'src/app/shared/interfaces/pages/social/edit/social-edit-page.interface';

@Component({
  selector: 'social-edit',
  templateUrl: './social-edit-page.component.html',
  styleUrls: ['./social-edit-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialEditPageComponent implements OnDestroy {

  private $deleteSubscription: Subscription;
  private $updateSubscription: Subscription;

  public data: ISocialEditPage;
  public files: Array<any> = new Array<any>();
  public inProgress = false;
  public socialEdit: ISocialEdit;
  public uploadedData: Array<any> = new Array<any>();
  public socialEditForm: FormGroup;

  constructor(
    private activatedRoute: ActivatedRoute,
    private socialService: ActivityService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private translate: TranslateService
  ) {
    this.activatedRoute.data.subscribe((data: ISocialEditPage) => {
      this.data = data;
      debugger;
      this.onParse();
      this.initSocialEditForm();
    });
  }

  public ngOnDestroy(): void {
    this.$deleteSubscription.unsubscribe();
    this.$updateSubscription.unsubscribe();
  }

  @HostListener('window:beforeunload') public checkIfDataChanged() {
    return !this.hasDataChangedService.hasDataChanged;
  }

  private onParse = (): void => {
    this.socialEdit = {
      ownerId: this.data.ownerId,
      description: this.data.description,
      tags: this.data.tags,
      availableTags: this.data.availableTags,
      lightboxPreviewModel: {
        medias: this.data.lightboxPreviewModel.medias || [],
        otherFiles: this.data.lightboxPreviewModel.otherFiles || [],
        filesToDisplay: this.data.lightboxPreviewModel.filesToDisplay,
        additionalImages: this.data.lightboxPreviewModel.additionalImages,
        hiddenImagesCount: this.data.lightboxPreviewModel.hiddenImagesCount
      },
      id: this.data.id,
      groupHeader: this.data.groupHeader,
      links: this.data.links,
      canDelete: !!this.data.canDelete,
      canEdit: !this.data.requiresRedirect,
      name: this.data.name,
      tagIdsData: new Array<string>(),
      newMedia: null,
      media: null,
      mediaRootId: this.data.mediaRootId
    };
  }

  public handleImageRemove(image): void {
    this.socialEdit.lightboxPreviewModel.medias =
      this.socialEdit.lightboxPreviewModel.medias.filter(m => m !== image);
    this.hasDataChangedService.onDataChanged();
  }

  public handleFileRemove(file): void {
    this.socialEdit.lightboxPreviewModel.otherFiles =
      this.socialEdit.lightboxPreviewModel.otherFiles.filter(m => m !== file);
    this.hasDataChangedService.onDataChanged();
  }

  public handleUpload(file: Array<object>): void {
    this.uploadedData.push(file);
    this.hasDataChangedService.onDataChanged();
  }

  public handleRemove(file: object): void {
    this.uploadedData = this.uploadedData.filter(d => d[0] !== file);
  }

  public onTagsChange(e): void {
    if (this.socialEdit.tags !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.socialEdit.tags = e;
  }

  public onDescriptionChange(e): void {
    if (this.socialEdit.description !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.socialEdit.description = e;
  }

  public handleSocialUpdate(): void {
    this.socialEdit.media = '';

    const otherFilesIds = this.socialEdit.lightboxPreviewModel.otherFiles
      .map(m => m.id);
    const mediaIds = this.socialEdit.lightboxPreviewModel.medias
      .map(m => m.id);

    this.socialEdit.media = otherFilesIds.concat(mediaIds).join(',');
    this.socialEdit.newMedia = this.uploadedData.map(u => u[1]).join(',');
    this.socialEdit.tagIdsData = this.socialEdit.tags.map(t => t.id);
    this.inProgress = true;

    this.$updateSubscription = this.socialService.updateSocial(this.socialEdit)
      .subscribe(
        (next: any) => {
          this.routerResolverService.removePageRouter(next.originalUrl);
          this.hasDataChangedService.reset();
          this.router.navigate([next.originalUrl]);
        },
        (err: any) => {
          this.inProgress = false;
        }
      );
  }

  public handleSocialDelete(): void {
    if (confirm(this.translate.instant('common.AreYouSure'))) {
      this.inProgress = true;

      this.$deleteSubscription = this.socialService.deleteSocial(this.socialEdit.id)
        .subscribe(
          (next) => {
            this.router.navigate([this.socialEdit.links.feed.originalUrl]);
          },
          (err) => {
            this.inProgress = false;
          },
        );
    }
  }

  private initSocialEditForm(): void {
    this.socialEditForm = new FormGroup({
      description: new FormControl(this.socialEdit.description, Validators.required)
    });
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
