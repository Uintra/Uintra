import { Component, Inject, HostListener } from "@angular/core";
import { DOCUMENT } from "@angular/common";

@Component({
  selector: "app-go-to-top-button",
  templateUrl: "./go-to-top-button.component.html",
  styleUrls: ["./go-to-top-button.component.less"]
})
export class GoToTopButtonComponent {
  windowScrolled: boolean;
  constructor(@Inject(DOCUMENT) private document: Document) {}

  @HostListener("window:scroll", [])
  onWindowScroll() {
    if (
      window.pageYOffset > 100 ||
      document.documentElement.scrollTop > 100 ||
      document.body.scrollTop > 100
    ) {
      this.windowScrolled = true;
    } else if (
      (this.windowScrolled && window.pageYOffset) ||
      document.documentElement.scrollTop ||
      document.body.scrollTop < 10
    ) {
      this.windowScrolled = false;
    }
  }

  scrollToBlock(event) {
    event.preventDefault();
    let targetElement = document.querySelectorAll(event.currentTarget.getAttribute('href'))[0];

    if (targetElement) {
      window.scrollTo(0, targetElement.offsetTop);
      targetElement.focus();
    }
  }
}
