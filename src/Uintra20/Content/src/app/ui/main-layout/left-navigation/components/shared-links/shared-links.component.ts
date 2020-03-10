import { Component, OnInit, Input } from "@angular/core";
import { SharedLinksService, ISharedNavData } from "./shared-links.service";

@Component({
  selector: "app-shared-links",
  templateUrl: "./shared-links.component.html",
  styleUrls: ["./shared-links.component.less"]
})
export class SharedLinksComponent implements OnInit {
  @Input() sharedLinks: Array<ISharedNavData>;
  isOpen: boolean;

  constructor(private sharedLinksService: SharedLinksService) {}

  ngOnInit() {
    this.isOpen = this.sharedLinksService.getOpenState();
  }

  onToggle() {
    this.isOpen = !this.isOpen;
    this.sharedLinksService.setOpenState(this.isOpen);
  }
}
