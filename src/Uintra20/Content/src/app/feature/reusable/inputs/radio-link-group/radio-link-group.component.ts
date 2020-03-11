import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

// interface IRadioLink {
//   type: string;
// }

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

  // @Input() links: Array<IRadioLink> = [];
  @Input() links: Array<any> = [];
  selectedLink: number;
  TOTAL_LINKS_COUNT = 4;
  constructor() { }

  public ngOnInit(): void {
      if (this.links.length < this.TOTAL_LINKS_COUNT) {
          this.links = this.links.filter(l => l.data.title !== "All");
      }
  }

  onRadioChange() {
    this.propagateChange(this.selectedLink);
  }

  onTouched(): any { }
  onChange(): any {}
  propagateChange: any = () => { };
  writeValue(value) { this.selectedLink = value; }
  registerOnChange(fn) { this.propagateChange = fn; }
  registerOnTouched(fn) { this.onTouched = fn; }
}
