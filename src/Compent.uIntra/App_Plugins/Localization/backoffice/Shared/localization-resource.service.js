(function (angular) {
    'use strict';

    var service = function ($http) {
        var self = this;
        var baseUrl = "/umbraco/backoffice/api/LocalizationSectionApi/";

        self.getLocalizationOverview = function () {
            return $http.get(baseUrl + "GetLocalizationOverview");
        }

        self.saveResource = function (key, data) {
            return $http.put(baseUrl + "SaveResource?key=" + key, data);
        }

        self.createResource = function (data) {
            return $http.post(baseUrl + "CreateResource", data);
        }

        self.deleteResource = function (key) {
            return $http.delete(baseUrl + "DeleteResource?key=" + key);
        }

        self.importResources = function (data) {
            return $http.post(baseUrl + "Import", data);
        }
    }

    service.$inject = ["$http"];
    angular.module('umbraco').service('localizationResourceService', service);
})(angular);