import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoPanelPopUpComponent } from './video-panel-pop-up.component';

describe('VideoPanelPopUpComponent', () => {
  let component: VideoPanelPopUpComponent;
  let fixture: ComponentFixture<VideoPanelPopUpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VideoPanelPopUpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideoPanelPopUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
