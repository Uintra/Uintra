import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
  name: 'trustHtml'
})
export class TrustHtmlPipe implements PipeTransform {
  constructor(public sanitizer: DomSanitizer) {}

  transform(str: string): SafeHtml 
  {
    return this.sanitizer.bypassSecurityTrustHtml(str);
  }
}
