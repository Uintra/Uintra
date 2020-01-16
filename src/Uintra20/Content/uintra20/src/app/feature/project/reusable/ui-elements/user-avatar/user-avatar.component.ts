import { Component, OnInit, Input } from '@angular/core';
import { IUserAvatar } from './user-avatar-interface';


@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.less']
})
export class UserAvatarComponent implements OnInit {
  @Input() photo: string;
  @Input() name: string;
  @Input('big') big: boolean;

  firstChar: string;

  convertToBoolean(): void {
    this.big = this.big !== undefined;
  }

  ngOnInit() {
    this.convertToBoolean();

    if (this.name && typeof this.name === 'string') {
      this.firstChar = this.name.charAt(0);
    }
  }
}
