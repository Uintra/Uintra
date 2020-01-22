import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstOrderItemComponent } from './first-order-item.component';

describe('FirstOrderItemComponent', () => {
  let component: FirstOrderItemComponent;
  let fixture: ComponentFixture<FirstOrderItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FirstOrderItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FirstOrderItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
