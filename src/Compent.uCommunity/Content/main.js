require('./../App_Plugins/Core/Content/Scripts/Core');

require('./../App_Plugins/Navigation/navigation');
require('./../App_Plugins/News/news');
require('./../App_Plugins/CentralFeed/centralFeed');

import commentOverview from "./../App_Plugins/Comments/comment";
import subscribe from "./../App_Plugins/Subscribe/subscribe";

window.subscribe = subscribe;
window.CommentOverview = commentOverview;

require('./../App_Plugins/Subscribe/subscribeList');
require('./../App_Plugins/Events/events');
require('./../App_Plugins/Notification/notification');
require('./../App_Plugins/Likes/likes');
require('./../App_Plugins/ContentPanel/contentPanel');