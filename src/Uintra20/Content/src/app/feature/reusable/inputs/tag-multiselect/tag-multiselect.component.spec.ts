import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TagMultiselectComponent } from './tag-multiselect.component';

describe('TagMultiselectComponent', () => {
  let component: TagMultiselectComponent;
  let fixture: ComponentFixture<TagMultiselectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TagMultiselectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TagMultiselectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
