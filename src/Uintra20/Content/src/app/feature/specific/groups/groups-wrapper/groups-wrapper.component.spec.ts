import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsWrapperComponent } from './groups-wrapper.component';

describe('GroupsTabsComponent', () => {
  let component: GroupsWrapperComponent;
  let fixture: ComponentFixture<GroupsWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupsWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupsWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
