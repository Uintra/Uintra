import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDetailsWrapperComponent } from './group-details-wrapper.component';

describe('GroupDetailsWrapperComponent', () => {
  let component: GroupDetailsWrapperComponent;
  let fixture: ComponentFixture<GroupDetailsWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupDetailsWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupDetailsWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
