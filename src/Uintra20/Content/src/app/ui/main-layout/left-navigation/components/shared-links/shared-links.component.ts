import { Component, OnInit, Input } from "@angular/core";
import { SharedLinksService, ISharedNavData, ISharedLink } from "./shared-links.service";

@Component({
  selector: "app-shared-links",
  templateUrl: "./shared-links.component.html",
  styleUrls: ["./shared-links.component.less"]
})
export class SharedLinksComponent implements OnInit {
  @Input() sharedLinks: Array<ISharedNavData>;
  isOpen: boolean;

  constructor(
    private sharedLinksService: SharedLinksService,
  ) {}

  ngOnInit() {debugger
    this.sharedLinks.map((linkGroup: ISharedNavData) => linkGroup.links.map((link: ISharedLink) => ({
      ...link,
      innerlink: this.checkIfInnerLink(link)
    })));
    this.isOpen = this.sharedLinksService.getOpenState();
  }

  onToggle() {
    this.isOpen = !this.isOpen;
    this.sharedLinksService.setOpenState(this.isOpen);
  }
  checkIfInnerLink(link: ISharedLink) {debugger
    return (link.url.originalUrl.startsWith(window.location.hostname)
      || link.url.originalUrl.startsWith(`http://${window.location.hostname}`)
      || link.url.originalUrl.startsWith(`https://${window.location.hostname}`)) && link.target !=='_blank';
  }
}
