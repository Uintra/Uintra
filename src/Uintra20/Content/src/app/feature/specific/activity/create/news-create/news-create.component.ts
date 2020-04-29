import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { RouterResolverService } from 'src/app/shared/services/general/router-resolver.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import { INewsCreateModel, IActivityCreatePanel } from 'src/app/feature/specific/activity/activity.interfaces';
import { Observable } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';

@Component({
  selector: 'app-news-create',
  templateUrl: './news-create.component.html',
  styleUrls: ['./news-create.component.less']
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  newsData: INewsCreateModel;
  members: Array<any>;
  creator: any;
  tags: Array<any>;
  isSubmitLoading = false;

  constructor(
    private activityService: ActivityService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) { }

  ngOnInit() {
    this.members = this.data.members as Array<any> || [];
    this.creator = this.data.creator;
    this.tags = this.data.tags;

    this.newsData = {
      ownerId: this.creator.id,
      title: null,
      description: null,
      publishDate: null
    };
  }

  onSubmit(data) {
    this.isSubmitLoading = true;

    this.activityService.submitNewsContent(data).subscribe(
      (r: any) => {
        this.routerResolverService.removePageRouter(r.originalUrl);
        this.hasDataChangedService.reset();
        this.router.navigate([r.originalUrl]);
      },
      err => {
        this.isSubmitLoading = false;
      }
    );
  }

  public onCancel() {
    this.router.navigate([this.data.links.feed.originalUrl]);
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
