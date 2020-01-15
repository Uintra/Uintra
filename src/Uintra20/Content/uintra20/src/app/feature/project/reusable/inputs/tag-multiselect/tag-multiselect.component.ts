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
  isAddedTag: boolean = false;

  constructor() { }

  onToggleDropdown() {
    this.isDwopdownShowed = !this.isDwopdownShowed;
  }
  onShowDropdown() {
    this.isDwopdownShowed = true;
  }
  onHideDropdown() {
    this.isDwopdownShowed = false;
  }

  onAddTag(tag) {
    if (this.selectedList.includes(tag)) return;
    this.isAddedTag = true;
    this.selectedList.push(tag);
    this.onHideDropdown();
  }

  onRemoveTag(tag, e) {
    e.event.stopPropagation();
    this.selectedList = this.selectedList.filter(curTag => curTag.id !== tag.id);
    if (this.selectedList.length == 0) {
        this.isAddedTag = false;
    }
  }

  onClearSelectedTags() {
    this.selectedList = [];
    this.isAddedTag = false;
    this.onHideDropdown();
  }

  onTouched(): any { }
  onChange(): any {}
  propagateChange: any = () => { };
  writeValue(value) { this.selectedList = value; }
  registerOnChange(fn) { this.propagateChange = fn; }
  registerOnTouched(fn) { this.onTouched = fn; }
}
