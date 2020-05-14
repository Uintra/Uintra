import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable, Subscription } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { INewsCreateModel, IOwner } from 'src/app/feature/specific/activity/activity.interfaces';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import { IUintraNewsEditPage } from 'src/app/shared/interfaces/pages/news/edit/uintra-news-edit-page.interface';

@Component({
  selector: 'uintra-news-edit-page',
  templateUrl: './uintra-news-edit-page.html',
  styleUrls: ['./uintra-news-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsEditPage implements OnInit, OnDestroy {

  private $newsSubscription: Subscription;

  public data: IUintraNewsEditPage;
  public newsData: INewsCreateModel;
  public details: any;
  public members: IOwner[];
  public creator: IOwner;
  public tags: ITagData[];

  constructor(
    private route: ActivatedRoute,
    private activityService: ActivityService,
    private router: Router,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe((data: IUintraNewsEditPage) => this.data = data);
  }

  public ngOnInit(): void {
    if (this.data) {
      this.details = this.data.details;
      this.members = this.data.members as Array<any> || [];
      this.creator = this.details.headerInfo.owner;
      this.tags = this.details.availableTags;
      this.newsData = {
        ownerId: this.details.ownerId,
        title: this.details.headerInfo.title,
        description: this.details.description,
        publishDate: this.details.publishDate,
        unpublishDate: this.details.unpublishDate,
        media: {
          medias: this.details.lightboxPreviewModel.medias as Array<any> || [],
          otherFiles: this.details.lightboxPreviewModel.otherFiles as Array<any> || []
        },
        endPinDate: this.details.endPinDate,
        isPinned: this.details.isPinned,
        location: {
          address: this.details.location.address,
          shortAddress: this.details.location.shortAddress
        },
        tags: this.details.tags as Array<any> || []
      };
    }
  }

  public ngOnDestroy(): void {
    if (this.$newsSubscription) { this.$newsSubscription.unsubscribe(); }
  }

  public onSubmit(data) {
    const copyObject = this.requesModelBuilder(data);

    this.$newsSubscription = this.activityService.updateNews(copyObject).subscribe((r: any) => {
      this.hasDataChangedService.reset();
      this.router.navigate([r.originalUrl]);
    });
  }

  public requesModelBuilder(data) {
    const otherFilesIds = data.media.otherFiles.map(m => m.id);
    const mediaIds = data.media.medias.map(m => m.id);

    data.media = otherFilesIds.concat(mediaIds).join(',');
    data['id'] = this.details.id;

    return data;
  }

  public onCancel(): void {
    this.router.navigate([this.details.links.details.originalUrl]);
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
