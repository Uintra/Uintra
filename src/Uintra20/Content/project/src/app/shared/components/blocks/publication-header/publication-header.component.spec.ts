import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicationHeaderComponent } from './publication-header.component';

describe('PublicationHeaderComponent', () => {
  let component: PublicationHeaderComponent;
  let fixture: ComponentFixture<PublicationHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PublicationHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicationHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
