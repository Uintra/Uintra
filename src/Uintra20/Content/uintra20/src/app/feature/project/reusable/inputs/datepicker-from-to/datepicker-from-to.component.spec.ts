import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatepickerFromToComponent } from './datepicker-from-to.component';

describe('DatepickerFromToComponent', () => {
  let component: DatepickerFromToComponent;
  let fixture: ComponentFixture<DatepickerFromToComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatepickerFromToComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatepickerFromToComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
