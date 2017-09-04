(function (angular) {
    'use strict';

    var controller = function (notificationsService, localizationResourceService) {
        var self = this;
        self.isImportVisible = false;

        self.importToggle = function () {
            self.isImportVisible = !self.isImportVisible;
        }

        self.import = function (resources) {
            // TODO: merge
            sendImport(resources);
        }

        function sendImport(resources) {
            localizationResourceService.importResources(resources).then(function (response) {
                self.isImportVisible = false;

                notificationsService.success("Success", "Resources were updated/imported successfully");
            }, function(response) {
                notificationsService.error("Error", "Resources were not updated/imported, because some error has occurred");
            });
        }
    }

    controller.$inject = ["notificationsService", "localizationResourceService"];

    angular.module('umbraco').controller('localizationExportImportController', controller);
})(angular);