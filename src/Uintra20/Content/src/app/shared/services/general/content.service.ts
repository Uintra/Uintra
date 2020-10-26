import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ContentService {

  constructor() { }

  public makeReadonly(targetName: string): void {

    const elements = document.querySelectorAll(targetName);

    elements.forEach(e => e.setAttribute('readonly', 'readonly'));
  }
}
