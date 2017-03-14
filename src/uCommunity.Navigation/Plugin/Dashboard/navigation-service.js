(function (angular) {
    'use strict';

    var uNavigationService = function ($http) {
        var self = this;
        var baseUrl = "/umbraco/backoffice/api/uNavigationApi/";

        self.createNavigationCompositions = function (data) {
            return $http.post(baseUrl + "createNavigationCompositions", data);
        }

        self.getInitialState = function () {
            return $http.get(baseUrl + "getInitialState");
        }
    }

    uNavigationService.$inject = ["$http"];
    angular.module('umbraco').service('uNavigationService', uNavigationService);
})(angular);