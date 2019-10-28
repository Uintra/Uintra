(function () {
    'use strict';

    function umbracoStructureManagementService($http) {

        var serviceRoot = "/umbraco/backoffice/umbracoStructureManagement/umbracoStructureManagementDashboardApi/";

        var service = {
            get: get,
            manage: manage
        };

        return service;

        function get() {
            return $http.get(serviceRoot + 'Get');
        }

        function manage(changes) {
            return $http.post(serviceRoot + 'Manage', changes);
        }
    }

    angular.module('umbraco.services')
        .factory('umbracoStructureManagementService', umbracoStructureManagementService);
})();