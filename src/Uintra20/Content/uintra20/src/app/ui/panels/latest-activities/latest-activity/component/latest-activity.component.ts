import { Component, OnInit, Input } from '@angular/core';
import { ActivityLinkService } from '../services/activity-link.service';
import { ActivityType } from 'src/app/feature/shared/enums/activity-type.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'latest-activity',
  templateUrl: './latest-activity.component.html',
  styleUrls: ['./latest-activity.component.less']
})
export class LatestActivityComponent implements OnInit {
  @Input()
  public readonly activityType: string;
  @Input()
  public readonly activityDate: Date;
  @Input()
  public readonly activityDescription: string;
  @Input()
  public readonly activityId: string;
  @Input()
  public readonly activityLink: string;

  constructor(
    private router: Router) { }

  ngOnInit() {
    console.log(this.activityLink);
  }

  public navigateToActivity = (): void => {
    this.router.navigate([this.activityLink]);
  }
}
