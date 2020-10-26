import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';

@Component({
  selector: 'app-iframe',
  templateUrl: './iframe.component.html',
  styleUrls: ['./iframe.component.less']
})
export class IframeComponent implements OnInit {
  @Input() src: string;
  @Input() params: string;

  safeSrc: SafeResourceUrl;

  constructor(private sanitizer: DomSanitizer){
  }

  ngOnInit() {
    if (!this.src) return;

    this.params = this.params || "?controls=0&mute=1&muted=1&autoplay=1&showinfo=0&loop=1&autopause=0";
    this.src = this.src + this.params;
    this.safeSrc = this.sanitizer.bypassSecurityTrustResourceUrl(this.src);
  }
}
