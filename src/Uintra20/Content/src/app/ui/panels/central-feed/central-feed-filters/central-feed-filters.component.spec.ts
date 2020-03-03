import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CentralFeedFiltersComponent } from './central-feed-filters.component';

describe('CentralFeedFiltersComponent', () => {
  let component: CentralFeedFiltersComponent;
  let fixture: ComponentFixture<CentralFeedFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CentralFeedFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CentralFeedFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
