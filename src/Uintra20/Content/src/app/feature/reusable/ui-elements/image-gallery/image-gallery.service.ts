import { Injectable } from '@angular/core';
import PhotoSwipe from "photoswipe";
import PhotoSwipeUI_Default from 'photoswipe/dist/photoswipe-ui-default';

@Injectable({
  providedIn: 'root'
})
export class ImageGalleryService {

  constructor() { }

  open(items, index) {
    let pswpElement = document.querySelectorAll('.pswp')[0];

    // build items array, example below

    // let items = [
    //     {
    //         src: 'https://placekitten.com/600/400',
    //         w: 600,
    //         h: 400
    //     },
    //     {
    //         src: 'https://placekitten.com/1200/900',
    //         w: 1200,
    //         h: 900
    //     }
    // ];

    // define options (if needed)
    let options = {
        // optionName: 'option value'
        // for example:
        index: index, // start at first slide
        history: false
    };

    // Initializes and opens PhotoSwipe
    let gallery = new PhotoSwipe( pswpElement, PhotoSwipeUI_Default, items, options);
    gallery.init();
    gallery.listen('close', () => {
      document.querySelectorAll('.pswp__video').forEach((element: HTMLVideoElement) => {
        element.pause();
      });
    })
  }
}
