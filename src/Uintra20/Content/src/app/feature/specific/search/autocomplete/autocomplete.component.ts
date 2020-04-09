import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { SearchService } from '../search.service';
import { Router, NavigationStart } from '@angular/router';
import { IAutocompleteItem, IMapedAutocompleteItem } from '../search.interface';

@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.less']
})
export class AutocompleteComponent implements OnInit {

  _query = new Subject<string>();
  inputValue: string = '';
  autocompleteList: IMapedAutocompleteItem[] = [];
  hasResults: boolean = true;
  isFocused: boolean;
  inputValueToRestore: string;

  constructor(
    private searchService: SearchService,
    private router: Router,
  ) {
    this._query.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((value: string) => {
      if (value.trim() && value.trim().length > 1) {
        this.searchService.autocomplete(value).subscribe((res: IAutocompleteItem[]) => {
          this.autocompleteList = res.map(suggestion => ({
            ...suggestion,
            isActive: false
          }));
          this.hasResults = res.length > 0;
        })
      } else {
        this.autocompleteList = [];
      }
    })
  }

  ngOnInit() {
    this.router.events.subscribe(e => {
      if (e instanceof NavigationStart) {
        this.closeAutocomplete();
        this.clearInput();
      }
    })
  }

  onKeyClick(keyCode: number) {
    switch (keyCode) {
      case 13:
        this.goToSearchPage();
        break;
      case 38:
        this.prevSuggestion();
        break;
      case 40:
        this.nextSuggestion();
        break;
    }
  }

  onQueryChange(val: string) {
    this.inputValue = val;
    this.inputValueToRestore = val;
    const trimedVal = val.replace(/\s+/g, ' ')
    this._query.next(trimedVal);
  }

  prevSuggestion() {
    if (this.autocompleteList.length > 0) {
      const currentSuggestionIndex = this.autocompleteList.findIndex(suggestion => suggestion.isActive == true);

      if (currentSuggestionIndex == 0) {
        this.autocompleteList[0].isActive = false;
        this.inputValue = this.inputValueToRestore;
      }
      if (currentSuggestionIndex >= 0) {
        const prevSuggestion = this.autocompleteList[currentSuggestionIndex - 1];
        this.autocompleteList[currentSuggestionIndex].isActive = false;
        prevSuggestion.isActive = true;
        this.inputValue = prevSuggestion.item.title;
      }
    }
  }

  nextSuggestion() {
    if (this.autocompleteList.length > 0) {
      const currentSuggestionIndex = this.autocompleteList.findIndex(suggestion => suggestion.isActive == true);

      if (currentSuggestionIndex == -1) {
        this.autocompleteList[0].isActive = true;
        this.inputValue = this.autocompleteList[0].item.title;
      }
      if (currentSuggestionIndex >= 0 && currentSuggestionIndex < this.autocompleteList.length - 1) {
        const nextSuggestion = this.autocompleteList[currentSuggestionIndex + 1];
        this.autocompleteList[currentSuggestionIndex].isActive = false;
        nextSuggestion.isActive = true;
        this.inputValue = nextSuggestion.item.title;
      }
    }
  }

  goToSearchPage() {
    if (this.inputValue.length > 1) {
      const currentSuggestionIndex = this.autocompleteList.length
        ? this.autocompleteList.findIndex(suggestion => suggestion.isActive == true)
        : -1;

      if (currentSuggestionIndex !== -1) {
        this.router.navigate([this.autocompleteList[currentSuggestionIndex].url.originalUrl]);
      } else {
        this.router.navigate(['/search'], {queryParams: {query: encodeURIComponent(this.inputValue)} });
      }
    }
  }

  clearActiveLink() {
    if (this.autocompleteList.length > 0) {
      const currentSuggestionIndex = this.autocompleteList.findIndex(suggestion => suggestion.isActive == true);

      if (currentSuggestionIndex !== -1) {
        this.autocompleteList[currentSuggestionIndex].isActive = false;
      }
    }
  }

  closeAutocomplete() {
    this.isFocused = false;
  }

  openAutocomplete() {
    this.isFocused = true;
  }

  clearInput() {
    this.inputValue = "";
    this.inputValueToRestore = "";
    this.autocompleteList = [];
    this._query.next('');
  }
}
