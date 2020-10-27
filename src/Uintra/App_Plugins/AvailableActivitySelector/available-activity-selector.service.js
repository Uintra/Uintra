(function (angular) {
    'use strict';

    const service = function ($http) {
        const self = this;
        const baseUrl = "/ubaseline/api/CentralFeedApi/";

        self.getAvailableActivityTypes = function (culture) {
            return $http.get(baseUrl + "AvailableActivityTypes");
        }
    };

    service.$inject = ["$http"];
    angular.module('umbraco').service('availableActivityService', service);

})(angular)