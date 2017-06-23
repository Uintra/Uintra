import appInitializer from "./../Core/Content/scripts/AppInitializer";
import topNavigation from './TopNavigation/topNavigation'
import subNavigation from './SubNavigation/subNavigation'
import leftNavigation from './LeftNavigation/leftNavigation'
import myLinks from "./MyLinks/myLinks";

function init() {
    topNavigation.init();
    subNavigation.init();
    leftNavigation.init();
    myLinks.init();
}

appInitializer.add(init);