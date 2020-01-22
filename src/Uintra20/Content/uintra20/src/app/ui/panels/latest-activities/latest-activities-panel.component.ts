import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { ILatestActivitiesPanel } from './latest-activities-panel.interface';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'latest-activities-panel',
  templateUrl: './latest-activities-panel.component.html',
  styleUrls: ['./latest-activities-panel.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class LatestActivitiesPanelComponent implements OnInit {

  constructor(private sanitizer: DomSanitizer) { }

  public readonly data: ILatestActivitiesPanel;
  public title: string;
  public teaser: string;
  public activityCells: any;
  public showAll: false;

  public ngOnInit(): void {
    this.title = this.data.title.get();
    this.teaser = this.data.teaser.get();
    this.activityCells = Object.values(this.data.feed.get());
    this.showAll = this.data.showSeeAllButton.get();
  }

  getSanitizedDescription(cell) {
    return this.sanitizer.bypassSecurityTrustHtml(cell.get().activity.get().description.get());
  }
}


