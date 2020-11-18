import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/app.service';
import { INotFoundPage } from 'src/app/shared/interfaces/pages/not-found/not-found-page.inteface';

@Component({
  selector: 'page-not-found-page',
  templateUrl: './page-not-found-page.html',
  styleUrls: ['./page-not-found-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class PageNotFoundPage {

  public data: INotFoundPage;

  constructor(
    private route: ActivatedRoute,
    private appService: AppService

  ) {
    this.route.data.subscribe((data: INotFoundPage) => {
      this.data = data;
      this.appService.setPageAccess(data.allowAccess);
    });
  }
}
