import { Component, OnInit, Input } from '@angular/core';
import { MyLinksService, IMyLink } from './my-links.service';
import { Router, Scroll } from '@angular/router';
import { Indexer } from '../../../../../shared/abstractions/indexer';



@Component({
  selector: 'app-my-links',
  templateUrl: './my-links.component.html',
  styleUrls: ['./my-links.component.less']
})
export class MyLinksComponent extends Indexer<number> implements OnInit {
  @Input()
  public myLinks: Array<IMyLink> = [];
  public isOpen: boolean;
  public isShowAddButton: boolean;

  constructor(
    private router: Router,
    private myLinksService: MyLinksService
  ) {
    super();
  }

  public ngOnInit(): void {
    this.isShowAddButton = this.isPageOnMyLinks(this.myLinks);
    this.isOpen = this.myLinksService.getOpenState();
    this.router.events.subscribe((val) => {
      if (val instanceof Scroll) {
        this.isShowAddButton = this.isPageOnMyLinks(this.myLinks);
      }
    });
  }

  public onAddLink(): void {
    this.myLinksService.addMyLinks().subscribe(r => {
      this.myLinks = r;
      this.isShowAddButton = this.isPageOnMyLinks(r);
    });
  }

  public onRemoveLink(link): void {
    this.myLinksService.removeMyLink(link.id).subscribe(r => {
      this.myLinks = r;
      this.isShowAddButton = this.isPageOnMyLinks(r);
    });
  }

  public onToggle(): void {
    this.isOpen = !this.isOpen;
    this.myLinksService.setOpenState(this.isOpen);
  }

  public onDND(links: Array<IMyLink>): void {
    this.myLinksService.setSortState(links);
  }

  private isPageOnMyLinks = (links: Array<IMyLink>): boolean =>
    links.every(l => l.url !== this.router.url)
}


