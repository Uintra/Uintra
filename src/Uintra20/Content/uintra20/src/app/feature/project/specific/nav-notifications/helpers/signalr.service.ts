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
    this.uintraHub = $.connection.uintraHub;
    this.uintraHub.notificationSubject = new Subject<any>();
    this.uintraHub.centralFeedSubject = new Subject<any>();
    this.uintraHub.client.updateNotifications = this.broadcastUpdateNotifications;
    this.uintraHub.client.reloadFeed = this.broadcastReloadFeed;

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

  private broadcastUpdateNotifications(notifications = []) {
    // @ts-ignore: Unreachable code error: 'this' variable is uintraHub context
    this.notificationSubject.next(notifications);
  }

  private broadcastReloadFeed() {
    // @ts-ignore: Unreachable code error: 'this' variable is uintraHub context
    this.centralFeedSubject.next();
  }

  public hubConnectionStop() {
    $.connection.centralFeedHub.server.onDisconnected();
    console.log("UintraHub connection: closed");
  }
}
