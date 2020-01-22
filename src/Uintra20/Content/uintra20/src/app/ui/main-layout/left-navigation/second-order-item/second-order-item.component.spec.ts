import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SecondOrderItemComponent } from './second-order-item.component';

describe('SecondOrderItemComponent', () => {
  let component: SecondOrderItemComponent;
  let fixture: ComponentFixture<SecondOrderItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SecondOrderItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SecondOrderItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
