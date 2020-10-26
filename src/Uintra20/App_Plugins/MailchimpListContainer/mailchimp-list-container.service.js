(function(angular) {
	'use strict';

    const service = function($http) {
        const self = this;
        const baseUrl = "/umbraco/backoffice/api/mailchimpApi/";

        self.getInfo = function() {
            return $http.get(baseUrl + "getInfo");
        };
    };

    service.$inject = ["$http"];
    angular.module('umbraco').service('mailchimpListContainerService', service);

})(angular)