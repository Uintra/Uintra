﻿require('./../App_Plugins/Core/Content/Scripts/Core');
require('./../App_Plugins/Navigation/TopNavigation/topNavigation');
require('./../App_Plugins/Navigation/SubNavigation/subNavigation');
require('./../App_Plugins/Navigation/LeftNavigation/leftNavigation');
//require('./../App_Plugins/Comments/Comment');
import commentOverview from "./../App_Plugins/Comments/Comment";
window.CommentOverview = commentOverview;

require('./../App_Plugins/News/Create/create-news');
require('./../App_Plugins/News/Edit/edit-news');
require('./../App_Plugins/CentralFeed/CentralFeed');

import subscribe from "./../App_Plugins/Subscribe/Subscribe";
window.subscribe= subscribe;
require('./../App_Plugins/Subscribe/SubscribeList');
require('./../App_Plugins/Events/Create/create-events');
require('./../App_Plugins/Events/Edit/edit-events');
require('./../App_Plugins/Events/List/edit-list');
require('./../App_Plugins/Notification/Notification');