import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateActivityBulletinsComponent } from './create-activity-bulletins.component';

describe('CreateActivityBulletinsComponent', () => {
  let component: CreateActivityBulletinsComponent;
  let fixture: ComponentFixture<CreateActivityBulletinsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateActivityBulletinsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateActivityBulletinsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
