import { Component, Input, forwardRef } from '@angular/core';
import { ITagData } from './tag-multiselect.interface';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';


@Component({
  selector: 'app-tag-multiselect',
  templateUrl: './tag-multiselect.component.html',
  styleUrls: ['./tag-multiselect.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TagMultiselectComponent),
      multi: true
    }
  ]
})
export class TagMultiselectComponent implements ControlValueAccessor {
  @Input() availableTags: Array<ITagData>;

  selectedList: Array<ITagData> = [];
  isDwopdownShowed: boolean = false;

  constructor() { }

  onShowDropdown() {
    this.isDwopdownShowed = true;
  }
  onHideDropdown() {
    this.isDwopdownShowed = false;
  }

  onAddTag(tag) {
    if(this.selectedList.includes(tag)) return;
    this.selectedList.push(tag);
    this.onHideDropdown();
  }

  onRemoveTag(tag, e) {
    e.event.stopPropagation();
    this.selectedList = this.selectedList.filter(curTag => curTag.id !== tag.id);
  }

  onClearSelectedTags() {
    this.selectedList = [];
    this.onHideDropdown();
  }

  onTouched(): any { }
  onChange(): any {}
  propagateChange: any = () => { };
  writeValue(value) { this.selectedList = value; }
  registerOnChange(fn) { this.propagateChange = fn; }
  registerOnTouched(fn) { this.onTouched = fn; }
}
