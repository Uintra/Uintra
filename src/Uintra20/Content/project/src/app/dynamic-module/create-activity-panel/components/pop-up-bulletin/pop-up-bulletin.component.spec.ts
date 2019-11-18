import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PopUpBulletinComponent } from './pop-up-bulletin.component';

describe('PopUpBulletinComponent', () => {
  let component: PopUpBulletinComponent;
  let fixture: ComponentFixture<PopUpBulletinComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PopUpBulletinComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PopUpBulletinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
