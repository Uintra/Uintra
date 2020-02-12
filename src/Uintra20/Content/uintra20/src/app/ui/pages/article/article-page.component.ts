import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DeactivationGuarded } from 'src/app/services/general/can-deactivate.service';
import { HasDataChangedService } from 'src/app/services/general/has-data-changed.service';

@Component({
  selector: 'article-page',
  templateUrl: './article-page.html',
  styleUrls: ['./article-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage implements DeactivationGuarded {
  data: any;
  hasDataChanged: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private hasDataChangedService: HasDataChangedService
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  canDeactivate(): boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      if(confirm('Are you sure, bla bla bla?')) {
        return true;
      } else {
        return false;
      }
    }

    return true;
  }
}
