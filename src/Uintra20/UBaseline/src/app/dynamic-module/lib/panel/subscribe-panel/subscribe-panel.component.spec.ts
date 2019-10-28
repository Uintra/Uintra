import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscribePanelComponent } from './subscribe-panel.component';

describe('SubscribePanelComponent', () => {
  let component: SubscribePanelComponent;
  let fixture: ComponentFixture<SubscribePanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubscribePanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscribePanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
