import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import {
  IUserTag,
  IMedia,
  IDocument
} from 'src/app/feature/specific/activity/activity.interfaces';
import { ImageGalleryService } from 'src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';
import { EventSubscriptionService } from '../../../../feature/specific/activity/event-subscription/event-subscription.service';
import { IEventDetails } from '../../../../feature/specific/activity/event-form/event-form.interface';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable, Subscription } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IEventDetailsPage } from 'src/app/shared/interfaces/pages/event/details/event-details-page.interface';
import { ICommentData } from 'src/app/shared/interfaces/panels/comments/comments-panel.interface';
import {TranslateService} from "@ngx-translate/core";
import {map} from 'rxjs/operators';
import {BreakpointObserver, BreakpointState} from '@angular/cdk/layout';

@Component({
  selector: 'event-details-page',
  templateUrl: './event-details-page.html',
  styleUrls: ['./event-details-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class EventDetailsPage implements OnInit, OnDestroy {

  data: IEventDetailsPage;
  details: IEventDetails;
  tags: Array<IUserTag>;
  activityName: string;
  likeData: ILikeData;
  medias: Array<IMedia> = new Array<IMedia>();
  documents: Array<IDocument> = new Array<IDocument>();
  commentDetails: ICommentData;
  detailsDescription: SafeHtml;
  detailsTitle: SafeHtml;
  fullEventTime: Array<string>;
  subscribers: string[] = [];
  $eventSubscription: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private imageGalleryService: ImageGalleryService,
    private sanitizer: DomSanitizer,
    private eventSubscription: EventSubscriptionService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private translateService: TranslateService
  ) {
    this.activatedRoute.data.subscribe((data: IEventDetailsPage) => {
      this.data = data;
    });
  }

  get locationUrl() {
    if (this.data) {
      return (
        'http://maps.google.co.uk/maps?q=' +
        this.data.details.location.address
      );
    }
  }

  public ngOnInit(): void {
    if (this.data) {
      this.$eventSubscription = this.eventSubscription.getListOfUsers(this.data.details.id).subscribe((res: string[]) => {
        this.subscribers = res;
      });

      this.details = this.data.details;
      this.commentDetails = {
        entityId: this.data.details.id,
        entityType: this.data.details.activityType
      };
      this.activityName = this.translateService.instant('eventDetails.Title');
      this.tags = this.data.tags;
      this.medias = this.data.details.lightboxPreviewModel.medias;
      this.documents = this.data.details.lightboxPreviewModel.otherFiles;
      this.fullEventTime = this.data.details.fullEventTime;
      this.detailsDescription = this.sanitizer.bypassSecurityTrustHtml(this.details.description);
      this.detailsTitle = this.sanitizer.bypassSecurityTrustHtml(this.details.headerInfo.title);
    }
  }

  public ngOnDestroy(): void {
    if (this.$eventSubscription) { this.$eventSubscription.unsubscribe(); }
  }

  public openGallery(i): void {
    const items = this.medias.map(el => {
      if (el.extension === 'mp4') {
        return {
          html: `<div class='gallery__video'>
                  <div class='pswp__video-box'>
                    <video class='pswp__video' src='${el.url}' controls=''></video>
                  <\div>
                <\div>`,
          w: el.width,
          h: el.height
        };
      } else {
        return {
          src: el.url,
          w: el.width,
          h: el.height
        };
      }
    });

    this.imageGalleryService.open(items, i);
  }

  public toggleNotification(val: boolean) {
    this.eventSubscription.toggleNotification(this.data.details.id, val).subscribe(res => {
      this.data.details.isNotificationsDisabled = val;
    });
  }

  public toggleSubscription(): void {
    (this.data.details.isSubscribed
      ? this.eventSubscription.unsubscribe(this.data.details.id)
      : this.eventSubscription.subscribe(this.data.details.id))
      .subscribe((res: string[]) => {
        this.subscribers = res;
        this.data.details.isSubscribed = !this.data.details.isSubscribed;
      });
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
