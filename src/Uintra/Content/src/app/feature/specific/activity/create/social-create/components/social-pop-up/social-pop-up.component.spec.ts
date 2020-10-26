import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SocialPopUpComponent } from './social-pop-up.component';

describe('SocialPopUpComponent', () => {
  let component: SocialPopUpComponent;
  let fixture: ComponentFixture<SocialPopUpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SocialPopUpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SocialPopUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
