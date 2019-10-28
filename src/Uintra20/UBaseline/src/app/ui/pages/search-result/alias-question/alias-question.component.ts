import { Component, OnInit, Input } from '@angular/core';
import { ISearchItem } from '../service/search.service';

@Component({
  selector: 'app-alias-question',
  templateUrl: './alias-question.component.html',
  styleUrls: ['./alias-question.component.less']
})
export class AliasQuestionComponent implements OnInit {
  @Input() items: ISearchItem[];

  constructor() { }

  ngOnInit() {
  }

}
