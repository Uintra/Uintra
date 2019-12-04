(function (angular) {
    'use strict';

    const service = function ($http) {
        const self = this;
        const baseUrl = "/api/Central/Feed/";

        self.getAvailableActivityTypes = function (culture) {
            return $http.get(baseUrl + "availableTypes");
        }
    };

    service.$inject = ["$http"];
    angular.module('umbraco').service('availableActivityService', service);

})(angular)