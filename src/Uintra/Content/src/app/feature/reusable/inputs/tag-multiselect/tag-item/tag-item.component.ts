import { Component, Output, EventEmitter, Input } from '@angular/core';
import { ITagData } from '../tag-multiselect.interface';

@Component({
  selector: 'app-tag-item',
  templateUrl: './tag-item.component.html',
  styleUrls: ['./tag-item.component.less']
})
export class TagItemComponent {
  @Input() tag: ITagData;
  @Output() removeTag = new EventEmitter();

  constructor() { }

  onRemoveTag(event) {
    this.removeTag.emit({ tag: this.tag, event });
  }
}
