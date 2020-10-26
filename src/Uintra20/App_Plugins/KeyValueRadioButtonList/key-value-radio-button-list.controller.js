(function (angular) {
    'use strict';

    var controller = function ($scope) {

        if (!$scope.model.value) {
            $scope.model.value = $scope.model.config.values.default;
        }
    }

    controller.$inject = ["$scope"];

    angular.module("umbraco").controller("keyValueRadioButtonListController", controller);
})(angular);