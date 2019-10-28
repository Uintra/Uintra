import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { ISearchItem } from '../service/search.service';
@Component({
  selector: 'app-result-list',
  templateUrl: './result-list.component.html',
  styleUrls: ['./result-list.component.less']
})

export class ResultListComponent implements OnInit, OnChanges {
  @Input() items: ISearchItem[];
  @Input() currentPage: number;
  @Input() total: number;
  @Input() itemsPerPage: number;

  @Output() pageChange: EventEmitter<number> = new EventEmitter()

  paginateOptions: any;
  resultItems: ISearchItem[];

  constructor() { }

  ngOnInit() { }

  ngOnChanges() {
    this.paginateOptions = {
      itemsPerPage: this.itemsPerPage,
      currentPage: this.currentPage + 1,
      totalItems: this.total
    };
  }

  onPageChange(e) {
    const newPage = e - 1;
    this.pageChange.emit(newPage);
  }

  getItemNumber(i) {
    return (this.itemsPerPage * this.currentPage) + i + 1;
  }
}
