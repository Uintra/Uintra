import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'search-page',
  templateUrl: './search-page.html',
  styleUrls: ['./search-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class SearchPage {
  data: any;
  parsedData: any;
  inputValue: string = "";
  filters: any[] = [];
  availableTags: any[] = [];
  resultsList: any[] = [];
  isOnlyPinned: boolean = false;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(this.data);
    });
  }

  ngOnInit() {
    this.availableTags = Object.values(this.parsedData.filterItems).map((item: any) => ({id: item.id, text: item.name}));
    this.resultsList = this.parsedData.results || [];
  }

  handlePinnedCbx(val) {
    debugger
  }
}
