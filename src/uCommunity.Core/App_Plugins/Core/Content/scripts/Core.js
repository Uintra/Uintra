if (!window.Promise) {
    window.Promise = require('promise-polyfill');
}

import {} from "./Polyfill";
import initAnchorScroll from "./AnchorScroll"

initAnchorScroll();