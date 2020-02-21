import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

export enum IconType {
  'icon-umbraco-logo' = 1,
  'icon-user-profile',
  'icon-uintra',
  'icon-logout',
}

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
    private router: Router,
    private http: HttpClient) { }

  ngOnInit() {
    this.http.get('/ubaseline/api/IntranetNavigation/TopNavigation')
    .subscribe(res => {
      this.data = res;
    });
  }

  toggleUserNavigation(e) {
    e.stopPropagation();
    this.navigationExpanded = !this.navigationExpanded;
  }

  closeUserNavigation() {
    this.navigationExpanded = false;
  }

  getClass(type) {
    return IconType[type];
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
          if (error.status === 403) {
            console.error(error.message);
          }
        },
      );
    }

    if (type == 4) {
      this.http.post(url.originalUrl, null).pipe(
        finalize(() => this.inProgress = false)
      ).subscribe(
        (next) => { window.location.href = '/login'; }
      )
    }
  }
}
