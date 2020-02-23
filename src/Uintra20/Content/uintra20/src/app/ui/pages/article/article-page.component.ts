import { Component, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { DeactivationGuarded } from "src/app/services/general/can-deactivate.service";
import { HasDataChangedService } from "src/app/services/general/has-data-changed.service";
import { Observable, Subject, BehaviorSubject, Subscriber } from "rxjs";
import { AddButtonService } from '../../main-layout/left-navigation/components/my-links/add-button.service';

@Component({
  selector: "article-page",
  templateUrl: "./article-page.html",
  styleUrls: ["./article-page.less"],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage implements DeactivationGuarded {
  data: any;
  isShow: boolean;

  subscriber: Subscriber<boolean>;
  isConfirm: boolean;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.addButtonService.setPageId(data.id);
    });
    this.route.data.subscribe(data => (this.data = data));
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      if(confirm('Are you sure?')) {
        this.hasDataChangedService.reset();
        return true;
      }

      return false;
    }

    return true;
  }

  onSubmit() {
    this.subscriber.next(true);
    this.subscriber.complete();
    this.isConfirm = true;
  }

  onCancel() {
    this.isConfirm = false;
    this.subscriber.next(false);
    this.isShow = false;
  }
}
