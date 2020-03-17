import { Injectable } from '@angular/core';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { ISelectItem } from 'src/app/feature/reusable/inputs/select/select.component';
import { IOwner } from '../../activity.interfaces';
import { IEventCreateModel } from './event-create.interface';

@Injectable({
  providedIn: 'root'
})
export class EventFormService {

  constructor() { }

  getEventDataInitialValue(data: any): IEventCreateModel {
    return {
      ownerId: data.creator.id,
      title: data.title || "",
      description: data.description || "",
      tagIdsData: data.tags || [],
      publishDate: data.publishDate || undefined,
      startDate: data.startDate || undefined,
      endDate: data.endDate || undefined,
      locationTitle: data.locationTitle || "",
      location: {
        address: (data.location && data.location.address) || null,
        shortAddress:(data.location && data.location.shortAddress) || null
      },
      canSubscribe: data.canSubscribe || false,
      subscribeNotes: data.subscribeNotes || "",
      pinAllowed: data.pinAllowed,
      isPinned: data.isPinned || false,
      newMedia: "",
      media: data.media || null,
      groupId: data.groupId || null
    };
  }
  getTagsForResponse(selectedTags: ITagData[] = []): string[] {
    return selectedTags.map(tag => tag.id);
  }
  getMediaIdsForResponse(files: Array<any>): string {
    return files.map(file => file[1]).join(",");
  }
  getOwners(members: Array<any>, creator: IOwner) {
    const owners = this.getMembers(members);
    if (creator) {
      owners.push({
        id: creator.id,
        text: creator.displayedName
      });
    }
    return owners;
  }
  private getMembers(members = []): ISelectItem[] {
    return members.map(member => ({
      id: member.id,
      text: member.displayedName
    }));
  }
}
