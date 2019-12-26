import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ISocialDetails } from './social-details.interface';
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
  tags: any;
  activityName: string;
  likeData: ILikeData;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    this.details = JSON.parse(JSON.stringify(this.data.details.get()));
    this.activityName = this.parseActivityType(this.details.activityType);
    this.tags = Object.values(JSON.parse(JSON.stringify(this.data.tags.get())));
    debugger;
    // this.likeData = {
    //   likedByCurrentUser: this.publication.activity.likedByCurrentUser,
    //   id: this.publication.activity.id,
    //   activityType: this.publication.activity.activityType,
    //   likes: this.publication.activity.likes
    // };
  }

  public ngOnDestroy(): void {
    console.log('died');
  }

  public parseActivityType(activityType: number): string {
    return ActivityEnum[activityType];
  }
}
