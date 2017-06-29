import appInitializer from "./../Core/Content/scripts/AppInitializer";
import tagsView from './tagsView';
import tagsEdit from './tagsEdit';

appInitializer.add(function () {
    tagsView.init();
    tagsEdit.init();
});