if (!window.Promise) {
    window.Promise = require('promise-polyfill');
}

﻿import showContent from "./ShowContent"
import {} from "./Polyfill";
import initTimezoneOffsetSetter from "./TimezoneOffsetSetter";
document.addEventListener('DOMContentLoaded', showContent, false);