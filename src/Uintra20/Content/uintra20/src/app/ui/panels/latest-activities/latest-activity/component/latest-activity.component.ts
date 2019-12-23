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
  public activityLink: string;

  constructor(
    private router: Router,
    private activityLinkService: ActivityLinkService) { }

  ngOnInit() {
    this.activityLink = this.activityLinkService.getBulletinLink(ActivityType.Bulletins, this.activityId);
  }

  public navigateToActivity = (): void => {
    console.log('Navigate to activity works');
    // this.router.navigate([this.activityLink]);
  }
}
