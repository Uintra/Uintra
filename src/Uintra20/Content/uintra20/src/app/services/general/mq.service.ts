import { Injectable } from '@angular/core';

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

  constructor() { }

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
}
