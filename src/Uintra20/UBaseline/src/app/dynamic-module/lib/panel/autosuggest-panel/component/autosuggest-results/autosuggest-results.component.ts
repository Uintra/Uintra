import { Component, Input, Output } from '@angular/core';
import { IAutosuggestResponseItem } from '../../service/autosuggest.service';
import { EventEmitter } from 'events';

@Component({
  selector: 'ubl-autosuggest-results',
  templateUrl: './autosuggest-results.component.html',
  styleUrls: ['./autosuggest-results.component.less']
})
export class AutosuggestResultsComponent {
  @Input() results:IAutosuggestResponseItem[];
  @Output() onSelect = new EventEmitter();
}
