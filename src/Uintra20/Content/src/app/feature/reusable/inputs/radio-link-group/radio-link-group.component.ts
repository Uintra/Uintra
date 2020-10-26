import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-radio-link-group',
  templateUrl: './radio-link-group.component.html',
  styleUrls: ['./radio-link-group.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadioLinkGroupComponent),
      multi: true
    }
  ]
})
export class RadioLinkGroupComponent implements ControlValueAccessor, OnInit {

  @Input() 
  public links: Array<any> = [];
  public selectedLink: number;
  public TOTAL_LINKS_COUNT = 4;

  constructor() { }

  public ngOnInit(): void {
    if (this.links.length < this.TOTAL_LINKS_COUNT) {
      this.links = this.links.filter(l => l.data.title !== 'All');
    }
  }

  public onRadioChange(): void {
    this.propagateChange(this.selectedLink);
  }

  public onTouched(): any { }

  public onChange(): any { }

  public propagateChange: any = () => { };

  public writeValue(value) { this.selectedLink = value; }

  public registerOnChange(fn) { this.propagateChange = fn; }

  public registerOnTouched(fn) { this.onTouched = fn; }
}
