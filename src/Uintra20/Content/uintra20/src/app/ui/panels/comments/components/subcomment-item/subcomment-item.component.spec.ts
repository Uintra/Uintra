import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubcommentItemComponent } from './subcomment-item.component';

describe('SubcommentItemComponent', () => {
  let component: SubcommentItemComponent;
  let fixture: ComponentFixture<SubcommentItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubcommentItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubcommentItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
