import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopLevelNavigationComponent } from './top-level-navigation.component';

describe('TopLevelNavigationComponent', () => {
  let component: TopLevelNavigationComponent;
  let fixture: ComponentFixture<TopLevelNavigationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TopLevelNavigationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopLevelNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
