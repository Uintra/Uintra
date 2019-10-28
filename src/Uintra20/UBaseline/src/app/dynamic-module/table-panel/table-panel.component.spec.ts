import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablePanelComponent } from './table-panel.component';

describe('TablePanelComponent', () => {
  let component: TablePanelComponent;
  let fixture: ComponentFixture<TablePanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablePanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablePanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
