import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentHeaderComponent } from './comment-header.component';

describe('CommentHeaderComponent', () => {
  let component: CommentHeaderComponent;
  let fixture: ComponentFixture<CommentHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
