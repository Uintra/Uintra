import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsOverviewPageComponent } from './news-overview-page.component';

describe('NewsOverviewPageComponent', () => {
  let component: NewsOverviewPageComponent;
  let fixture: ComponentFixture<NewsOverviewPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsOverviewPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsOverviewPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
