import { Injectable } from '@angular/core';
import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { Subscription } from 'rxjs';
import { config } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class MqService {
  readonly desktopMediaQuery = config.desktopMediaQuery || '(min-width: 768px)';

  constructor(
    private bpObserver: BreakpointObserver
  ) { }

  mqFactory(mediaQuery: string)
  {
    return (left: () => void, right: () => void): Subscription => {
      return this.bpObserver
        .observe(mediaQuery)
        .subscribe((state: BreakpointState) => {
          return state.matches ? right() : left();
        });
    }
  }

  mobileDesktop(mobile: () => void, desktop: () => void)
  {
    return this.mqFactory(this.desktopMediaQuery)(mobile, desktop);
  }
}
