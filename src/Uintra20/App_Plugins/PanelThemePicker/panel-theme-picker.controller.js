(function (angular) {
    'use strict';

    const controller = function ($scope) {
        const self = this;

        self.onSelect = function (color) {
            $scope.model.value = color;
        }

        self.isActive = function (color) {
            function isEqual(a, b) {
                return a.toLowerCase() === b.toLowerCase();
            }

            if ($scope.model.value) {
                return isEqual($scope.model.value.backgroundColor, color.backgroundColor) &&
                    isEqual($scope.model.value.textColor, color.textColor) &&
                    isEqual($scope.model.value.titleColor, color.titleColor) &&
                    isEqual($scope.model.value.buttonColor, color.buttonColor);
            }

            return false;
        };

        self.getStyle = function (color) {
            const result = {
                'background-color': color.backgroundColor,
                'color': color.textColor
            };

            return result;
        }

        function getSelectedOrDefaultColor() {
            var result;

            if ($scope.model.value) {
                result = $scope.model.value;
            } else if ($scope.model.config.settings && $scope.model.config.settings.length > 0) {
                result = $scope.model.config.settings[0];
            } else {
                result = {};
            }

            return result;
        }

        function init() {
            $scope.model.value = getSelectedOrDefaultColor();
        }

        init();
    }

    controller.$inject = ["$scope"];

    angular.module("umbraco").controller("panelThemePickerController", controller);
})(angular);