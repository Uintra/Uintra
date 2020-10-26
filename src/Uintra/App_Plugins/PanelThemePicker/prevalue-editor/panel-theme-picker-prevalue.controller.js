(function (angular) {
    'use strict';

    const controller = function ($scope) {
        const self = this;

        const defaultColor = '#000000';
        self.textColor = defaultColor;
        self.backgroundColor = defaultColor;
        self.titleColor = defaultColor;
        self.buttonColor = defaultColor;

        $scope.model.value = $scope.model.value || [];

        self.remove = function (item, evt) {
            evt.preventDefault();

            $scope.model.value = _.reject($scope.model.value, function (mItem) {
                return mItem.$$hashKey === item.$$hashKey;
            });
        };

        self.add = function (evt) {
            evt.preventDefault();

            if (self.textColor && self.backgroundColor) {
                const colorModel = {
                    alias: self.alias,
                    textColor: self.textColor,
                    backgroundColor: self.backgroundColor,
                    titleColor: self.titleColor,
                    buttonColor: self.buttonColor
                };
                $scope.model.value.push(colorModel);
            }
        };

        $scope.sortableOptions = {
            axis: 'y',
            containment: 'parent',
            cursor: 'move',
            items: '> div.control-group',
            tolerance: 'pointer',
            update: function (e, ui) {
                const newIndex = ui.item.index();
                const originalIndex = ui.item[0].querySelector('.js-index').textContent;

                const movedElement = $scope.model.value[originalIndex];
                $scope.model.value.splice(originalIndex, 1);
                $scope.model.value.splice(newIndex, 0, movedElement);
            }
        };
    }

    controller.$inject = ["$scope"];

    angular.module("umbraco").controller("panelThemePickerPreValueController", controller);
})(angular);