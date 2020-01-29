import { Component, OnInit } from "@angular/core";
import { MyLinksService, IMyLink } from "./my-links.service";

@Component({
  selector: "app-my-links",
  templateUrl: "./my-links.component.html",
  styleUrls: ["./my-links.component.less"]
})
export class MyLinksComponent implements OnInit {
  myLinks: Array<IMyLink>;
  isOpen: boolean;

  constructor(private myLinksService: MyLinksService) {}

  ngOnInit() {
    this.getLinks();
  }

  getLinks() {
    this.myLinksService.getMyLinks().subscribe(r => {
      this.myLinks = r;
    });
  }

  onAddLink(contentId = "") {
    this.myLinksService.addMyLinks(contentId).subscribe(r => {});
  }

  onToggle() {
    this.isOpen = !this.isOpen;
  }

  onDND(links: Array<IMyLink>) {
    this.myLinksService.setSortState(links);
  }

  onRemoveLink(link) {
    this.myLinksService.removeMyLink(link.id).subscribe(r => {
      this.myLinks = r;
    });
  }
}
