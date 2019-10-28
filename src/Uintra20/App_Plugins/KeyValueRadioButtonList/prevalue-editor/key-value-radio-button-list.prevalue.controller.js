(function (angular) {
    'use strict';

    var controller = function ($scope, notificationsService) {
        var self = this;

        self.newItem = {
            value: "",
            key: ""
        };
        self.hasError = false;

        if (!$scope.model.value) {
            $scope.model.value = {
                default: "",
                items: []
            };
        }

        self.remove = function (item) {
            $scope.model.value.items = _.reject($scope.model.value.items, function (modelItem) {
                return modelItem.key === item.key;
            });
        };

        function validate(item) {
            if (!item.key || !item.value) {
                self.hasError = true;
                notificationsService.error("Error", "Either key or value is empty!");
                return false;
            }

            if (_.findWhere($scope.model.value.items, { key: item.key })) {
                self.hasError = true;
                notificationsService.error("Error", "The Key is duplicated!");
                return false;
            }

            return true;
        }

        self.add = function () {
            if (!validate(self.newItem)) {
                return;
            }

            $scope.model.value.items.push({ value: self.newItem.value, key: self.newItem.key });
            self.newItem = {
                value: "",
                key: ""
            };
            self.hasError = false;
        };

        function getElementIndexByPrevalueText(value) {
            for (var i = 0; i < $scope.model.value.items.length; i++) {
                if ($scope.model.value.items[i].value === value) {
                    return i;
                }
            }

            return -1;
        }

        $scope.sortableOptions = {
            axis: 'y',
            containment: 'parent',
            cursor: 'move',
            items: '> div.control-group',
            tolerance: 'pointer',
            update: function (e, ui) {
                var newIndex = ui.item.index();
                var movedPrevalueText = $('input[type="text"]', ui.item).val();
                var originalIndex = getElementIndexByPrevalueText(movedPrevalueText);

                if (originalIndex > -1) {
                    var movedElement = $scope.model.value.items[originalIndex];
                    $scope.model.value.items.splice(originalIndex, 1);
                    $scope.model.value.items.splice(newIndex, 0, movedElement);
                }
            }
        };
    }

    controller.$inject = ["$scope", "notificationsService"];

    angular.module("umbraco").controller("keyValueRadioButtonListPreValueController", controller);
})(angular);