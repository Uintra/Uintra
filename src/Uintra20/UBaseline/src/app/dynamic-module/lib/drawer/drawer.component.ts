import { Component, OnInit, HostBinding, Output, EventEmitter } from '@angular/core';
import { DrawerService, DrawerState } from './service/drawer.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'ubl-drawer',
  templateUrl: './drawer.component.html',
  styleUrls: ['./drawer.component.less'],
  animations: [
    trigger('animation', [
      state('opened', style({
        left: 0
      })),
      state('closed', style({
        left: '100%'
      })),
      transition('* <=> *', [animate('0.2s')])
    ])
  ]
})
export class DrawerComponent implements OnInit {
  @Output() onChangeState = new EventEmitter();

  states = DrawerState;
  state: BehaviorSubject<DrawerState>;

  private alive$ = new Subject();

  constructor(
    public drawerService: DrawerService
  ) { }

  @HostBinding('class') get className()
  {
    if (this.state)
    {
      return this.state.value === this.states.opened ? 'drawer opened' : 'drawer closed';
    }

    return 'drawer';
  }

  ngOnInit()
  {
    this.state = this.drawerService.state$;
    this.state.pipe(takeUntil(this.alive$)).subscribe((drawerState: DrawerState) => {
      this.onChangeState.emit(drawerState);
    });
  }

  ngOnDestroy()
  {
    this.alive$.next();
    this.alive$.complete();
  }
}
