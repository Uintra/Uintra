import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IProfilePage } from 'src/app/shared/interfaces/pages/profile/profile-page.interface';
import { Indexer } from '../../../../shared/abstractions/indexer';

@Component({
  selector: 'profile-page',
  templateUrl: './profile-page.html',
  styleUrls: ['./profile-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfilePage extends Indexer<number> implements OnInit {

  public data: IProfilePage;

  constructor(
    private router: Router) {
    super();
  }

  ngOnInit(): void {
    //TODO: move this logic to guard
    if (this.data.requiresRedirect) {
      this.router.navigate([this.data.errorLink.originalUrl]);
    }
  }
}
