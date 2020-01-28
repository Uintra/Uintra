import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostHeaderComponent } from './post-header.component';

describe('PostHeaderComponent', () => {
  let component: PostHeaderComponent;
  let fixture: ComponentFixture<PostHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
