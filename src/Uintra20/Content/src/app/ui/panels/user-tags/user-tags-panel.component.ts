import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { IUserTagsPanel } from './user-tags-panel.interface';
import { TranslateService } from '@ngx-translate/core';
import { IUserTag } from 'src/app/feature/specific/activity/activity.interfaces';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'user-tags-panel',
  templateUrl: './user-tags-panel.html',
  styleUrls: ['./user-tags-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserTagsPanel implements OnInit {

  data: IUserTagsPanel;
  tags: Array<IUserTag>;

  constructor(
    private translateService: TranslateService
  ) { }

  public ngOnInit(): void {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(parsed.tags);
  }
}
