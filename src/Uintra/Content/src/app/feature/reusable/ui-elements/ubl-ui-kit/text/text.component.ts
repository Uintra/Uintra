import { Component, OnInit, Input, HostBinding, ViewChild, ElementRef, ChangeDetectionStrategy, ChangeDetectorRef, Renderer2 } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';
import { getContentClasses } from './helper/get-classes';

@Component({
  selector: 'section[ubl-text], p[ubl-text]',
  templateUrl: './text.component.html',
  styleUrls: ['./text.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TextComponent implements OnInit {
  @Input() ublText: string | SafeHtml;
  @Input() classToOverride: string;
  @Input() ublBeforeText: string | SafeHtml;
  @Input() ublAfterText: string | SafeHtml;

  @ViewChild("divContent", {static: false}) set contentClass(Content: ElementRef) 
  {
    if (!this.classToOverride) 
    {
      if (getContentClasses(Content)) {
        this.renderer.addClass(this.elementRef.nativeElement, `text-panel${getContentClasses(Content)}`);
        if (getContentClasses(Content) === '--html') { this.renderer.addClass(this.elementRef.nativeElement, 'rte'); }
      } else {
        this.removeEmptyDiv();
      }
    }
  }
  @ViewChild("divInnerHTML", {static: false}) set innerHTMLClass(Content: ElementRef) {
    if (!this.classToOverride && getContentClasses(Content)) 
    {
      this.renderer.addClass(this.elementRef.nativeElement, `text-panel${getContentClasses(Content)}`);
      if (getContentClasses(Content) === '--html') { this.renderer.addClass(this.elementRef.nativeElement, 'rte'); }
    }
  }
  @HostBinding('class') className: string;

  constructor(private elementRef: ElementRef, private renderer: Renderer2) { }

  isShow: boolean = true;

  removeEmptyDiv() 
  {
    this.isShow = false;
  }

  ngOnInit() 
  {
    let initialClassName = this.elementRef.nativeElement.className;

    if (this.classToOverride) 
    {
      this.className = `${initialClassName} text-panel--${this.classToOverride}`;
    } else 
    {
      this.className = initialClassName;
    }
  }
}
