import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class AppService {
  private aceessPageTrigger = new Subject<boolean>();
  aceessPageTrigger$ = this.aceessPageTrigger.asObservable();

  constructor() {}

  public setPageAccess = (hasAccess: boolean): void => {
    this.aceessPageTrigger.next(hasAccess);
  };

  public getPageAccessTrigger = () => {
    return this.aceessPageTrigger$;
  };
}
