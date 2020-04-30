import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { IUserTagPanel } from 'src/app/shared/interfaces/panels/user-tag/user-tag-panel.interface';

@Component({
  selector: 'user-tags-panel',
  templateUrl: './user-tags-panel.html',
  styleUrls: ['./user-tags-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserTagsPanel implements OnInit {

  public data: IUserTagPanel;

  constructor() { }

  public ngOnInit(): void { }

  public index = (index): number => index;
}
