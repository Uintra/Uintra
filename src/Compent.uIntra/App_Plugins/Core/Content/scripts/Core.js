if (!window.Promise) {
    window.Promise = require('promise-polyfill');
}

﻿import showContent from "./ShowContent"
﻿import initBlockOnSubmit from "./BlockOnSubmit"
initBlockOnSubmit();
import {} from "./Polyfill";
document.addEventListener('DOMContentLoaded', showContent, false);