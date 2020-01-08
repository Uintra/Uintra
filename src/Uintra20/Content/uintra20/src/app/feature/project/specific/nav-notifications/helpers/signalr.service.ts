import { Injectable } from '@angular/core';

declare var $: any;

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private notificationsHub;
  private callbackFunction;

  constructor() { }

  public createHub(callback) {
    this.callbackFunction = callback;

    this.notificationsHub = $.connection.notificationsHub;
    this.notificationsHub.client.updateNotificationsCount = this.callbackFunction.bind(this);

    $.connection.hub.disconnected(() => {
      if ($.connection.hub.lastError) { alert('Disconnected. Reason: ' + $.connection.hub.lastError.message); }
    });
    $.connection.hub.reconnected(() => {
      this.hubConnectionStart();
    });

    this.hubConnectionStart();
  }

  private setNotNotifiedCount() {
    this.notificationsHub.server.getNotNotifiedCount()
      .then((data) => {
        this.callbackFunction(data);
      });
  }

  private hubConnectionStart() {
    $.connection.hub
      .start()
      .done(r => this.setNotNotifiedCount())
      .catch(r => console.log(r));
  }
}
