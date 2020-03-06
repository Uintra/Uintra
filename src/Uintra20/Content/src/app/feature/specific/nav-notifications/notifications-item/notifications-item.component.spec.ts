import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationsItemComponent } from './notifications-item.component';

describe('NotificationsItemComponent', () => {
  let component: NotificationsItemComponent;
  let fixture: ComponentFixture<NotificationsItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationsItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationsItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
