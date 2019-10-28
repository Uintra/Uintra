import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleStartPanelComponent } from './article-start-panel.component';

describe('ArticleStartPanelComponent', () => {
  let component: ArticleStartPanelComponent;
  let fixture: ComponentFixture<ArticleStartPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArticleStartPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArticleStartPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
