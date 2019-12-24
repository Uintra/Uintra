import { Injectable } from '@angular/core';
import { ActivityType } from 'src/app/feature/shared/enums/activity-type.enum';

@Injectable({
  providedIn: 'root'
})
export class ActivityLinkService {

  constructor() { }

  public getBulletinLink(type: ActivityType, activityId): string {
    return `/${type}-details?id=${activityId}`;
  }
}
