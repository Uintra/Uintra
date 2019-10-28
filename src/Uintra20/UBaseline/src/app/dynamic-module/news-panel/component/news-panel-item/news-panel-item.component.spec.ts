import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsPanelItemComponent } from './news-panel-item.component';

describe('NewsPanelItemComponent', () => {
  let component: NewsPanelItemComponent;
  let fixture: ComponentFixture<NewsPanelItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsPanelItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsPanelItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
