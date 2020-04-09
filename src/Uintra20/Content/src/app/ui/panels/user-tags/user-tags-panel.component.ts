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
    debugger
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(parsed.tags);

    // this.tags = [
    //   {
    //     "id": '1',
    //     "text": "First"
    //   },
    //   {
    //     "id": '2',
    //     "text": "Second"
    //   },
    //   {
    //     "id": '3',
    //     "text": "Third"
    //   }
    // ];
  }
}