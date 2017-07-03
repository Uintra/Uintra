require('./../App_Plugins/Core/Content/Scripts/Core');
require('./../App_Plugins/Core/Content/Scripts/ScrollToTop');
require('./../App_Plugins/Navigation/navigation');
require('./../App_Plugins/Tagging/tags');
require('./../App_Plugins/News/news');
require('./../App_Plugins/CentralFeed/centralFeed');
require("./../App_Plugins/Comments/comment");
require("./../App_Plugins/Subscribe/subscribe");
require('./../App_Plugins/Subscribe/subscribeList');
require('./../App_Plugins/Events/events');
require('./../App_Plugins/Notification/notification');
require('./../App_Plugins/Likes/likes');
require('./../App_Plugins/Panels/ContentPanel/contentPanel');
require('./../App_Plugins/Users/users');
require('./../App_Plugins/Bulletins/bulletins');
require('../App_Plugins/Core/Content/scripts/ConfirmOnBeforeUnload');

import initSearch from './../App_Plugins/Search/search';
import initActionLinkWithConfirm from "../App_Plugins/Core/Content/scripts/ActionLinkWithConfirm";

initSearch();
initActionLinkWithConfirm();