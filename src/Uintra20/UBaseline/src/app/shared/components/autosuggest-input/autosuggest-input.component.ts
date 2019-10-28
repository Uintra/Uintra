import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-autosuggest-input',
  templateUrl: './autosuggest-input.component.html',
  styleUrls: ['./autosuggest-input.component.less']
})
export class AutosuggestInputComponent implements OnInit {
  @Input() searchQuery: string;
  @Input() placeholder: string;
  @Output() onQuery = new EventEmitter<string>();
  @Output() onBlur = new EventEmitter();
  @Output() onClear = new EventEmitter();
  @Output() toResults = new EventEmitter();

  _query = new Subject<string>();
  inputValue:string = '';

  constructor() {
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    )
      .subscribe(value => {
        this.onQuery.emit(value);
      })
  }

  ngOnInit() {
    if(this.searchQuery) {
      this.inputValue = this.searchQuery;
    }
  }

  search(val) {
    this.inputValue = val;
    this._query.next(val)
  }

  toResultPage() {
    this.toResults.emit();
  }
}
