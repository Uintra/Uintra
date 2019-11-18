import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateActivityEventsComponent } from './create-activity-events.component';

describe('CreateActivityEventsComponent', () => {
  let component: CreateActivityEventsComponent;
  let fixture: ComponentFixture<CreateActivityEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateActivityEventsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateActivityEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
