import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'ubl-autosuggest-input',
  templateUrl: './autosuggest-input.component.html',
  styleUrls: ['./autosuggest-input.component.less']
})
export class AutosuggestInputComponent {
  @Input() placeholder: string;
  @Input() iconsPosition: 'before' | 'after';

  @Output() onQuery = new EventEmitter<string>();
  @Output() onBlur = new EventEmitter();
  @Output() onClear = new EventEmitter();

  _query = new Subject<string>();

  constructor() 
  {
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.onQuery.emit(value)
    })
  }

  search(val: string) {
    this._query.next(val)
  }

}
