import { Component, OnInit, HostBinding } from "@angular/core";
import { IPanelSettings } from "src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/panel-settings";
import {
  IActivityLinks,
  IOwner,
} from "src/app/feature/specific/activity/activity.interfaces";

interface IComingEvents {
  readonly events: IComingEventsItem[];
  readonly title: string;
  readonly url: string;
  readonly panelSettings?: IPanelSettings;
}
interface IComingEventsItem {
  dates: string[];
  links: IActivityLinks;
  owner: IOwner;
  startDate: string;
  title: string;
  eventDate: string;
  eventMonth: string;
}

@Component({
  selector: "coming-events-panel",
  templateUrl: "./coming-events-panel.html",
  styleUrls: ["./coming-events-panel.less"],
})
export class ComingEventsPanel implements OnInit {
  data: IComingEvents;

  @HostBinding("class") rootClasses;

  constructor() {}

  ngOnInit() {
    this.rootClasses = `
      ${this.data.panelSettings.theme.value.alias || "default-theme"}
    `;
  }
}
