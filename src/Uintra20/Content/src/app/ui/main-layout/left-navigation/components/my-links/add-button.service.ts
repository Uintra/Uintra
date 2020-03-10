import { Injectable } from '@angular/core';
import { UmbracoFlatPropertyModel } from '@ubaseline/next';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddButtonService {
  private pageIdTrigger = new Subject();
  pageIdTrigger$ = this.pageIdTrigger.asObservable();

  pageId: number;

  constructor() { }

  setPageId(id: number) {
    this.pageId = id;
    this.pageIdTrigger.next(this.pageId);
  }
}
