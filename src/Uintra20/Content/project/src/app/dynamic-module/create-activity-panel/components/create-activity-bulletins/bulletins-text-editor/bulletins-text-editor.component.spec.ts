import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BulletinsTextEditorComponent } from './bulletins-text-editor.component';

describe('BulletinsTextEditorComponent', () => {
  let component: BulletinsTextEditorComponent;
  let fixture: ComponentFixture<BulletinsTextEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BulletinsTextEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BulletinsTextEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
