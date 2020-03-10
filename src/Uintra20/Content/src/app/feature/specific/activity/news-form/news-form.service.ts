import { Injectable } from '@angular/core';
import { INewsCreateModel, IOwner } from '../activity.interfaces';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { ISelectItem } from 'src/app/feature/reusable/inputs/select/select.component';

@Injectable({
  providedIn: 'root'
})
export class NewsFormService {

  constructor() { }

  getNewsDataInitialValue(data: INewsCreateModel): INewsCreateModel {
    return {
      ownerId: data.ownerId,
      title: data.title || "",
      description: data.description || "",
      publishDate: undefined,
      location: {
        address: (data.location && data.location.address) || null,
        shortAddress:
          (data.location && data.location.shortAddress) || null
      },
      endPinDate: data.endPinDate || null,
      isPinned: data.isPinned || false,
      media: data.media || null
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
