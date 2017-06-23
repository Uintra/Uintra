require('./style.css');

import appInitializer from "./../Core/Content/scripts/AppInitializer";
import createEvents from './Create/create-events';
import editEvents from './Edit/edit-events';

appInitializer.add(
    function () {
        createEvents.init();
        editEvents.init();
    }
);