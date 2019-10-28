(function (angular) {
    'use strict';

    const controller = function ($scope) {
        const self = this;

        if ($scope.model.value == null) {
            $scope.model.value = {
                useThumbnail: true,
                showLoop: true,
                useMobileVideo: true
            }
        }
    }

    controller.$inject = ["$scope"];

    angular.module("umbraco").controller("videoPickerPreValueController", controller);
})(angular);