require('./style.css');

import appInitializer from "./../Core/Content/scripts/AppInitializer";

import bulletinsCreate from './Create/bulletins.create';
import bulletinsEdit from './Edit/bulletins.edit';

appInitializer.add(() => {
    bulletinsCreate.init();
    bulletinsEdit.init();
});