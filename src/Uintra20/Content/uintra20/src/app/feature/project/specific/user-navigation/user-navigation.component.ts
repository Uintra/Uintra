import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../../../auth/services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'user-navigation',
  templateUrl: './user-navigation.component.html',
  styleUrls: ['./user-navigation.component.less']
})
export class UserNavigationComponent implements OnInit {
  public inProgress: boolean;
  data: any;
  navigationExpanded: boolean;

  get isNavigationExpanded() {
    return this.navigationExpanded;
  }

  constructor(
    private authService: AuthService,
    private router: Router,
    private http: HttpClient) { }

  ngOnInit() {
    this.http.get('/ubaseline/api/IntranetNavigation/TopNavigation')
    .subscribe(res => {
      this.data = res;
    });
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

  toggleUserNavigation(e) {
    e.stopPropagation();
    this.navigationExpanded = !this.navigationExpanded;
  }

  closeUserNavigation() {
    this.navigationExpanded = false;
  }
}
