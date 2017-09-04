(function () {
    'use strict';

    var service = function ($http) {
        var self = this;
        var baseUrl = "/Umbraco/backoffice/Api/IntranetUser/";

        self.getAll = function () {
            return $http.get(baseUrl + 'GetAll');
        }
    }

    service.$inject = ["$http"];
    angular.module('umbraco').service('intranetUserService', service);
})();