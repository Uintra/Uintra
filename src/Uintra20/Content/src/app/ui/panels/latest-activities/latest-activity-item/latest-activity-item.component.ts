import { Component, OnInit, Input } from '@angular/core';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'latest-activity',
  templateUrl: './latest-activity-item.component.html',
  styleUrls: ['./latest-activity-item.component.less']
})
export class LatestActivityComponent implements OnInit {
  @Input() public activityType: string;
  @Input() public activityDate: Date;
  @Input() public activityId: string;
  @Input() public activityDescription: string;
  @Input() public activityLinks: any;
  @Input() public activityTitle: string;

  sanitizedActivityDescription: SafeHtml;

  constructor(private sanitizer: DomSanitizer) { }

  public ngOnInit(): void {
    debugger;
    this.activityDate = Object.values(this.activityDate)[0];
    this.sanitizedActivityDescription = this.sanitizer.bypassSecurityTrustHtml(this.activityDescription);
  }
}
