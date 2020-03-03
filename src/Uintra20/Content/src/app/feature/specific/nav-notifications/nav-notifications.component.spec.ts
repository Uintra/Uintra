import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavNotificationsComponent } from './nav-notifications.component';

describe('NavNotificationsComponent', () => {
  let component: NavNotificationsComponent;
  let fixture: ComponentFixture<NavNotificationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavNotificationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavNotificationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
