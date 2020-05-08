import { Component, OnInit, Input } from "@angular/core";
import { MyLinksService, IMyLink } from "./my-links.service";
import { PageIdService } from "ubaseline-next-for-uintra";
import { Router, Scroll } from '@angular/router';

@Component({
  selector: "app-my-links",
  templateUrl: "./my-links.component.html",
  styleUrls: ["./my-links.component.less"]
})
export class MyLinksComponent implements OnInit {
  @Input() myLinks: Array<IMyLink> = [];
  isOpen: boolean;
  isShowAddButton: boolean;
  currentPageId: number;

  constructor(
    private myLinksService: MyLinksService,
    private pageIdService: PageIdService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.isShowAddButton = this.checkCurrentPage(this.myLinks);
    this.isOpen = this.myLinksService.getOpenState();

    this.router.events.subscribe((val) => {
      if (val instanceof Scroll) {
        this.isShowAddButton = this.checkCurrentPage(this.myLinks);
      }
    });
  }

  onAddLink() {
    this.myLinksService.addMyLinks().subscribe(r => {
      this.myLinks = r;
      this.isShowAddButton = this.checkCurrentPage(r);
    });
  }

  onRemoveLink(link) {
    this.myLinksService.removeMyLink(link.id).subscribe(r => {
      this.myLinks = r;
      this.isShowAddButton = this.checkCurrentPage(r);
    });
  }

  onToggle() {
    this.isOpen = !this.isOpen;
    this.myLinksService.setOpenState(this.isOpen);
  }

  onDND(links: Array<IMyLink>) {
    this.myLinksService.setSortState(links);
  }

  checkCurrentPage(r: Array<IMyLink>): boolean {
    return r.every(link => link.url !== this.router.url);
  }
}
