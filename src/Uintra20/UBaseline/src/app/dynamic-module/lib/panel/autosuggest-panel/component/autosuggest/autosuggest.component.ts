import { Component, OnInit, Input, HostListener, Optional, Inject } from '@angular/core';
import { AutosuggestService, IAutosuggestResponse, IAutosuggestResponseItem } from '../../service/autosuggest.service';
import { BehaviorSubject, combineLatest, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AUTOSUGGEST_CONFIG, IAutosuggestModuleConfig } from '../../config';
import { IButtonData } from 'src/app/shared/components/button/button.component';
import { ButtonData } from 'src/app/dynamic-module/lib/helper/button-data';
import { TranslateService } from '@ngx-translate/core';

enum Direction {
  UP = 1, DOWN, ENTER
}
@Component({
  selector: 'ubl-autosuggest',
  templateUrl: './autosuggest.component.html',
  styleUrls: ['./autosuggest.component.less']
})
export class AutosuggestComponent implements OnInit {
  @Input() data: any;

  results: BehaviorSubject<IAutosuggestResponse>;
  suggestions: IAutosuggestResponseItem[];
  direction$ = new BehaviorSubject(null);
  isShowResults = false;
  searchPageUrl: IButtonData;

  private alive$ = new Subject();

  constructor(
    private autosuggestService: AutosuggestService,
    private router: Router,
    @Optional() @Inject(AUTOSUGGEST_CONFIG) private config: IAutosuggestModuleConfig,
    private translateService: TranslateService
  ) {
    this.translateService.get('autoSuggest.SeeAllResults').pipe(takeUntil(this.alive$)).subscribe(localized => {
      this.searchPageUrl = ButtonData.fromUrlWithName(this.config.searchPageUrl, localized);
    });
  }

  @HostListener('window:keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (event.code === 'ArrowDown') this.direction$.next(Direction.DOWN);
    if (event.code === 'ArrowUp') this.direction$.next(Direction.UP);
    if (event.code === 'Enter') this.direction$.next(Direction.ENTER);
  }

  ngOnInit() 
  {
    this.results = this.autosuggestService.getResults();

    this.results.pipe(
      takeUntil(this.alive$),
      map(res => res.items)
    ).subscribe(res => {
      this.suggestions = res;
      this.isShowResults = !!this.suggestions.length;
    });

    this.highlight();
  }

  ngOnDestroy()
  {
    this.alive$.next();
    this.alive$.complete();
    this.autosuggestService.clear();
  }

  handleQuery(query: string)
  {
    this.autosuggestService.getSuggestionsFor(query);
  }

  hideResults()
  {
    this.isShowResults = false;
  }

  clearResults()
  {
    this.autosuggestService.clear();
  }

  private highlight()
  {
    combineLatest(this.results, this.direction$,
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
        
      }).pipe(takeUntil(this.alive$)).subscribe();
  }
}
