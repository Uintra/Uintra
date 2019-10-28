import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export enum DrawerState {
  closed = 1, opened
}

@Injectable({
  providedIn: 'root'
})
export class DrawerService {
  state$ = new BehaviorSubject(DrawerState.closed);

  open()
  {
    this.state$.next(DrawerState.opened);
  }

  close()
  {
    this.state$.next(DrawerState.closed);
  }

  toggle()
  {
    this.state$.value === DrawerState.opened ? this.close() : this.open();
  }
}
