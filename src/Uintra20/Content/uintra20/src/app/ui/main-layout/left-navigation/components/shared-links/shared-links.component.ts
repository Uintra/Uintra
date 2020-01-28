import { Component, OnInit } from "@angular/core";
import { SharedLinksService, ISharedNavData } from "./shared-links.service";

@Component({
  selector: "app-shared-links",
  templateUrl: "./shared-links.component.html",
  styleUrls: ["./shared-links.component.less"]
})
export class SharedLinksComponent implements OnInit {
  sharedLinks: Array<ISharedNavData>;
  isOpen: boolean;

  constructor(private sharedLinksService: SharedLinksService) {}

  ngOnInit() {
    this.sharedLinksService.getSharedLinks().subscribe(r => {
      this.sharedLinks = r;
    });
  }

  onToggle() {
    this.isOpen = !this.isOpen;
  }
}
