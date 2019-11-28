import { Component, OnInit, Input } from '@angular/core';
import { IPictureData } from '../../picture/picture.component';

@Component({
  selector: 'app-user-avatar',
  // TODO: Use app-picture instead of img
  template: `<img class="avatar" src="{{data}}" [ngClass]="{'big': big}"/>`,
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
  // TODO: use IPictureData instead of string
  @Input() data: string;
  @Input('big') big: boolean = false;

  // TODO: use default avatar from server
  readonly defaultAvatar: string = 'http://s3.amazonaws.com/37assets/svn/765-default-avatar.png';

  constructor() { }

  ngOnInit() {
    this.big = this.big !== undefined;
    this.data = this.data || this.defaultAvatar;
  }
}
