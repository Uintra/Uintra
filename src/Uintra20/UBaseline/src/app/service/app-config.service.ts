import { Injectable, Inject } from '@angular/core';
import { config } from '../app.config';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  readonly config = config;
  constructor(
    @Inject(DOCUMENT) private document: Document
  ) { }

  getHostName()
  {
    const {hostname, protocol} = this.document.location;

    return `${protocol}//${hostname}`;
  }
}
