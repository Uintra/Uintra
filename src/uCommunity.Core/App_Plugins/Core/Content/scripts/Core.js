import initAnchorScroll from "./AnchorScroll"
import showContent from "./ShowContent"

window.$ = window.jQuery = require('jquery');
initAnchorScroll();

document.addEventListener('DOMContentLoaded', showContent, false);