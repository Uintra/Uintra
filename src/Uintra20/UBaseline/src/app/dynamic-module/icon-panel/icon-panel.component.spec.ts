import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IconPanelComponent } from './icon-panel.component';

describe('IconPanelComponent', () => {
  let component: IconPanelComponent;
  let fixture: ComponentFixture<IconPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
