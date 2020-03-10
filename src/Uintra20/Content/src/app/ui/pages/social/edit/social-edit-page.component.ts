import { Component, ViewEncapsulation, HostListener } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import ParseHelper from "../../../../shared/utils/parse.helper";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { RouterResolverService } from "src/app/shared/services/general/router-resolver.service";
import { AddButtonService } from "src/app/ui/main-layout/left-navigation/components/my-links/add-button.service";
import { Observable } from "rxjs";
import { HasDataChangedService } from "src/app/shared/services/general/has-data-changed.service";
import { CanDeactivateGuard } from "src/app/shared/services/general/can-deactivate.service";
import { ActivityService } from "src/app/feature/specific/activity/activity.service";
import { ISocialEdit } from 'src/app/feature/specific/activity/activity.interfaces';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'social-edit',
  templateUrl: './social-edit-page.component.html',
  styleUrls: ['./social-edit-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialEditPageComponent {

  private data: any;
  public files: Array<any> = new Array<any>();
  public inProgress = false;
  public socialEdit: ISocialEdit;
  public uploadedData: Array<any> = new Array<any>();
  public socialEditForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private socialService: ActivityService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private translate: TranslateService
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.addButtonService.setPageId(data.id);
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
    this.onParse();
    this.initSocialEditForm();
  }

  private onParse = (): void => {
    const parsedSocialEdit = ParseHelper.parseUbaselineData(this.data);
    this.socialEdit = {
      ownerId: parsedSocialEdit.ownerId,
      description: parsedSocialEdit.description,
      tags: Object.values(parsedSocialEdit.tags),
      availableTags: Object.values(parsedSocialEdit.availableTags),
      lightboxPreviewModel: {
        medias: Object.values(parsedSocialEdit.lightboxPreviewModel.medias || []),
        otherFiles: Object.values(parsedSocialEdit.lightboxPreviewModel.otherFiles || []),
        filesToDisplay: parsedSocialEdit.lightboxPreviewModel.filesToDisplay,
        additionalImages: parsedSocialEdit.lightboxPreviewModel.additionalImages,
        hiddenImagesCount: parsedSocialEdit.lightboxPreviewModel.hiddenImagesCount
      },
      id: parsedSocialEdit.id,
      groupHeader: parsedSocialEdit.groupHeader,
      links: parsedSocialEdit.links,
      canDelete: !!parsedSocialEdit.canDelete,
      canEdit: !parsedSocialEdit.requiresRedirect,
      name: parsedSocialEdit.name,
      tagIdsData: new Array<string>(),
      newMedia: null,
      media: null,
      mediaRootId: parsedSocialEdit.mediaRootId
    };
  }

  @HostListener('window:beforeunload') checkIfDataChanged() {
    return !this.hasDataChangedService.hasDataChanged;
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
    if (this.socialEdit.tags != e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.socialEdit.tags = e;
  }

  public onDescriptionChange(e): void {
    if (this.socialEdit.description != e) {
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

    this.socialService.updateSocial(this.socialEdit)
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

      this.socialService.deleteSocial(this.socialEdit.id)
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

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
