import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoPanelComponent } from './video-panel.component';

describe('VideoPanelComponent', () => {
  let component: VideoPanelComponent;
  let fixture: ComponentFixture<VideoPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VideoPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideoPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
