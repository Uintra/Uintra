import { Component, OnInit, Input } from '@angular/core';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'latest-activity',
  templateUrl: './latest-activity-item.component.html',
  styleUrls: ['./latest-activity-item.component.less']
})
export class LatestActivityComponent implements OnInit {
  @Input() activityType: string;
  @Input() activityDate: Date;
  @Input() activityId: string;
  @Input() activityDescription: string;
 // @Input() activityLinks: { details: { baseUrl: string; params: Array<{name: string, value: string}>}};
  @Input() activityLinks: any;

  get detailsParams() {
    // TODO: fix UmbracoFlatProperty
    const paramsArray = Object.values(this.activityLinks.details.get().params.get());

    return paramsArray.reduce((acc, val: any) => {
     acc[val.data.name] = val.data.value;
      return acc;
    }, {});
  }

  sanitizedActivityDescription: SafeHtml;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.sanitizedActivityDescription = this.sanitizer.bypassSecurityTrustHtml(this.activityDescription);
  }
}
