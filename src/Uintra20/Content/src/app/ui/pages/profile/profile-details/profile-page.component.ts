import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { AddButtonService } from '../../../main-layout/left-navigation/components/my-links/add-button.service';
import { IProfilePage } from 'src/app/shared/interfaces/pages/profile/profile-page.interface';


@Component({
  selector: 'profile-page',
  templateUrl: './profile-page.html',
  styleUrls: ['./profile-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfilePage implements OnInit {
  public data: any;
  public profile: IProfilePage;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private router: Router) {
  }

  public ngOnInit(): void {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.addButtonService.setPageId(data.id.get());
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
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
        tags: Object.values(parsed.tags),
      },
      title: parsed.name,
      link: parsed.editProfileLink
    };
  }
}
