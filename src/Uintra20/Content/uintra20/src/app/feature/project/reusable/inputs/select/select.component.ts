import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';

interface ISelectItem {
  id: string;
  text: string;
  selected?: boolean;
  disabled?: boolean;
}

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.less']
})
export class SelectComponent implements OnInit {
  @Input() items: ISelectItem[] = [];
  @Input() defaultItem: ISelectItem;

  selectedItem: ISelectItem;

  constructor() { }

  ngOnInit() {
    if (this.defaultItem) {
      this.selectedItem = this.defaultItem;
    }
  }
}
