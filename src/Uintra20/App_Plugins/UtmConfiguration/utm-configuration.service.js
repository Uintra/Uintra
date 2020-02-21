(function(angular) {
	'use strict';

    const service = function($http) {
        const self = this;
        const baseUrl = "/umbraco/backoffice/api/utmConfigurationApi/";

        self.getUtmParameters = function() {
            return $http.get(baseUrl + "getUtmParameters");
        };
    };

    service.$inject = ["$http"];
    angular.module('umbraco').service('utmConfigurationService', service);

})(angular)