import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'uintraewsetailsage-page',
  templateUrl: './uintraewsetailsage-page.html',
  styleUrls: ['./uintraewsetailsage-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraewsetailsagePage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}