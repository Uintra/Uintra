import { Component, ViewEncapsulation, OnInit } from '@angular/core';

@Component({
  selector: 'user-list-panel',
  templateUrl: './user-list-panel.html',
  styleUrls: ['./user-list-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserListPanel implements OnInit {

  public data: any;

  constructor() { }

  public ngOnInit(): void { }
}
