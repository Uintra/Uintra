import { Component, OnInit } from '@angular/core';
import { IUserTagPanel } from 'src/app/shared/interfaces/panels/user-tag/user-tag-panel.interface';
import { Indexer } from '../../../shared/abstractions/indexer';

@Component({
  selector: 'user-tags-panel',
  templateUrl: './user-tags-panel.html',
  styleUrls: ['./user-tags-panel.less']
})
export class UserTagsPanel extends Indexer<number> implements OnInit {

  public data: IUserTagPanel;

  constructor() {
    super();
  }

  public ngOnInit(): void { }
}
