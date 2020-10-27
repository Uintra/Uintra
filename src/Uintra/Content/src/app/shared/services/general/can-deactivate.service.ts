import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { HasDataChangedService } from './has-data-changed.service';
import { TranslateService } from '@ngx-translate/core';

export interface DeactivationGuarded {
  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean;
}

@Injectable()
export class CanDeactivateGuard implements CanDeactivate<DeactivationGuarded> {
  constructor(
    private hasDataChangedService: HasDataChangedService,
    private translateService: TranslateService) { }

  canDeactivate(component: DeactivationGuarded): Observable<boolean> | Promise<boolean> | boolean {

    if (component) {
      return component.canDeactivate
        ? component.canDeactivate()
        : true;
    }

    return true;
  }

  canDeacrivateConfirm() {
    if (confirm(this.translateService.instant('pageLeaveAlert'))) {
      this.hasDataChangedService.reset();
      return true;
    }

    return false;
  }
}
