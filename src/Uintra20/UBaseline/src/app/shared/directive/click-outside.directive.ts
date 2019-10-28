import { Directive, ElementRef, HostListener, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Directive({
  selector: '[clickOutside]'
})
export class ClickOutsideDirective {
  @Output() clickOutside = new EventEmitter();

  @HostListener('document:click', ['$event']) handleBodyClick(ev: MouseEvent)
  {
    let isClickOutside = true;
    let current = <HTMLElement>ev.target;

    while(current) {
      if (current === this.element.nativeElement)
      {
        isClickOutside = false;
        break;
      }
      current = current.parentElement
    }
     
    if (isClickOutside)
    {
      this.clickOutside.emit(null);
    }
  }

  constructor(protected element: ElementRef) {}
}
