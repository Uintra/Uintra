import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNavMobileComponent } from './user-nav-mobile.component';

describe('UserNavMobileComponent', () => {
  let component: UserNavMobileComponent;
  let fixture: ComponentFixture<UserNavMobileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserNavMobileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserNavMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
