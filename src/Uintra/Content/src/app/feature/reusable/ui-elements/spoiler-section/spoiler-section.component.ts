import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

@Component({
  selector: 'app-spoiler-section',
  templateUrl: './spoiler-section.component.html',
  styleUrls: ['./spoiler-section.component.less'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => SpoilerSectionComponent), multi: true}
  ]
})
export class SpoilerSectionComponent  implements ControlValueAccessor {
  @Input() title: string;
  @Input() isSpoilerShowed: boolean = false;

  constructor() { }

  onToggleSpoiler() {
    this.isSpoilerShowed = !this.isSpoilerShowed;
    this.writeValue(this.isSpoilerShowed);
  }

  propagateChange: any = () => {
  }

  writeValue(value) {
    this.isSpoilerShowed = value;
    this.onChange(value);
  }

  onChange: any = () => {
  }

  onTouched: any = () => {
  }

  registerOnChange(fn) {
    this.onChange = fn;
  }

  registerOnTouched(fn) {
    this.onTouched = fn;
  }
}
