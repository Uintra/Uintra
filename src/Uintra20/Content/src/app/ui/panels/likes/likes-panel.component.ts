import { Component, HostBinding, OnInit } from '@angular/core';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';
import { ILikesPanel } from 'src/app/shared/interfaces/panels/likes/likes-panel.interface';

@Component({
  selector: 'likes-panel',
  templateUrl: './likes-panel.html',
  styleUrls: ['./likes-panel.less']
})
export class LikesPanel implements OnInit {
  @HostBinding('class')
  public hostClass;
  public data: ILikesPanel;
  public likeData: ILikeData;
  public isDisabled: boolean;
  public isContentPage: boolean;

  constructor() { }

  public ngOnInit(): void {
    this.likeData = {
      likedByCurrentUser: !!this.data.likedByCurrentUser,
      id: this.data.entityId,
      likes: this.data.likes,
      activityType: this.data.activityType && this.data.activityType.toString()
    };
    this.isDisabled = this.data.isGroupMember;
    this.isContentPage = this.data.activityType && this.data.activityType.toString() === '6';
    if (this.isContentPage) {
      this.hostClass = 'likes-panel--content';
    }
  }
}
