import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  private _height: number;

  constructor() { }

  get height()
  {
    return this._height
  }
  set height(val: number)
  {
    if (val === undefined || val === null) return;

    this._height = val;
  }
}
