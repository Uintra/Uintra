import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopImagePanelComponent } from './top-image-panel.component';

describe('TopImagePanelComponent', () => {
  let component: TopImagePanelComponent;
  let fixture: ComponentFixture<TopImagePanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopImagePanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopImagePanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
