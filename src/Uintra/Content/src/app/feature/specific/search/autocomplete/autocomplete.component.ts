import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { SearchService } from '../search.service';
import { Router, NavigationStart, ActivatedRoute } from '@angular/router';
import { IAutocompleteItem, IMapedAutocompleteItem } from '../search.interface';

@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.less']
})
export class AutocompleteComponent implements OnInit, OnDestroy {

  private $autocompleteSubscription: Subscription;

  _query = new Subject<string>();
  inputValue = '';
  autocompleteList: IMapedAutocompleteItem[] = [];
  hasResults = true;
  isFocused: boolean;
  inputValueToRestore: string;

  public minNumberOfCharacters: number; 

  constructor(
    private searchService: SearchService,
    private router: Router
  ) {
    this._query.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe((value: string) => {
      if (value.trim() && value.trim().length > this.minNumberOfCharacters) {
        this.$autocompleteSubscription = this.searchService.autocomplete(value).subscribe((res: IAutocompleteItem[]) => {
          this.autocompleteList = res.map(suggestion => ({
            ...suggestion,
            isActive: false
          }));
          this.hasResults = res.length > 0;
        });
      } else {
        this.autocompleteList = [];
      }
    });
  }

  public ngOnInit(): void {
    this.minNumberOfCharacters = this.searchService.minNumberOfCharactersToSearch;

    this.router.events.subscribe(e => {
      if (e instanceof NavigationStart) {
        this.closeAutocomplete();
        this.clearInput();
      }
    });
  }

  public ngOnDestroy(): void {
    if (this.$autocompleteSubscription) { this.$autocompleteSubscription.unsubscribe(); }
  }

  public onKeyClick(keyCode: number): void {
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

  public onQueryChange(val: string): void {
    this.isFocused = true;
    this.inputValue = val;
    this.inputValueToRestore = val;
    const trimedVal = val.replace(/\s+/g, ' ');
    this._query.next(trimedVal);
  }

  public prevSuggestion(): void {
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

  public nextSuggestion(): void {
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

  public goToSearchPage(): void {
    if (this.inputValue.length > 1) {
      const currentSuggestionIndex = this.autocompleteList.length
        ? this.autocompleteList.findIndex(suggestion => suggestion.isActive == true)
        : -1;

      if (currentSuggestionIndex !== -1) {
        this.router.navigate([this.autocompleteList[currentSuggestionIndex].url.originalUrl]);
      } else {
        this.router.navigate(['/search'], { queryParams: { query: this.inputValue } });
      }
    }
  }

  public clearActiveLink(): void {
    if (this.autocompleteList.length > 0) {
      const currentSuggestionIndex = this.autocompleteList.findIndex(suggestion => suggestion.isActive == true);

      if (currentSuggestionIndex !== -1) {
        this.autocompleteList[currentSuggestionIndex].isActive = false;
      }
    }
  }

  public closeAutocomplete(): void {
    this.isFocused = false;
  }

  public openAutocomplete(): void {
    this.isFocused = true;
  }

  public clearInput(): void {
    this.inputValue = '';
    this.inputValueToRestore = '';
    this.autocompleteList = [];
    this._query.next('');
  }
}
