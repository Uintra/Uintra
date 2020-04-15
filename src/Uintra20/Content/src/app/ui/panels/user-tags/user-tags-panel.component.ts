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
  //TODO: Change data interface from any to IUserTagsPanel once you remove UFP from this panel and remove first three lines in ngOnInit()
  data: any;
  // data: IUserTagsPanel;
  tags: Array<IUserTag>;

  constructor(
    private translateService: TranslateService
  ) { }

  public ngOnInit(): void {
    if (this.data.get) {
      this.data = this.data.get();
    }
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(parsed.tags);
  }
}
