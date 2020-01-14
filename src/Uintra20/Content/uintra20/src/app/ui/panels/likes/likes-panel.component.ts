import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LikeButtonService } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.service';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

export interface ILikesPanelData {
  memberId: string;
  entityId: string;
  canAddLike: boolean;
  isReadOnly: boolean;
  showTitle: boolean;
  likes: any;
  contentTypeAlias: string;
}

@Component({
  selector: 'likes-panel',
  templateUrl: './likes-panel.html',
  styleUrls: ['./likes-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LikesPanel {
  data: any;
  panelData: ILikesPanelData;
  likeData: ILikeData;
  constructor(
    private route: ActivatedRoute,
    private likeButtonService: LikeButtonService) {
    this.route.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.likeData = {
      likedByCurrentUser: (this.panelData.canAddLike),
      id: this.panelData.entityId,
      likes: Object.values(this.panelData.likes)
    };
  }
}
