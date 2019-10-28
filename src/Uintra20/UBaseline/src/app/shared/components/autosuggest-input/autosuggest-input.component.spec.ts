import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutosuggestInputComponent } from './autosuggest-input.component';

describe('AutosuggestInputComponent', () => {
  let component: AutosuggestInputComponent;
  let fixture: ComponentFixture<AutosuggestInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutosuggestInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutosuggestInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
