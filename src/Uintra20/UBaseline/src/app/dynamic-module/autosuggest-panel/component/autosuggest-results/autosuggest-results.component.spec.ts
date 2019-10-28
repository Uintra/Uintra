import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutosuggestResultsComponent } from './autosuggest-results.component';

describe('AutosuggestResultsComponent', () => {
  let component: AutosuggestResultsComponent;
  let fixture: ComponentFixture<AutosuggestResultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutosuggestResultsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutosuggestResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
