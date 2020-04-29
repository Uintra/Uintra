import {
  Injectable,
  Inject,
  ComponentFactoryResolver,
  ApplicationRef,
  Injector,
  EmbeddedViewRef
} from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  currentComponentRef: any;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private componentFactoryResolver: ComponentFactoryResolver,
    private appRef: ApplicationRef,
    private injector: Injector
    ) { }

  addClassToRoot(className: string): void {
    this.document.documentElement.classList.add(className);
  }

  removeClassFromRoot(className: string): void {
    this.document.documentElement.classList.remove(className);
  }

  appendComponentToBody(component: any, data?: any, callerElementRef?: any) {
    const componentRef = this.componentFactoryResolver
      .resolveComponentFactory(component)
      .create(this.injector);

    if (data && typeof componentRef.instance === 'object') {
      Object.assign(componentRef.instance as object, data);
      Object.assign(componentRef.instance as object, callerElementRef);
    }

    this.currentComponentRef = componentRef;

    this.appRef.attachView(componentRef.hostView);

    const domElem = (componentRef.hostView as EmbeddedViewRef<any>)
      .rootNodes[0] as HTMLElement;

    document.body.appendChild(domElem);
  }

  removeComponentFromBody() {
    if (this.currentComponentRef) {
      this.appRef.detachView(this.currentComponentRef.hostView);
      this.currentComponentRef.destroy();
      this.currentComponentRef = null;
    }
  }
}
