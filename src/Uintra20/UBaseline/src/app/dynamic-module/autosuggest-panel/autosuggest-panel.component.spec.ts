import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutosuggestPanelComponent } from './autosuggest-panel.component';

describe('AutosuggestPanelComponent', () => {
  let component: AutosuggestPanelComponent;
  let fixture: ComponentFixture<AutosuggestPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutosuggestPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutosuggestPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
