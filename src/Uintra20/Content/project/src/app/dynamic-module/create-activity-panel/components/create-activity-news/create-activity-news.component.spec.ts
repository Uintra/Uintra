import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateActivityNewsComponent } from './create-activity-news.component';

describe('CreateActivityNewsComponent', () => {
  let component: CreateActivityNewsComponent;
  let fixture: ComponentFixture<CreateActivityNewsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateActivityNewsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateActivityNewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
