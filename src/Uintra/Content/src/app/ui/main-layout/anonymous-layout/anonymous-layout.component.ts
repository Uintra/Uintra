import { ViewportScroller } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-anonymous-layout',
  templateUrl: './anonymous-layout.component.html',
  styleUrls: ['./anonymous-layout.component.less']
})
export class AnonymousLayoutComponent implements OnInit {

  constructor(
    private viewportScroller: ViewportScroller
  ) { }

  ngOnInit() {
  }

  public closeLeftNav(): void {
    document.body.classList.remove('nav--open');
  }


  scrollToBlock(event) {
    event.preventDefault();
    let targetElement = document.querySelectorAll(event.currentTarget.getAttribute('href'))[0];

    targetElement.focus();
    targetElement.blur();

    this.viewportScroller.scrollToPosition([0, targetElement.offsetTop])
  }

}
