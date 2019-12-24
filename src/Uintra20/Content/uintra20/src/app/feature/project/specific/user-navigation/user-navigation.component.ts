import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/ui/pages/login/services/auth.service';

@Component({
  selector: 'user-navigation',
  templateUrl: './user-navigation.component.html',
  styleUrls: ['./user-navigation.component.less']
})
export class UserNavigationComponent implements OnInit {
  public inProgress: boolean;

  constructor(
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
  }

  public logout() {
    this.inProgress = true;
    this.authService.logout()
      .pipe(finalize(() => this.inProgress = false))
      .subscribe(
        (next) => this.router.navigate(['/login']),
        (error) => { },
        () => { }
      );
  }

}
