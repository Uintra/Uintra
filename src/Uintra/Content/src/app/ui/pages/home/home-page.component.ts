import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IHomePage } from 'src/app/shared/interfaces/pages/home/home-page.interface';
import { ISocialCreate } from 'src/app/shared/interfaces/components/social/create/social-create.interface';
import { Indexer } from '../../../shared/abstractions/indexer';

@Component({
  selector: 'home-page',
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomePage extends Indexer<number> implements OnInit {

  public data: IHomePage;
  public latestActivities: any;
  public socialCreateData: ISocialCreate;
  public otherPanels: any;

  constructor(
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    super();
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
}
