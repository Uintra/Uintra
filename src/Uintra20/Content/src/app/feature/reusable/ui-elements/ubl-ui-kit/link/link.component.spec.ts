import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LinkComponent } from './link.component';
import { RouterTestingModule } from '@angular/router/testing';
import { Component } from '@angular/core';
import { By } from '@angular/platform-browser';
import { LinkTargetType } from 'src/app/shared/interface/link';
import { LinkModule } from './link.module';

fdescribe('LinkComponent', () => {
  let component: TestApp;
  let fixture: ComponentFixture<TestApp>;
  let linkComponent: LinkComponent;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ 
        TestApp
      ],
      imports: [
        LinkModule,
        RouterTestingModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestApp);
    component = fixture.componentInstance;
    linkComponent = fixture.debugElement.query(By.css('.test')).componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("shouldn't have link if there is no data and sould have link if there is data", () => {
    expect(fixture.debugElement.query(By.css('.link__link'))).toBeFalsy();
    linkComponent.data = linkData;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.link__link'))).toBeTruthy();
  });

  it("should have link text if there is one in content", () => {
    linkComponent.data = linkData;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.link__link'))).toBeTruthy();
    expect(fixture.debugElement.query(By.css('.link__text')).nativeElement.textContent).toBe('test text');
  });

  it("should have before text and after text if they are in content", () => {
    linkComponent = fixture.debugElement.query(By.css('.test--with-before-and-after')).componentInstance;
    linkComponent.data = linkData;
    fixture.detectChanges();
    console.log(fixture.debugElement.query(By.css('.link__link')));
    expect(fixture.debugElement.query(By.css('.link__link')).nativeElement.textContent).toBe('before text test text after text');
  });

  it("should distinguish innerRoute and outer", () => {
    linkComponent.data = linkData;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.link__link')).properties.href).toContain(`/${linkData.url}`);
    
    linkComponent.data.innerRoute = false;
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.css('.link__link')).properties.href).toBe(linkData.url);
  });
});

@Component({
  selector: 'test-app',
  template: `
    <ubl-link class="test">test text</ubl-link>

    <ubl-link class="test--with-before-and-after">
      test text
      <div position="before">before text</div>
      <div position="after">after text</div>
    </ubl-link>
  `
})
class TestApp {
}

const linkData = {
  url: 'string',
  innerRoute: true,
  target: '_self' as LinkTargetType,
  queryParams: {label: 'string'},
  label: 'string',
}
