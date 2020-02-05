import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailasHeaderComponent } from './detailas-header.component';

describe('DetailasHeaderComponent', () => {
  let component: DetailasHeaderComponent;
  let fixture: ComponentFixture<DetailasHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailasHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailasHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
