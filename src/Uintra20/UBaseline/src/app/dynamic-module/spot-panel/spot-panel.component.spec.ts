import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpotPanelComponent } from './spot-panel.component';

describe('SpotPanelComponent', () => {
  let component: SpotPanelComponent;
  let fixture: ComponentFixture<SpotPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SpotPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpotPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
