import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RTEStripHTMLService {

  constructor() { }

  isEmpty(html: string): boolean {
    return this.isImg(html) ? false : this.isNullOrWhitespace(this.stripHtml(html))
  }

  isImg(html: string): boolean {
    return !html ? false : html.indexOf('<img') !== -1;
  }

  stripHtml(html: string): string {
    if (!html) {
      return '';
    }

    const stripped = html.replace(/<[^>]*>?/gm, '');

    return stripped;
  }

  isNullOrWhitespace(value: string): boolean {
    if (!value) {
      return true;
    }

    return value.replace(/\s/g, '').length < 1;
  }
}
