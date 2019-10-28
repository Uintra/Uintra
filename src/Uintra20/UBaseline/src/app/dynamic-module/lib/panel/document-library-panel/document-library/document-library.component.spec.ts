import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentLibraryComponent } from './document-library.component';

describe('DocumentLibraryComponent', () => {
  let component: DocumentLibraryComponent;
  let fixture: ComponentFixture<DocumentLibraryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentLibraryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentLibraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
