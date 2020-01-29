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

    // this.onParse();

    this.profile = {
      member: {
        photo: 'https://uintra20.local.compent.dk:4200/media/n04hu24e/black15.jpg',
        firstName: 'Nastya',
        lastName: 'Vasylyk',
        email: 'ast@compent.net',
        phone: '+48693604340',
        department: 'IT-depart',
      },
      title: 'Profile details'
    };
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
