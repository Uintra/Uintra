import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class DownloadedPhotoWatcherService {
  private avatarUpdateTrigger = new Subject();
  avatarUpdateTrigger$ = this.avatarUpdateTrigger.asObservable();

  constructor() {}

  public updateAvatar = (data: string): void =>
    this.avatarUpdateTrigger.next(data);

  public getTrigger = () => {
    return this.avatarUpdateTrigger$;
  };
}
