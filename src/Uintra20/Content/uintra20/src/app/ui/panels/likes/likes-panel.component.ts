import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';

@Component({
  selector: 'likes-panel',
  templateUrl: './likes-panel.html',
  styleUrls: ['./likes-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LikesPanel implements OnInit {

  data: any;
  parsedData: any;
  likeData: ILikeData;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    this.parseData();
    this.onInitLike();
  }

  private onInitLike(): void {
    this.likeData = {
      likedByCurrentUser: null,
      id: this.parsedData.entityId,
      activityType: this.parsedData.activityType,
      likes: null
    };
  }

  private parseData(): void {
    this.parsedData = JSON.parse(JSON.stringify(this.data));
  }
}
