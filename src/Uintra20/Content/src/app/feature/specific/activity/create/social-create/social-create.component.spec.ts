import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SocialCreateComponent } from './social-create.component';

describe('SocialCreateComponent', () => {
  let component: SocialCreateComponent;
  let fixture: ComponentFixture<SocialCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SocialCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SocialCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
