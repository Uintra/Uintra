require('select2');
require('./style.css');

import createEvents from './Create/create-events';
import editEvents from './Edit/edit-events';

export default function () {
    createEvents.init();
    editEvents.init();
}