import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RadioLinkGroupComponent } from './radio-link-group.component';

describe('RadioLinkGroupComponent', () => {
  let component: RadioLinkGroupComponent;
  let fixture: ComponentFixture<RadioLinkGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RadioLinkGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RadioLinkGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
