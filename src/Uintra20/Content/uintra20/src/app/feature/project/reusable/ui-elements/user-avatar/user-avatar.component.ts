import { Component, OnInit, Input } from '@angular/core';


@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.less']
})
export class UserAvatarComponent implements OnInit {
  @Input() data: string;
  @Input() firstChar: string;
  @Input('big') big: boolean;

  ngOnInit() {
    this.big = this.big !== undefined;
    this.data = this.data ;
  }
}
