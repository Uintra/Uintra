import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ISocialDetails, IUserTag } from './social-details.interface';
import { ActivityEnum } from 'src/app/feature/shared/enums/activity-type.enum';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-page.component.html',
  styleUrls: ['./social-details-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit, OnDestroy {

  data: any;
  details: ISocialDetails;
  tags: Array<IUserTag>;
  activityName: string;
  likeData: ILikeData;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    const parsedData = JSON.parse(JSON.stringify(this.data));

    this.details = parsedData.details;
    this.activityName = this.parseActivityType(this.details.activityType);
    this.tags = Object.values(parsedData.tags);

    this.likeData = {
      likedByCurrentUser: parsedData.likedByCurrentUser,
      id: parsedData.details.id,
      activityType: parsedData.details.activityType,
      likes: Object.values(parsedData.likes)
    };
  }

  public ngOnDestroy(): void {
    console.log('died');
  }

  public parseActivityType(activityType: number): string {
    return ActivityEnum[activityType];
  }
}
