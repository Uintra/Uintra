import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateActivityPanelComponent } from './create-activity-panel.component';

describe('CreateActivityPanelComponent', () => {
  let component: CreateActivityPanelComponent;
  let fixture: ComponentFixture<CreateActivityPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateActivityPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateActivityPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
