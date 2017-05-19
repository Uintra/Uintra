if (!window.Promise) {
    window.Promise = require('promise-polyfill');
}

﻿import showContent from "./ShowContent"
import {} from "./Polyfill";
import initAnchorScroll from "./AnchorScroll"

initAnchorScroll();
document.addEventListener('DOMContentLoaded', showContent, false);