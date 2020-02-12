import { Injectable } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class HasDataChangedService {
  hasDataChanged: boolean = false;

  constructor(private router: Router) {
    this.router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        this.hasDataChanged = false;
      }
    })
  }

  onDataChanged() {
    this.hasDataChanged = true;
  }
}
