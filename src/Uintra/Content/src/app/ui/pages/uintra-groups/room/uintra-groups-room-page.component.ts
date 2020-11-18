
import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IULink } from 'src/app/shared/interfaces/general.interface';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { GroupsService } from 'src/app/feature/specific/groups/groups.service';
import { TranslateService } from '@ngx-translate/core';
import { ResolveService } from 'ubaseline-next-for-uintra';
import { IUintraGroupsRoomPage } from 'src/app/shared/interfaces/pages/uintra-groups/room/uintra-groups-room-page.interface';
import { ISocialCreate } from 'src/app/shared/interfaces/components/social/create/social-create.interface';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'uintra-groups-room-page',
  templateUrl: './uintra-groups-room-page.html',
  styleUrls: ['./uintra-groups-room-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsRoomPage implements OnInit {

  public data: IUintraGroupsRoomPage;
  public isLoading: boolean;
  public socialCreateData: ISocialCreate;

  constructor(
    private route: ActivatedRoute,
    private groupsService: GroupsService,
    private router: Router,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private translate: TranslateService,
    private resolveService: ResolveService,
    private appService: AppService
  ) {
    this.route.data.subscribe((data: IUintraGroupsRoomPage) => {
      this.data = data;
      this.appService.setPageAccess(data.allowAccess);
    });
  }

  public ngOnInit(): void {
    this.setSocialCreateData();
  }

  public toggleSubscribe(): void {
    if (!this.data.groupInfo.isMember || confirm(this.translate.instant('groupInfo.Unsubscribe.Message.lnk'))) {
      this.isLoading = true;
      this.groupsService.toggleSubscribe(this.data.groupId)
        .then(async (res: IULink) => {
          if (this.data.groupInfo.isMember) {
            this.data.groupInfo.membersCount -= 1;
            this.data.groupInfo.isMember = false;
          } else {
            this.data.groupInfo.membersCount += 1;
            this.data.groupInfo.isMember = true;
          }

          this.data = await this.resolveService.resolveDataOnSameUrl(this.router.url);
          this.setSocialCreateData();
        })
        .finally(() => {
          this.isLoading = false;
        });
    }
  }

  public getMembersText(): string {
    return this.data.groupInfo.membersCount === 1
      ? this.translate.instant('groupInfo.OneMemberCount.lbl')
      : this.translate.instant('groupInfo.MembersCount.lbl');
  }

  public getSubscribeBtn(): string {
    return this.data.groupInfo.isMember
      ? this.translate.instant('groupInfo.Unsubscribe.lnk')
      : this.translate.instant('groupInfo.Subscribe.lnk');
  }

  setSocialCreateData() {
    this.socialCreateData = this.data.socialCreateModel;
    if (this.socialCreateData) {
      this.socialCreateData.canCreate = !this.data.socialCreateModel.requiresRedirect;
      this.socialCreateData.createNewsLink = this.data.createNewsLink;
      this.socialCreateData.createEventsLink = this.data.createEventsLink;
    }
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
