import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-spoiler-section',
  templateUrl: './spoiler-section.component.html',
  styleUrls: ['./spoiler-section.component.less']
})
export class SpoilerSectionComponent {
  @Input() title: string;
  @Input() isSpoilerShowed: boolean = true;

  constructor() { }

  onToggleSpoiler() {
    this.isSpoilerShowed = !this.isSpoilerShowed;
  }
}
