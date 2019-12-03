import { Component, OnInit } from '@angular/core';

export interface ITagData {
  id: string;
  text: string;
}

@Component({
  selector: 'app-tag-multiselect',
  templateUrl: './tag-multiselect.component.html',
  styleUrls: ['./tag-multiselect.component.less']
})
export class TagMultiselectComponent implements OnInit {
  isDwopdownShowed: boolean = false;
  selectedList: Array<ITagData> = [];
  dropdownList: Array<ITagData> = [];

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

  onRemoveTag(tag) {
    this.selectedList = this.selectedList.filter(curTag => curTag.id !== tag.id);
  }

  onClearSelectedTags() {
    this.selectedList = [];
    this.onHideDropdown();
  }

  ngOnInit() {
    // TODO: remove it
    this.dropdownList = [
      { id: '1', text: 'test' },
      { id: '2', text: 'testtest' },
      { id: '3', text: 'test2' },
      { id: '4', text: 'testtest2' },
      { id: '5', text: 'test3' },
      { id: '6', text: 'testtest3' }
    ];
  }
}
