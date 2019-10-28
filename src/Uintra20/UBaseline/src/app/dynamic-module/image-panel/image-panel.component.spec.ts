import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImagePanelComponent } from './image-panel.component';

describe('ImagePanelComponent', () => {
  let component: ImagePanelComponent;
  let fixture: ComponentFixture<ImagePanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImagePanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImagePanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
