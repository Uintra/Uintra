import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ISocialDetails } from './social-details.interface';
import { ActivityEnum } from 'src/app/feature/shared/enums/activity-type.enum';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-page.component.html',
  styleUrls: ['./social-details-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit, OnDestroy {

  data: any;
  details: ISocialDetails;
  activityName: string;
  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    this.details = JSON.parse(JSON.stringify(this.data.details.get()));
    console.log(this.details);
    const parsedActivityName = this.parse(this.details.activityType);
    this.activityName = parsedActivityName.toLowerCase() + '/edit';
    console.log(this.activityName);
  }

  public ngOnDestroy(): void {
    console.log('died');
  }

  public parse(activityType: number): string {
    return ActivityEnum[activityType];
  }
}
