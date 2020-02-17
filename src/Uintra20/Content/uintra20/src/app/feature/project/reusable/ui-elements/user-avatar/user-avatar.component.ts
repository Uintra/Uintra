import { Component, OnInit, Input } from '@angular/core';
import { IUserAvatar } from './user-avatar-interface';


@Component({
  selector: 'app-user-avatar',
  templateUrl: './user-avatar.component.html',
  styleUrls: ['./user-avatar.component.less']
})
export class UserAvatarComponent implements OnInit {
  @Input() public photo: string;
  @Input() public name: string;
  @Input('big') public big: boolean;
  @Input() public routerLink: string;
  @Input() public queryParams: object;

  firstChar: string;

  public ngOnInit(): void {
    this.convertToBoolean();
    this.validateImagePreset();
    this.initFirstlLetter();
  }
  private validateImagePreset(): void {
    if (this.photo && this.photo.startsWith('?')) {
      this.photo = '';
    }
  }

  private initFirstlLetter(): void {
    if (this.name && typeof this.name === 'string') {
      this.firstChar = this.name.charAt(0);
    }
  }

  private convertToBoolean(): void {
    this.big = this.big !== undefined;
  }
}
