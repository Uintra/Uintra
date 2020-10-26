(function () {
    'use strict';

    function umbracoStructureManagementController($scope, notificationsService, umbracoStructureManagementService) {

        var scope = this;

        scope.changes = {};
        scope.working = true;

        init();

        function init() {
            umbracoStructureManagementService.get()
                .then(function (result) {
                    scope.working = false;
                    scope.changes = result.data;
                }, function (error) {
                    notificationsService.error('Reporting', error.data.Message);
                });
        }

        scope.manage = function () {
            scope.working = true;

            umbracoStructureManagementService.manage(scope.changes)
                .then(function (result) {
                    scope.changes = result.data;
                }, function (error) {
                    notificationsService.error('Manage changes', error.data.Message);
                }).then(function () {
                    scope.working = false;
                });
        };
    };

    angular.module('umbraco').controller('umbracoStructureManagementController', umbracoStructureManagementController);
})();