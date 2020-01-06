import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'home-page',
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomePage implements OnInit {

  data: any;
  latestActivities: any;
  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  ngOnInit(): void {
    this.latestActivities = this.data.panels.get().filter(p => p.data.contentTypeAlias === 'latestActivitiesPanel')[0];
  }

}
