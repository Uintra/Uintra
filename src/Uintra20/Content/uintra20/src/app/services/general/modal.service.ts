import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  constructor(@Inject(DOCUMENT) private document: Document) { }

  addClassToRoot(className: string): void {
    this.document.documentElement.classList.add(className);
  }

  removeClassFromRoot(className: string): void {
    this.document.documentElement.classList.remove(className);
  }
}
