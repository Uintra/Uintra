import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

export interface ICookieComponentData {
  cookieTitle: string;
  cookieDescription: string;
  btnLabel: string;
}

@Component({
  selector: 'app-cookie',
  templateUrl: './cookie.component.html',
  styleUrls: ['./cookie.component.less']
})
export class CookieComponent implements OnInit {
  @Input() data: ICookieComponentData;

  isConsented: boolean = false;

  COOKIE_CONSENT: string = 'cookieConsent';
  COOKIE_CONSENT_EXPIRE_DAYS: number = 30;

  constructor() {
    this.isConsented = this.getCookie(this.COOKIE_CONSENT) === '1';
  }

  ngOnInit() {
  }

  handleConsent(isConsent: boolean) {
    if (!isConsent) {
      this.setCookie(this.COOKIE_CONSENT, '1', this.COOKIE_CONSENT_EXPIRE_DAYS);
      this.isConsented = true;
    }
  }

  private getCookie(name: string) {
      let ca: Array<string> = document.cookie.split(';');
      let caLen: number = ca.length;
      let cookieName = `${name}=`;
      let c: string;

      for (let i: number = 0; i < caLen; i += 1) {
          c = ca[i].replace(/^\s+/g, '');
          if (c.indexOf(cookieName) == 0) {
              return c.substring(cookieName.length, c.length);
          }
      }
      return '';
  }

  private setCookie(name: string, value: string, expireDays: number, path: string = '') {
      let d:Date = new Date();
      d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
      let expires:string = `expires=${d.toUTCString()}`;
      let cpath:string = path ? `; path=${path}` : '';
      document.cookie = `${name}=${value}; ${expires}${cpath}`;
  }
}
