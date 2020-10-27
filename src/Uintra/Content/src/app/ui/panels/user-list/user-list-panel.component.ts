import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { IUserListPanel } from 'src/app/shared/interfaces/panels/user-list/user-list-panel.interface';

@Component({
  selector: 'user-list-panel',
  templateUrl: './user-list-panel.html',
  styleUrls: ['./user-list-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserListPanel implements OnInit {

  public data: IUserListPanel;

  constructor(private translate: TranslateService) {}

  ngOnInit() {
    if (this.data.details.groupId) {
      this.data.customTitle = this.translate.instant('userListPanel.GroupTitle.lbl');
    }
  }
}
