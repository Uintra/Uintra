import { Component, Input } from "@angular/core";
import { IDefaultSpotData } from "src/app/shared/interfaces/panels/spot/spot-panel.interface";

@Component({
  selector: "app-default",
  templateUrl: "./default.component.html",
  styleUrls: ["./default.component.less"],
})
export class DefaultComponent {
  @Input() data: { items: IDefaultSpotData[] };

  constructor() {}

  ITEM_WIDTH: number = 89;

  currentItem: number = 0;
  xDown: number = null;
  yDown: number = null;

  isVideo(item: IDefaultSpotData) {
    return item && item.media && item.media.video;
  }

  isImage(item: IDefaultSpotData) {
    return item && item.media && item.media.image;
  }

  leftSwipe() {
    this.currentItem = Math.min(
      this.currentItem + 1,
      this.data.items.length - 1
    );
  }

  rightSwipe() {
    this.currentItem = Math.max(this.currentItem - 1, 0);
  }

  getTouches = (e) => e.touches || e.originalEvent.touches;

  handleTouchStart(evt) {
    const firstTouch = this.getTouches(evt)[0];
    this.xDown = firstTouch.clientX;
    this.yDown = firstTouch.clientY;
  }

  handleTouchMove(evt) {
    if (!this.xDown || !this.yDown) {
      return;
    }

    const xUp = evt.touches[0].clientX;
    const yUp = evt.touches[0].clientY;

    const xDiff = this.xDown - xUp;
    const yDiff = this.yDown - yUp;

    if (Math.abs(xDiff) > Math.abs(yDiff)) {
      if (xDiff > 0) {
        this.leftSwipe();
      } else {
        this.rightSwipe();
      }
    }
    this.xDown = null;
    this.yDown = null;
  }
}
