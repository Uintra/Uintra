import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { GroupsService } from 'src/app/feature/project/specific/groups/groups.service';
import { IGroupRoomData } from 'src/app/feature/project/specific/groups/groups.interface';

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
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(data);
    });
  }

  toggleSubscribe() {
    this.isLoading = true;
    this.groupsService.toggleSubscribe(this.parsedData.groupId)
      .then(res => {
        if (this.parsedData.groupInfo.isMember) {
          this.parsedData.groupInfo.membersCount -= 1;
          this.parsedData.groupInfo.isMember = false;
        } else {
          this.parsedData.groupInfo.membersCount += 1;
          this.parsedData.groupInfo.isMember = true;
        }
      })
      .finally(() => {
        this.isLoading = false;
      })
  }
}
