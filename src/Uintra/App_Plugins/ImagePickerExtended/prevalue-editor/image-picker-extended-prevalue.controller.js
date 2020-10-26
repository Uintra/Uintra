(function (angular) {
    'use strict';

    const controller = function ($scope, notificationsService, $filter) {
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
        else if ($scope.model.value.crops) {
            $scope.model.value.crops = sortCrops($scope.model.value.crops);
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

            $scope.model.value.crops = sortCrops($scope.model.value.crops);

            self.newItem = createNewItem();
            self.hasError = false;
        };

        function sortCrops(crops) {
            return $filter('orderBy')(crops, function (i) {
                return i.width * i.height;
            }, true);
        }

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
    }

    controller.$inject = ["$scope", "notificationsService", "$filter"];

    angular.module("umbraco").controller("imagePickerExtendedPreValueController", controller);
})(angular);