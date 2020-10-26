(function (angular) {
    'use strict';

    const controller = function ($scope) {
        const self = $scope;

        self.newItem = {
            value: "",
            key: ""
        };
        self.hasError = false;

        if (!self.items) {
            self.items = [];
        }

        self.remove = function (item) {
            self.items = _.reject(self.items, function (modelItem) {
                return modelItem.key === item.key;
            });
        };

        function validate(item) {
            if (!item.key || !item.value) {
                self.hasError = true;
                notificationsService.error("Error", "Either key or value is empty!");
                return false;
            }

            if (_.findWhere(self.items, { key: item.key })) {
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

            self.items.push({ value: self.newItem.value, key: self.newItem.key });
            self.newItem = {
                value: "",
                key: ""
            };
            self.hasError = false;
        };

        function getElementIndexByPrevalueText(value) {
            for (var i = 0; i < self.items.length; i++) {
                if (self.items[i].value === value) {
                    return i;
                }
            }

            return -1;
        }

        self.sortableOptions = {
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
                    var movedElement = self.items[originalIndex];
                    self.items.splice(originalIndex, 1);
                    self.items.splice(newIndex, 0, movedElement);
                }
            }
        };
    };

    const directive = function () {
        return {
            restrict: 'E',
            translcude: true,
            scope: {
                items: "="
            },
            templateUrl: '/App_Plugins/Shared/directives/key-value-dropdown-list/key-value-dropdown-list.directive.html',
            controller: controller
        }
    };

    controller.$inject = [
        '$scope'
    ];

    angular.module("umbraco").directive('keyValueDropdownList', directive);
})(angular);