import { Component, OnInit, Input, HostListener } from '@angular/core';
import { AutosuggestService, IAutosuggestResponse, IAutosuggestResponseItem } from '../../service/autosuggest.service';
import { BehaviorSubject, combineLatest, Subscription } from 'rxjs';
import { Router } from '@angular/router';

enum Direction {
  UP = 1, DOWN, ENTER
}
@Component({
  selector: 'app-autosuggest',
  templateUrl: './autosuggest.component.html',
  styleUrls: ['./autosuggest.component.less']
})
export class AutosuggestComponent implements OnInit {
  @Input() data: any;
  results: BehaviorSubject<IAutosuggestResponse>;
  suggestions: IAutosuggestResponseItem[];
  direction$ = new BehaviorSubject(null);
  isShowResults = false;
  isResponseCompleted = false;

  query: string;
  resultsPage: string;

  readonly MAX_RESULTS = 6;

  private _subscriptions: Subscription[] = [];

  constructor(
    private autosuggestService: AutosuggestService,
    private router: Router
  ) { }

  @HostListener('window:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (event.code === 'ArrowDown') this.direction$.next(Direction.DOWN);
    if (event.code === 'ArrowUp') this.direction$.next(Direction.UP);
    if (event.code === 'Enter') this.direction$.next(Direction.ENTER);
  }

  ngOnInit()
  {
    this.results = this.autosuggestService.getResults();

    const subscription = this.results.subscribe(res => {
      this.suggestions = res.items;
      this.resultsPage = res.searchPageUrl;
      this.isResponseCompleted = true;

      this.suggestions = this.higlightQuery(this.suggestions);
    });

    this._subscriptions.push(subscription);

    this.highlight();
  }

  ngOnDestroy()
  {
    this.autosuggestService.clear();
    this._subscriptions.forEach(s => s.unsubscribe());
  }

  higlightQuery(suggestions) {
    const replacer = (str) => `<b>${str}</b>`;
    const re = new RegExp(this.query, "i");

    return suggestions.map(item => ({
      ...item,
      title: item.title.replace(re, replacer)
    }))
  }

  handleQuery(query)
  {
    this.isResponseCompleted = false;
    this.isShowResults = !!query;
    this.query = query;
    this.autosuggestService.getSuggestionsFor(query);
  }

  hideResults()
  {
    this.isShowResults = false;
  }

  clearResults()
  {
    this.autosuggestService.clear();
    this.resultsPage = '';
  }

  toResultsPage() {
    if(this.resultsPage && this.query) {
      const newUrl = `${ this.resultsPage.replace( /\/$/, '' ) }?query=${this.query}`;
      this.router.navigateByUrl(newUrl);
    }
  }

  private highlight()
  {
    let subscription = combineLatest(this.results, this.direction$,
      (res, dir) => {
        if (dir === null) return;
        if (res.items.length === 0) return;

        if (dir === Direction.ENTER)
        {
          let current = res.items.find(i => i.highlighted);
          if (current)this.router.navigate([current.url]);
        }

        if (dir === Direction.DOWN)
        {
          let current = res.items.find(i => i.highlighted);
          if (!current)
          {
            res.items[0].highlighted = true;
          } else {
            current.highlighted = false;
            let nextIdx = res.items.indexOf(current) + 1;
            res.items[res.items.length === nextIdx ? 0 : nextIdx].highlighted = true;
          }
        }

        if (dir === Direction.UP)
        {
          let current = res.items.find(i => i.highlighted);
          if (!current)
          {
            res.items[res.items.length - 1].highlighted = true;
          } else {
            current.highlighted = false;
            let prevIdx = res.items.indexOf(current) - 1;
            res.items[-1 === prevIdx ? (res.items.length - 1) : prevIdx].highlighted = true;
          }
        }

      }).subscribe();

      this._subscriptions.push(subscription);
  }

}
