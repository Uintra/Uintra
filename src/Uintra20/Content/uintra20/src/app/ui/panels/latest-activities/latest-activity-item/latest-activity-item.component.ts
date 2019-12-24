import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'latest-activity',
  templateUrl: './latest-activity-item.component.html',
  styleUrls: ['./latest-activity-item.component.less']
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
  }

  public navigateToActivity = (): void => {
    this.router.navigate([this.activityLink]);
  }
}
