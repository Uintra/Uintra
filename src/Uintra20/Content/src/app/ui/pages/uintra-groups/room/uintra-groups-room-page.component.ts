import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { RouterResolverService } from 'src/app/shared/services/general/router-resolver.service';
import { IULink } from 'src/app/shared/interfaces/general.interface';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IGroupRoomData } from 'src/app/feature/specific/groups/groups.interface';
import { GroupsService } from 'src/app/feature/specific/groups/groups.service';
import { TranslateService } from '@ngx-translate/core';

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
  socialCreateData: any;

  constructor(
    private route: ActivatedRoute,
    private groupsService: GroupsService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private translate: TranslateService,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(data);
      this.addButtonService.setPageId(data.id);
      this.routerResolverService.removePageRouter(this.parsedData.groupInfo.groupUrl.originalUrl);
    });
  }

  ngOnInit() {
    this.socialCreateData = this.data.socialCreateModel.get().data.get();
    this.socialCreateData.canCreate = this.data.socialCreateModel.get().canCreate.get();
    this.socialCreateData.createNewsLink = this.data.createNewsLink.get();
    this.socialCreateData.createEventsLink = this.data.createEventsLink.get();
  }

  toggleSubscribe() {
    if (!this.parsedData.groupInfo.isMember || confirm(this.translate.instant('groupInfo.Unsubscribe.Message.lnk'))) {
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

  getMembersText() {
    return this.parsedData.groupInfo.membersCount === 1
      ? this.translate.instant('groupInfo.OneMemberCount.lbl')
      : this.translate.instant('groupInfo.MembersCount.lbl');
  }

  getSubscribeBtn() {
    return this.parsedData.groupInfo.isMember
      ? this.translate.instant('groupInfo.Unsubscribe.lnk')
      : this.translate.instant('groupInfo.Subscribe.lnk');
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
