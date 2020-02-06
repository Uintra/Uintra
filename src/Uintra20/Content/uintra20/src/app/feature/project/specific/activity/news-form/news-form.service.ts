import { Injectable } from '@angular/core';
import { INewsCreateModel } from '../activity.interfaces';

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
}
