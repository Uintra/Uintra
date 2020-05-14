import { Injectable } from '@angular/core';
import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { Subscription } from 'rxjs';

export const config = {
  minWidthTablet: 600,
  minWidthLaptop: 900,
  minWidthDesktop: 1200,
  minWidthDesktopHD: 1600,
}

@Injectable({
  providedIn: 'root'
})
export class MqService {
  readonly desktopMediaQuery = `(min-width: ${config.minWidthTablet}px)`;

  constructor(
    private bpObserver: BreakpointObserver
  ) { }

  isDesktopHD(width: number): boolean {
    return width >= config.minWidthDesktopHD;
  }

  isDesktop(width: number): boolean {
    return width >= config.minWidthDesktop;
  }

  isLaptop(width: number): boolean {
    return width >= config.minWidthLaptop;
  }

  isTablet(width: number): boolean {
    return width >= config.minWidthTablet;
  }

  mdDown (width: number): boolean {
    return width < config.minWidthLaptop;
  }

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
