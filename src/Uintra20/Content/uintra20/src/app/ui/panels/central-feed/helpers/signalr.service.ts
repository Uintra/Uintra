import { Injectable } from "@angular/core";

declare var $: any;

@Injectable({
  providedIn: "root"
})
export class SignalrService {
  private centralFeedHub;
  private callbackFunction;

  constructor() {}

  public createHub(callback) {
    this.callbackFunction = callback;

    this.centralFeedHub = $.connection.centralFeedHub;
    this.centralFeedHub.client.reloadFeed = this.callbackFunction.bind(this);

    $.connection.hub.disconnected(() => {
      if ($.connection.hub.lastError) {
        console.log(
          "Disconnected. Reason: " + $.connection.hub.lastError.message
        );
      }
    });
    $.connection.hub.reconnected(() => {
      this.hubConnectionStart();
    });

    this.hubConnectionStart();
  }

  private hubConnectionStart() {
    $.connection.hub
      .start()
      .done(() => {
        console.log("success");
      })
      .catch(r => console.log(r));
  }

  public hubConnectionStop() {
    $.connection.hub.stop();
  }
}
