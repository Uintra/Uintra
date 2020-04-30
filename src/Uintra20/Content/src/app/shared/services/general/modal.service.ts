import {
  Injectable,
  Inject,
  ComponentFactoryResolver,
  ApplicationRef,
  Injector,
  EmbeddedViewRef
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  currentComponentRef: any;
  componentsById: any = {};
  closePopUpSubject = new Subject();

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

  appendComponentToBody(component: any, data?: any, callerElementRef?: any, id?: string, needsShadowBackground?: boolean) {
    const componentRef = this.componentFactoryResolver
      .resolveComponentFactory(component)
      .create(this.injector);

    if (data && typeof componentRef.instance === 'object') {
      Object.assign(componentRef.instance as object, data);
      Object.assign(componentRef.instance as object, callerElementRef);
    }

    id ? this.componentsById[id] = componentRef : this.currentComponentRef = componentRef;
    if (id) {
      Object.assign(componentRef.instance as object, {id: id, needsShadowBackground: needsShadowBackground});
    } else {
      Object.assign(componentRef.instance as object, {needsShadowBackground: true});
    }

    this.appRef.attachView(componentRef.hostView);

    const domElem = (componentRef.hostView as EmbeddedViewRef<any>)
      .rootNodes[0] as HTMLElement;

    document.body.appendChild(domElem);
  }

  removeComponentFromBody(id?: string) {
    if (id) {
      this.appRef.detachView(this.componentsById[id].hostView);
      this.componentsById[id].destroy();
      this.componentsById[id] = null;
      if (this.componentsById[parseInt(id) - 1]) {
        Object.assign(this.componentsById[parseInt(id) - 1].instance as object, {needsShadowBackground: true});
      }
    } else {
      if (this.currentComponentRef) {
        this.appRef.detachView(this.currentComponentRef.hostView);
        this.currentComponentRef.destroy();
        this.currentComponentRef = null;
      }
    }
  }
}
