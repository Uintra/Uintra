import topNavigation from './TopNavigation/topNavigation'
import subNavigation from './SubNavigation/subNavigation'
import leftNavigation from './LeftNavigation/leftNavigation'
import myLinks from "./MyLinks/myLinks";
import systemLinks from "./SystemLinks/systemLinks";

export default function(){
    topNavigation.init();
    subNavigation.init();
    leftNavigation.init();
    myLinks.init();
    systemLinks.init();
}