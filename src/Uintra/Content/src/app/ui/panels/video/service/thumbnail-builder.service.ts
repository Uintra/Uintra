import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThumbnailBuilderService {

  constructor() { }

  create(data: string) {
    return {
        alt: data,
        height: 900,
        width: 1600,
        src: data,
        sources: [
          {
            media: `(min-width:1440px)`,
            srcSet: `${data}?center=0.5,0.5&mode=crop&width=1920&height=1080`
          },
          {
            media: `(min-width: 1014px)`,
            srcSet: `${data}?center=0.5,0.5&mode=crop&width=600&height=400`
          },
          {
            media: `(min-width: 860px)`,
            srcSet: `${data}?center=0.5,0.5&mode=crop&width=1024&height=700`
          },
          {
            media: ``,
            srcSet: `${data}?center=0.5,0.5&mode=crop&width=600&height=400`
          }
        ]
    };
  }
}

