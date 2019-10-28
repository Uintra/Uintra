import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IconPanelItemComponent } from './icon-panel-item.component';

describe('IconPanelItemComponent', () => {
  let component: IconPanelItemComponent;
  let fixture: ComponentFixture<IconPanelItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconPanelItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconPanelItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
