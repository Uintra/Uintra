(function () {
    'use strict';

    var groupCentralFeedService = function ($http) {
        var self = this;
        var baseUrl = "/umbraco/surface/groupFeed/";

        self.getActivityTypes = function () {
            return $http.get(baseUrl + 'AvailableActvityTypes');
        }
    }

    groupCentralFeedService.$inject = ["$http"];
    angular.module('umbraco').service('groupCentralFeedService', groupCentralFeedService);
})();