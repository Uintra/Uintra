import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { HasDataChangedService } from './has-data-changed.service';

export interface DeactivationGuarded {
  canDeactivate(): Observable<boolean> | Promise<boolean> | boolean;
}

@Injectable()
export class CanDeactivateGuard implements CanDeactivate<DeactivationGuarded> {
  constructor(private hasDataChangedService: HasDataChangedService) {}

  canDeactivate(component: DeactivationGuarded):  Observable<boolean> | Promise<boolean> | boolean {
    return component.canDeactivate ? component.canDeactivate() : true;
  }

  canDeacrivateConfirm() {
    if(confirm('Are you sure? Changes you made may not be saved.')) {
      this.hasDataChangedService.reset();
      return true;
    }

    return false;
  }
}
