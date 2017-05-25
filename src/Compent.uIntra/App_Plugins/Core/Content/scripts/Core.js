if (!window.Promise) {
    window.Promise = require('promise-polyfill');
}

﻿import showContent from "./ShowContent"
import {} from "./Polyfill";
document.addEventListener('DOMContentLoaded', showContent, false);