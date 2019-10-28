import { Component, OnInit } from '@angular/core';
import { AbstractPageComponent } from 'src/app/shared/components/abstract-page/abstract-page.component';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.less']
})
export class SearchResultComponent extends AbstractPageComponent {
  defaultTitle = 'Search results page';
}
