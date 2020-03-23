import { Component, Input, forwardRef } from "@angular/core";
import { ITagData } from "./tag-multiselect.interface";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
  selector: "app-tag-multiselect",
  templateUrl: "./tag-multiselect.component.html",
  styleUrls: ["./tag-multiselect.component.less"],
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
  @Input() placeholder: string;

  selectedList: Array<ITagData> = [];
  isDwopdownShowed = false;
  isAddedTag = false;

  constructor() { }

  public onToggleDropdown(): void {
    this.isDwopdownShowed = !this.isDwopdownShowed;
  }
  public onShowDropdown(): void {
    this.isDwopdownShowed = true;
  }
  public onHideDropdown(): void {
    this.isDwopdownShowed = false;
  }

  public onAddTag(tag): void {
    if (this.selectedList.filter(t => t.id === tag.id).length) { return; }

    this.isAddedTag = true;
    this.selectedList = [...this.selectedList, tag];
    this.onHideDropdown();
    this.writeValue(this.selectedList);
  }

  public onRemoveTag(tag, e): void {
    e.event.stopPropagation();
    this.selectedList = this.selectedList.filter(
      curTag => curTag.id !== tag.id
    );
    if (this.selectedList.length === 0) {
      this.isAddedTag = false;
    }
    this.writeValue(this.selectedList);
  }

  public onClearSelectedTags(): void {
    this.selectedList = [];
    this.isAddedTag = false;
    this.onHideDropdown();
  }

  selectedCheck(tag: ITagData): boolean {
    if (Array.isArray(this.selectedList)) {
      const selectedArray = this.selectedList.filter(
        listItem => listItem.id === tag.id
      );
      return !!selectedArray.length;
    }

    return false;
  }

  propagateChange: any = () => {
  }

  writeValue(value) {
    this.selectedList = value || [];
    this.onChange(this.selectedList);
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
