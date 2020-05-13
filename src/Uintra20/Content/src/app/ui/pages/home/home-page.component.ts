import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IHomePage } from 'src/app/shared/interfaces/pages/home/home-page.interface';
import { ISocialCreate } from 'src/app/shared/interfaces/components/social/create/social-create.interface';

@Component({
  selector: 'home-page',
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomePage implements OnInit {

  public data: IHomePage;
  public latestActivities: any;
  public socialCreateData: ISocialCreate;
  public otherPanels: any;

  constructor(
    private route: ActivatedRoute,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe((data: IHomePage) => this.data = data);
  }

  public ngOnInit(): void {
    if (this.data.panels) {
      this.latestActivities = this.data.panels.filter(p => p.contentTypeAlias === 'imagePanel')[0];
      this.otherPanels = this.data.panels.filter(p => p.contentTypeAlias !== 'imagePanel');//latestActivitiesPanel
    }
    this.socialCreateData = this.data.socialCreateModel;
    if (this.socialCreateData) {
      this.socialCreateData.canCreate = !this.data.socialCreateModel.requiresRedirect;
      this.socialCreateData.createNewsLink = this.data.createNewsLink;
      this.socialCreateData.createEventsLink = this.data.createEventsLink;
    }
  }

  public canDeactivate(): Observable<boolean> | boolean {
    return this.hasDataChangedService.hasDataChanged
      ? this.canDeactivateService.canDeacrivateConfirm()
      : true;
  }

  public index = (index): number => index;
}
