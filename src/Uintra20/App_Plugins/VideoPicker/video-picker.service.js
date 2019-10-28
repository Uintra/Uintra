(function(angular) {
	'use strict';

    const service = function($http) {
        const self = this;
        const baseUrl = "/umbraco/backoffice/api/VideoPickerApi/";

        self.getThumbnailUrl = function(code, type) {
            return $http.get(baseUrl + "getThumbnailUrl?code=" + code + "&type=" + type);
        }
    };

    service.$inject = ["$http"];
    angular.module('umbraco').service('videoPickerService', service);

})(angular)