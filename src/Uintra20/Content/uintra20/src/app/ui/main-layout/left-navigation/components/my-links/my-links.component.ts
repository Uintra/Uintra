import { Component, OnInit } from "@angular/core";
import { MyLinksService, IMyLink } from "./my-links.service";
import { AddButtonService } from "./add-button.service";

@Component({
  selector: "app-my-links",
  templateUrl: "./my-links.component.html",
  styleUrls: ["./my-links.component.less"]
})
export class MyLinksComponent implements OnInit {
  myLinks: Array<IMyLink> = [];
  isOpen: boolean;
  isShowAddButton: boolean;
  currentPageId: number;

  constructor(
    private myLinksService: MyLinksService,
    private addButtonService: AddButtonService
  ) {}

  ngOnInit() {
    this.myLinksService.getMyLinks().subscribe(r => {
      this.myLinks = r;
      this.isShowAddButton = this.checkCurrentPage(r);
    });
    this.isOpen = this.myLinksService.getOpenState();

    this.addButtonService.pageIdTrigger$.subscribe((id: number) => {
      this.currentPageId = id;
      this.isShowAddButton = this.checkCurrentPage(this.myLinks);
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
    if (!this.currentPageId) {
      return true;
    }

    return r.every(link => link.contentId !== this.currentPageId);
  }
}
