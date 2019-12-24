import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-user-avatar',
  templateUrl: "./user-avatar.component.html",
  styles: [`
    .avatar {
      border-radius: 50%;
      width: 30px;
      height: 30px;
      background: #eee;
    }
    .avatar.big {
      width: 60px;
      height: 60px;
    }
  `]
})
export class UserAvatarComponent implements OnInit {
  @Input() data: string;
  @Input('big') big: boolean;

  // TODO: use default avatar from server
  readonly defaultAvatar: string = '';

  constructor() { }

  ngOnInit() {
    this.big = this.big !== undefined;
    this.data = this.data || this.defaultAvatar;
  }
}
