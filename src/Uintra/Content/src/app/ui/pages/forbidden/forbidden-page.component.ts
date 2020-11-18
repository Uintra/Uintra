import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'forbidden-page',
  templateUrl: './forbidden-page.html',
  styleUrls: ['./forbidden-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ForbiddenPage {

  public data: any;

  constructor(
    private route: ActivatedRoute,
    private appService: AppService
  ) {
    this.route.data.subscribe((data: any) => {
      this.data = data;
      this.appService.setPageAccess(data.allowAccess);
    });
  }
}
