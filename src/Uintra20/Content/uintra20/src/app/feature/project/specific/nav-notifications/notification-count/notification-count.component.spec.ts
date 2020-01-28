import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationCountComponent } from './notification-count.component';

describe('NotificationCountComponent', () => {
  let component: NotificationCountComponent;
  let fixture: ComponentFixture<NotificationCountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationCountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationCountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
