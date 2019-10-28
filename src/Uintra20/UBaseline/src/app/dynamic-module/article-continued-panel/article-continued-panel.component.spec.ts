import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleContinuedPanelComponent } from './article-continued-panel.component';

describe('ArticleContinuedPanelComponent', () => {
  let component: ArticleContinuedPanelComponent;
  let fixture: ComponentFixture<ArticleContinuedPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArticleContinuedPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticleContinuedPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
