import { Component, OnInit, Input, ViewChild, ElementRef, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

export interface ISelectItem {
  id: string;
  text: string;
  selected?: boolean;
  disabled?: boolean;
}

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectComponent),
      multi: true
    }
  ]
})
export class SelectComponent implements OnInit {
  @Input() items: ISelectItem[] = [];
  @Input() defaultItem: ISelectItem;

  selectedItem: ISelectItem;

  constructor() { }

  ngOnInit() {
    if (this.defaultItem) {
      this.selectedItem = this.defaultItem;
    } else {
      if (this.items.length) {
        this.selectedItem = this.items[0];
      }
    }
  }

  onSelectChange(e) {
    this.writeValue(e);
  }

  propagateChange: any = () => {
  }

  writeValue(value) {
    this.selectedItem = value;
    this.onChange(this.selectedItem);
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
