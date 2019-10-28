import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuotePanelComponent } from './quote-panel.component';

describe('QuotePanelComponent', () => {
  let component: QuotePanelComponent;
  let fixture: ComponentFixture<QuotePanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuotePanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuotePanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
