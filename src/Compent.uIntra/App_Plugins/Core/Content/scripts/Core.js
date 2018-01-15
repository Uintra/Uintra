if (!window.Promise) {
    window.Promise = require('promise-polyfill');
}

import {} from './Polyfill';
import {} from './QuillExtender';
import {uIntra} from './Namespace';
import showContent from './ShowContent';
import blockOnSubmit from './BlockOnSubmit';
import anchorScroll from './AnchorScroll';
import validationExtensions from './ValidationExtensions';
import scrollToTop from './ScrollToTop';
import lightboxGallery from '../../Controls/LightboxGallery/LightboxGallery';


export default function() {
    anchorScroll();
    blockOnSubmit();
    validationExtensions();
    scrollToTop();
    lightboxGallery.init();
    showContent();
}