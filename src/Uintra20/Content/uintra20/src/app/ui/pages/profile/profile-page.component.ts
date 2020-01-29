import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { IProfilePage } from './profile-page.interface';

@Component({
  selector: 'profile-page',
  templateUrl: './profile-page.html',
  styleUrls: ['./profile-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfilePage implements OnInit {
  private data: any;
  public profile: IProfilePage;

  constructor(private route: ActivatedRoute) {
  }

  public ngOnInit(): void {
    this.route.data.subscribe(data => this.data = data);
    this.onParse();
  }

  private onParse(): void {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.profile = {
      member: {
        photo: parsed.profile.photo,
        firstName: parsed.profile.firstName,
        lastName: parsed.profile.lastName,
        email: parsed.profile.email,
        phone: parsed.profile.phone,
        department: parsed.profile.department,
      },
      title: parsed.name
    };
  }
}
