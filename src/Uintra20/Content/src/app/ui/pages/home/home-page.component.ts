import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddButtonService } from '../../main-layout/left-navigation/components/my-links/add-button.service';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';

@Component({
  selector: 'home-page',
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomePage implements OnInit {

  data: any;
  latestActivities: any;
  otherPanels: any;
  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.addButtonService.setPageId(data.id);
    });
  }

  ngOnInit(): void {
    if (this.data.panels) {
      this.latestActivities = this.data.panels.get().filter(p => p.data.contentTypeAlias === 'latestActivitiesPanel')[0];
      this.otherPanels = this.data.panels.get().filter(p => p.data.contentTypeAlias !== 'latestActivitiesPanel');
    }
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
