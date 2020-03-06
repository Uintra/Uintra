import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropzoneExistingImagesComponent } from './dropzone-existing-images.component';

describe('DropzoneExistingImagesComponent', () => {
  let component: DropzoneExistingImagesComponent;
  let fixture: ComponentFixture<DropzoneExistingImagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropzoneExistingImagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropzoneExistingImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
