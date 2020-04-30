import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

declare var $: any;

@Injectable({
  providedIn: "root"
})
export class SignalrService {
  private uintraHub;

  constructor() {}

  public startHub() {
    debugger
    this.uintraHub = $.connection.uintraHub;
    this.uintraHub.notificationSubject = new Subject<any>();
    this.uintraHub.centralFeedSubject = new Subject<any>();
    this.uintraHub.showPopupSubject = new Subject<any>();
    this.uintraHub.client.updateNotifications = this.broadcastUpdateNotifications;
    this.uintraHub.client.reloadFeed = this.broadcastReloadFeed;
    this.uintraHub.client.showPopup = this.broadcastShowPopup;

    $.connection.hub.disconnected(() => {
      if ($.connection.hub.lastError) {
        console.log(
          "UintraHub connection: disconnected.reason: " +
            $.connection.hub.lastError.message
        );
      }
    });
    $.connection.hub.reconnected(() => {
      this.hubConnectionStart("reconnected");
    });

    this.hubConnectionStart("started");
  }

  private hubConnectionStart(logmessage: string) {
    if ($.connection.hub.state === $.signalR.connectionState.disconnected) {
      $.connection.hub
        .start()
        .done(r => {
          console.log("UintraHub connection:" + logmessage);
        })
        .catch(r => console.log(r));
    }
  }

  public getUpdateNotificationsSubjects(): Observable<any> {
    return this.uintraHub.notificationSubject.asObservable();
  }

  public getReloadFeedSubjects(): Observable<any> {
    return this.uintraHub.centralFeedSubject.asObservable();
  }

  public getShowPopup(): Observable<any> {
    return this.uintraHub.showPopupSubject.asObservable();
  }

  private broadcastUpdateNotifications(notifications = []) {
    // @ts-ignore: Unreachable code error: 'this' variable is uintraHub context
    this.notificationSubject.next(notifications);
  }

  private broadcastReloadFeed() {
    // @ts-ignore: Unreachable code error: 'this' variable is uintraHub context
    this.centralFeedSubject.next();
  }

  private broadcastShowPopup(popups = []) {
    // @ts-ignore: Unreachable code error: 'this' variable is uintraHub context
    this.showPopupSubject.next(popups);
  }

  public hubConnectionStop() {
    $.connection.centralFeedHub.server.onDisconnected();
    console.log("UintraHub connection: closed");
  }
}
