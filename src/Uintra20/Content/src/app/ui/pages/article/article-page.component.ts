import { Component, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DeactivationGuarded, CanDeactivateGuard } from "src/app/shared/services/general/can-deactivate.service";
import { HasDataChangedService } from "src/app/shared/services/general/has-data-changed.service";
import { Observable } from "rxjs";
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: "article-page",
  templateUrl: "./article-page.html",
  styleUrls: ["./article-page.less"],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage {
  data: any;
  parsedData: any;

  constructor(
    private route: ActivatedRoute,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(data);
    });
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
