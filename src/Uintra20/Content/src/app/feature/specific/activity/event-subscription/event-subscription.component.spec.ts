import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventSubscriptionComponent } from './event-subscription.component';

describe('EventSubscriptionComponent', () => {
  let component: EventSubscriptionComponent;
  let fixture: ComponentFixture<EventSubscriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventSubscriptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventSubscriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
