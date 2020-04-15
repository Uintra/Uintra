import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';

export interface ILikesPanelData {
  entityId: string;
  activityType: string;
  likedByCurrentUser: boolean;
  isReadOnly: boolean;
  showTitle: boolean;
  likes: any;
  contentTypeAlias: string;
  isGroupMember: boolean;
}

@Component({
  selector: 'likes-panel',
  templateUrl: './likes-panel.html',
  styleUrls: ['./likes-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LikesPanel {
  @HostBinding('class') hostClass;
  //TODO: Change data interface from any to appropriate one once you remove UFP from this panel and remove first three lines in ngOnInit()
  data: any;
  panelData: ILikesPanelData;
  likeData: ILikeData;
  isDisabled: boolean;
  isContentPage: boolean;

  constructor(
    private route: ActivatedRoute) {
    this.route.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    if (this.data.get) {
      this.data = this.data.get();
    }
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.likeData = {
      likedByCurrentUser: !!this.panelData.likedByCurrentUser,
      id: this.panelData.entityId,
      likes: Object.values(this.panelData.likes),
      activityType: this.panelData.activityType
    };
    this.isDisabled = this.panelData.isGroupMember;
    this.isContentPage = this.panelData.activityType == '6';
    if (this.isContentPage) {
      this.hostClass = "likes-panel--content"
    }
  }
}
