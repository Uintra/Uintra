(function (angular) {
    'use strict';

    var uNavigationController = function ($scope, uNavigationService) {
        var self = this;

        self.isLoaded = false;
        self.isDocumentTypesAlreadyExists = undefined;
        self.folderId = "";

        self.create = function () {
            console.log("create");
            uNavigationService.createNavigationCompositions({ folderId: self.folderId });
        }

        function init() {
            uNavigationService.getInitialState().then(function (response) {
                self.isLoaded = true;
                self.isDocumentTypesAlreadyExists = response.isDocumentTypesAlreadyExists;
            });
        }

        init();
    }

    uNavigationController.$inject = ["$scope", "uNavigationService"];
    angular.module('umbraco').controller('uNavigationController', uNavigationController);
})(angular);