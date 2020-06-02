import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentLibraryPanelComponent } from './document-library-panel.component';

describe('DocumentLibraryPanelComponent', () => {
  let component: DocumentLibraryPanelComponent;
  let fixture: ComponentFixture<DocumentLibraryPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentLibraryPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentLibraryPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
