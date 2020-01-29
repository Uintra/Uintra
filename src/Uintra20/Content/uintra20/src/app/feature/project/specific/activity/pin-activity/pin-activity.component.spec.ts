import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PinActivityComponent } from './pin-activity.component';

describe('PinActivityComponent', () => {
  let component: PinActivityComponent;
  let fixture: ComponentFixture<PinActivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PinActivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PinActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
