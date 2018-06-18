(function () {
    'use strict';

    var centralFeedService = function ($http) {
        var self = this;
        var baseUrl = "/umbraco/surface/centralFeed/";

        self.getActivityTypes = function () {
            return $http.get(baseUrl + 'AvailableActivityTypes');
        }
    }

    centralFeedService.$inject = ["$http"];
    angular.module('umbraco').service('centralFeedService', centralFeedService);
})();