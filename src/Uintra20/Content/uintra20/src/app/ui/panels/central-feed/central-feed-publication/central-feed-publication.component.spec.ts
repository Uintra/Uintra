import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CentralFeedPublicationComponent } from './central-feed-publication.component';

describe('CentralFeedPublicationComponent', () => {
  let component: CentralFeedPublicationComponent;
  let fixture: ComponentFixture<CentralFeedPublicationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CentralFeedPublicationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CentralFeedPublicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
