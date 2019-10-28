import { Component, Input, Output } from '@angular/core';
import { IAutosuggestResponseItem } from '../../service/autosuggest.service';
import { EventEmitter } from 'events';

@Component({
  selector: 'app-autosuggest-results',
  templateUrl: './autosuggest-results.component.html',
  styleUrls: ['./autosuggest-results.component.less']
})
export class AutosuggestResultsComponent {
  @Input() results:IAutosuggestResponseItem[];
  @Input() isResponseCompleted:boolean;
  @Input() query:string;
  @Output() onSelect = new EventEmitter();

  MAX_LINES: number = 5;
  MIN_LENGTH: number = 3;
}
