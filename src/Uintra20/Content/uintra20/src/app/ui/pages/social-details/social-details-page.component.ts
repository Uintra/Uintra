import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-page.component.html',
  styleUrls: ['./social-details-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit {

  data: any;
  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = this.addActivityTypeProperty(data));
   }

  private addActivityTypeProperty(data) {
    // TODO investigate UmbracoFlatProperty and refactor code below
    data.panels.data.value = data.panels.get().map(panel => {
      // panel.data.value.activityType = data.details.get().activityType.get();
      return panel
    })
    
    return data;
  }

  public ngOnInit(): void {
    console.log(this.data);
    console.log('Social Detail Works');
  }
}
