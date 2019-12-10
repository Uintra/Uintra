import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpoilerSectionComponent } from './spoiler-section.component';

describe('SpoilerSectionComponent', () => {
  let component: SpoilerSectionComponent;
  let fixture: ComponentFixture<SpoilerSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpoilerSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpoilerSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
