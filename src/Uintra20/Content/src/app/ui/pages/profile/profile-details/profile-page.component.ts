import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AddButtonService } from '../../../main-layout/left-navigation/components/my-links/add-button.service';
import { IProfilePage } from 'src/app/shared/interfaces/pages/profile/profile-page.interface';

@Component({
  selector: 'profile-page',
  templateUrl: './profile-page.html',
  styleUrls: ['./profile-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfilePage implements OnInit {

  public data: IProfilePage;

  constructor(
    private activatedRoute: ActivatedRoute,
    private addButtonService: AddButtonService,
    private router: Router) {
  }

  public ngOnInit(): void {
    this.activatedRoute.data.subscribe((data: IProfilePage) => {
      if (!data.requiresRedirect) {
        this.data = data;
        this.addButtonService.setPageId(data.id.toString());
      } else {
        this.router.navigate([data.errorLink.originalUrl]);
      }
    });
  }

  public trackIndex = (index, item): string => item.id;
}
