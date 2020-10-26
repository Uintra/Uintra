import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { LikeButtonService, IAddLikeRequest, IRemoveLikeRequest } from './like-button.service';
import { ILikeData, IUserLikeData } from './like-button.interface';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-like-button',
  templateUrl: './like-button.component.html',
  styleUrls: ['./like-button.component.less']
})
export class LikeButtonComponent implements OnInit, OnDestroy {

  @Input()
  public likeData: ILikeData;
  @Input()
  public isDisabled = false;

  public likeCount: number = null;
  public userLikes: Array<IUserLikeData> = [];
  public defaultLike: string;

  public addLikeSubscription: Subscription;
  public removeLikeSubscription: Subscription;

  public get likes() {
    if (this.userLikes.length === 0) {
      return this.defaultLike;
    }
    if (this.likeCount !== this.userLikes.length) {
      return this.userLikes.length;
    }

    return this.likeCount;
  }

  constructor(
    private likeButtonService: LikeButtonService,
    private translateService: TranslateService
  ) { }

  public ngOnInit(): void {
    this.defaultLike = this.translateService.instant('activity.Like.lnk');
    this.likeCount = this.likeData.likes && this.likeData.likes.length;
    this.userLikes = this.likeData.likes;
  }

  public ngOnDestroy(): void {
    if (this.addLikeSubscription) { this.addLikeSubscription.unsubscribe(); }
    if (this.removeLikeSubscription) { this.removeLikeSubscription.unsubscribe(); }
  }

  public onClickLike(): void {
    return this.likeData.likedByCurrentUser === false
      ? this.addLike(this.getLikeRequest())
      : this.removeLike(this.getLikeRequest());
  }

  public addLike(data: IAddLikeRequest): void {
    this.addLikeSubscription = this.likeButtonService
      .addLike(data)
      .subscribe((next: Array<IUserLikeData>) => {
        this.userLikes = next;
        this.likeCount += 1;
        this.likeData.likedByCurrentUser = true;
      });
  }

  public removeLike(data: IRemoveLikeRequest): void {
    this.removeLikeSubscription = this.likeButtonService
      .removeLike(data)
      .subscribe((next: Array<IUserLikeData>) => {
        this.userLikes = next;
        this.likeCount = this.likeCount > 1
          ? this.likeCount - 1
          : 0;
        this.likeData.likedByCurrentUser = false;
      });
  }

  private getLikeRequest = (): IAddLikeRequest => {
    return {
      entityId: this.likeData.id,
      entityType: this.likeData.activityType
    };
  }
}
