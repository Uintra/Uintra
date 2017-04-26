(function (angular) {
    'use strict';

    var uNavigationController = function ($scope, uNavigationService) {
        var self = this;

        self.isLoaded = false;
        self.isDocumentTypesAlreadyExists = undefined;
        self.parentIdOrAlias = "";

        self.create = function () {
            uNavigationService.createNavigationCompositions({ parentIdOrAlias: self.parentIdOrAlias }).then(function (response) {
                self.isDocumentTypesAlreadyExists = response.data.isDocumentTypesAlreadyExists;

                if (response.data.isUnknownParent) {
                    alert("Unknown parent id or alias");
                }
            });
        }

        self.delete = function () {
            uNavigationService.deleteNavigationCompositions().then(function (response) {
                self.isDocumentTypesAlreadyExists = response.data.isDocumentTypesAlreadyExists;
            });
        }

        function init() {
            uNavigationService.getInitialState().then(function (response) {
                self.isLoaded = true;
                self.isDocumentTypesAlreadyExists = response.data.isDocumentTypesAlreadyExists;
            });
        }

        init();
    }

    uNavigationController.$inject = ["$scope", "uNavigationService"];
    angular.module('umbraco').controller('uNavigationController', uNavigationController);
})(angular);