import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { GroupsService } from 'src/app/feature/project/specific/groups/groups.service';
import { IGroupRoomData } from 'src/app/feature/project/specific/groups/groups.interface';
import { RouterResolverService } from 'src/app/services/general/router-resolver.service';
import { IULink } from 'src/app/feature/shared/interfaces/general.interface';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/services/general/can-deactivate.service';

@Component({
  selector: 'uintra-groups-room-page',
  templateUrl: './uintra-groups-room-page.html',
  styleUrls: ['./uintra-groups-room-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsRoomPage {
  data: any;
  parsedData: IGroupRoomData;
  isLoading: boolean;

  constructor(
    private route: ActivatedRoute,
    private groupsService: GroupsService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(data);
      this.addButtonService.setPageId(data.id);
      this.routerResolverService.removePageRouter(this.parsedData.groupInfo.groupUrl.originalUrl);
    });
  }

  toggleSubscribe() {
    if (!this.parsedData.groupInfo.isMember || confirm('Are you sure?')) {
      this.isLoading = true;
      this.groupsService.toggleSubscribe(this.parsedData.groupId)
      .then((res: IULink) => {
        if (this.parsedData.groupInfo.isMember) {
          this.parsedData.groupInfo.membersCount -= 1;
          this.parsedData.groupInfo.isMember = false;
        } else {
          this.parsedData.groupInfo.membersCount += 1;
          this.parsedData.groupInfo.isMember = true;
        }
        this.routerResolverService.removePageRouter(res.originalUrl);
        document.location.reload();
      })
      .finally(() => {
        this.isLoading = false;
      })
    }
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
