(function (angular) {
    'use strict';

    const controller = function ($scope, notificationsService) {
        const self = this;

        const defaultItem = {
            name: "",
            alias: "",
            width: "",
            height: "",
            media: ""
        };

        self.newItem = createNewItem();

        self.hasError = false;
        if (!$scope.model.value) {
            $scope.model.value = {
                imageTypeAlias: "Image",
                mediaFolderId: null,
                crops: [],
                focalPoint: {
                    left: 0.5,
                    top: 0.5
                },
                multiMode: false
            };
        }

        self.remove = function (item) {
            $scope.model.value.crops = _.reject($scope.model.value.crops, function (modelItem) {
                return modelItem.width === item.width && modelItem.height === item.height;
            });
        };

        function validate(item) {
            if (!item.width || !item.height || !item.name || !item.alias) {
                self.hasError = true;
                notificationsService.error("Error", "Either crop size or name or alias is empty!");
                return false;
            }

            if (!angular.isNumber(item.width) ||
                !angular.isNumber(item.height) ||
                item.width <= 0 ||
                item.height <= 0) {
                self.hasError = true;
                notificationsService.error("Error", "The Crop size is incorrect!");
                return false;
            }

            if (_.find($scope.model.value.crops, function (crop) { return crop.width === item.width && crop.height === item.height })) {
                self.hasError = true;
                notificationsService.error("Error", "The Crop size is duplicated!");
                return false;
            }

            if (_.findWhere($scope.model.value.crops, { name: item.name })) {
                self.hasError = true;
                notificationsService.error("Error", "The Name is duplicated!");
                return false;
            }
            return true;
        }

        self.add = function () {
            if (!validate(self.newItem)) {
                return;
            }

            $scope.model.value.crops.push({
                name: self.newItem.name,
                alias: self.newItem.alias,
                width: self.newItem.width,
                height: self.newItem.height,
                media: self.newItem.media
            });

            self.newItem = createNewItem();
            self.hasError = false;
        };

        function getElementIndexByPrevalueText(name) {
            for (var i = 0; i < $scope.model.value.crops.length; i++) {
                if ($scope.model.value.crops[i].name === name) {
                    return i;
                }
            }

            return -1;
        }

        function createNewItem() {
            return Object.assign({}, defaultItem);
        }

        $scope.sortableOptions = {
            axis: 'y',
            containment: 'parent',
            cursor: 'move',
            items: '> div.control-group',
            tolerance: 'pointer',
            update: function (e, ui) {
                const newIndex = ui.item.index();
                const movedPrevalueText = $('input[type="text"]', ui.item).val();
                const originalIndex = getElementIndexByPrevalueText(movedPrevalueText);

                if (originalIndex > -1) {
                    const movedElement = $scope.model.value.crops[originalIndex];
                    $scope.model.value.crops.splice(originalIndex, 1);
                    $scope.model.value.crops.splice(newIndex, 0, movedElement);
                }
            }
        };
    }

    controller.$inject = ["$scope", "notificationsService"];

    angular.module("umbraco").controller("imagePickerExtendedPreValueController", controller);
})(angular);