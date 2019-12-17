import { Component, OnInit } from '@angular/core';
import { LogoutService } from 'src/app/feature/logout/logout.service';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'user-navigation',
  templateUrl: './user-navigation.component.html',
  styleUrls: ['./user-navigation.component.less']
})
export class UserNavigationComponent implements OnInit {
  public inProgress: boolean;

  constructor(
    private logoutService: LogoutService,
    private router: Router) { }

  ngOnInit() {
  }

  public logout() {
    this.inProgress = true;
    this.logoutService.logout()
      .pipe(finalize(() => setTimeout(() => this.inProgress = false, 400)))
      .subscribe(
        (next) => this.router.navigate(['/login']),
        (error) => { console.log(error); },
        () => { }
      );
  }

}
