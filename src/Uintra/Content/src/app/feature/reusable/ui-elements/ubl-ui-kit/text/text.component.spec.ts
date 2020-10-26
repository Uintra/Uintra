import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TextComponent } from './text.component';
import { Component, DebugElement } from '@angular/core';
import { TextModule } from './text.module';
import { By } from '@angular/platform-browser';

fdescribe('TextComponent', () => {
  let component: TestApp;
  let fixture: ComponentFixture<TestApp>;
  let textComponent: TextComponent;
  let textEl = DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestApp ],
      imports: [
        TextModule,
      ],
    });
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestApp);
    component = fixture.componentInstance;
    textComponent = fixture.debugElement.query(By.css('section[ubl-text]')).componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get text-panel--text class with ubl-text text data', () => {
    fixture.debugElement.query(By.css('.ubl-text')).componentInstance.ublText = 'some text';
    textComponent.ngOnInit();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('text-panel--text');
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('ubl-text');
  });

  it('should get text-panel--quote class with ubl-text quote data', () => {
    fixture.debugElement.query(By.css('.ubl-text')).componentInstance.ublText = '<q>some quote text</q>';
    textComponent.ngOnInit();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('text-panel--quote');
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('ubl-text');
  });

  it('should get text-panel--code class with ubl-text code data', () => {
    fixture.debugElement.query(By.css('.ubl-text')).componentInstance.ublText = '<code>some code</code>';
    textComponent.ngOnInit();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('text-panel--code');
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('ubl-text');
  });

  it('should get text-panel--html class with ubl-text html data', () => {
    fixture.debugElement.query(By.css('.ubl-text')).componentInstance.ublText = '<code>some code</code><q>some quote text</q>';
    textComponent.ngOnInit();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('text-panel--html');
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('rte');
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('ubl-text');
  });

  it('should get right classes with ng-content data and to have initial classes', () => {
    textComponent.ngOnInit();
    fixture.detectChanges();
    
    expect(fixture.debugElement.query(By.css('.text')).nativeElement).toHaveClass('text-panel--text');
    expect(fixture.debugElement.query(By.css('.text')).nativeElement).toHaveClass('text');
    
    expect(fixture.debugElement.query(By.css('.quote')).nativeElement).toHaveClass('text-panel--quote');
    expect(fixture.debugElement.query(By.css('.quote')).nativeElement).toHaveClass('quote');

    expect(fixture.debugElement.query(By.css('.code')).nativeElement).toHaveClass('text-panel--code');
    expect(fixture.debugElement.query(By.css('.code')).nativeElement).toHaveClass('code');

    expect(fixture.debugElement.query(By.css('.html')).nativeElement).toHaveClass('text-panel--html');
    expect(fixture.debugElement.query(By.css('.html')).nativeElement).toHaveClass('rte');
    expect(fixture.debugElement.query(By.css('.html')).nativeElement).toHaveClass('html');
  });

  it('should render before and after elements if they are provided', () => {
    textComponent.ublBeforeText = 'before text';
    textComponent.ublAfterText = 'after text';
    textComponent.ngOnInit();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('section[ubl-text]')).childNodes.some(childNode => childNode.nativeNode.className === 'text-panel__before-content' && childNode.nativeNode.innerHTML === 'before text')).toBeTruthy();
    expect(fixture.debugElement.query(By.css('section[ubl-text]')).childNodes.some(childNode => childNode.nativeNode.className === 'text-panel__after-content' && childNode.nativeNode.innerHTML === 'after text')).toBeTruthy();
  });

  it('should have only override and initial classes if override class provided', () => {
    fixture.debugElement.query(By.css('.ubl-text')).componentInstance.ublText = '<q>some quote text</q>';
    textComponent.classToOverride = 'code';
    textComponent.ngOnInit();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('text-panel--code');
    expect(fixture.debugElement.query(By.css('.ubl-text')).nativeElement).toHaveClass('ubl-text');
  });
});

@Component({
  selector: 'test-app',
  template: `
    <section ubl-text class="ubl-text"></section>
    <section ubl-text class="text">some text</section>
    <section ubl-text class="quote"><q>some quote text</q></section>
    <section ubl-text class="code"><code>some code</code></section>
    <section ubl-text class="html">
      <code>some code</code>
      <q>some quote text</q>
    </section>
  `
})
class TestApp {
}
