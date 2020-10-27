(function (angular) {
    'use strict';

    var service = function ($http) {
        var self = this;
        var baseUrl = "/ubaseline/api/UserTagsApi/";

        self.getAll = function (pageId) {
            return $http.get(baseUrl + "GetAll?pageId=" + pageId);
        };
    };

    service.$inject = ["$http"];
    angular.module('umbraco').service('usersTagsService', service);
})(angular);