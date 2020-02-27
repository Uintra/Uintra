import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IMobileUserNavigation } from '../../left-navigation.interface';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-user-nav-mobile',
  templateUrl: './user-nav-mobile.component.html',
  styleUrls: ['./user-nav-mobile.component.less']
})
export class UserNavMobileComponent implements OnInit {
  @Output() closeLeftNavigation = new EventEmitter();
  data: IMobileUserNavigation;
  inProgress: boolean;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.http.get('/ubaseline/api/IntranetNavigation/TopNavigation')
    .subscribe((res: any) => {
      this.data = res;
    });
  }

  redirect(url, type) {
    this.inProgress = true;

    if (type == 1) {
      this.http.post(url.originalUrl, null).pipe(
        finalize(() => this.inProgress = false)
      ).subscribe(
        (next) => {
          window.open(window.location.origin + "/umbraco", "_blank");
        },
        (error) => {
          if (error.status === 400 || error.status === 403) {
            console.error(error.message);
          }
        },
      );
    }

    if (type == 4) {
      this.http.post(url.originalUrl, null).pipe(
        finalize(() => this.inProgress = false)
      ).subscribe(
        (next) => {
          window.location.href = '/login';
        },
        (error) => {
          if (error.status === 400) {
            console.error(error.message);
          }
        },
      )
    }
  }
}

