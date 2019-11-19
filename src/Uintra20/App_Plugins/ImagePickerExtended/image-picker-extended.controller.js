(function (angular) {
    'use strict';

    const localCache = {
        get: function (key) { return this[key]; },
        set: function (key, value) { this[key] = value; }
    }

    const controller = function ($scope, editorService, mediaResource, notificationsService) {
        const self = this;
        const defaultMedia = { src: "" };

        self.getAllImages = function () {
            if (angular.isArray($scope.model.value.image)) {
                return $scope.model.value.image;
            } else {
                var result = $scope.model.value.image === null ? [] : [$scope.model.value.image];
                if (!$scope.model.value.image) {
                    $scope.model.value = null;
                }
                return result;
            }
        }

        self.addImage = function () {
            editorService.mediaPicker({
                multiPicker: $scope.config.multiMode,
                disableFolderSelect: true,
                onlyImages: !$scope.config.showSvg,
                startNodeId: -1,
                submit: function (model) {
                    if (angular.isArray(model.selection)) {
                        model.selection.forEach(addNewItem);
                        removeDefaultItem();
                    }
                    else {
                        addNewItem(model.selection);
                    }
                    editorService.close();
                },
                close: function () {
                    editorService.close();
                }
            });
        }

        self.removeItem = function (index) {
            if (angular.isArray($scope.model.value.image)) {
                $scope.model.value.image.splice(index, 1);

                if ($scope.model.value.image.length === 0) $scope.model.value.image = null;

            } else {
                $scope.model.value.image = null;
            }
        }

        self.getCachedMedia = function (mediaId) {
            if (!mediaId) return defaultMedia;

            return localCache.get(mediaId);
        }

        self.editCrops = function (media, index) {
            $scope.editCropOverlay = {
                view: "/App_Plugins/ImagePickerExtended/overlay/image-picker-extended-overlay.html",
                title: "Edit crops",
                image: angular.copy(media),
                imageSrc: self.getCachedMedia(media.mediaId).src,
                show: true,
                submit: (model) => {
                    const image = model.image;

                    $scope.config.multiMode ? $scope.model.value.image.splice(index, 1, image) : $scope.model.value.image = model.image;
                    $scope.editCropOverlay.show = false;
                },
                close: () => $scope.editCropOverlay.show = false
            };
        }

        self.showImagePreview = function (mediaId) {
            const media = getCachedMedia(mediaId);
            if (!media) return false;

            const extention = media.src.split('.')[1];
            return $scope.config.imageExtentions.indexOf(extention) > -1;
        }

        self.isSvg = function (image) {
            var file = self.getCachedMedia(image.mediaId);
            return file && file.src.split('.')[1] === 'svg';
        }

        self.initAlt = function (image) {
            if (image.alt)
                return;

            mediaResource.getById(image.mediaId).then(media => {
                let alt = getAlt(media);
                image.alt = alt;
            });
        }

        function getAlt(media) {
            if (!media || !media.tabs)
                return '';

            for (let i = 0; i < media.tabs.length; i++) {
                let properties = media.tabs[i].properties;
                if (!properties) {
                    continue;
                }
                for (let j = 0; j < media.tabs[i].properties.length; j++) {
                    if (media.tabs[i].properties[j].alias === 'alt') {
                        return media.tabs[i].properties[j].value;
                    }
                }
            }
            return '';
        }

        function getImageExtentions() {
            const extentions = Umbraco.Sys.ServerVariables.umbracoSettings.imageFileTypes.split(',');
            if ($scope.config.showSvg) {
                extentions.push('svg');
            }

            return extentions;
        }

        function addNewItem(content) {
            if (!$scope.model.value) {
                $scope.model.value = { image: [] };
            }
            if (isImageSelected(content)) return;

            const src = content.image;
            const extention = src.split('.')[1];

            if ($scope.config.imageExtentions.indexOf(extention) === -1) {
                notificationsService.error("File type failed", "Please choose only: " + $scope.config.imageExtentions.join(','));
                return;
            }

            var media = self.getCachedMedia(content.id);
            let altImage = getAlt(media);
            const newModel = { mediaId: content.id, alt: altImage };

            if (extention !== 'svg' && $scope.config.imageExtentions.indexOf(extention) > -1) {
                newModel.crops = angular.copy($scope.config.crops);
                newModel.focalPoint = angular.copy($scope.config.focalPoint);
            }

            if ($scope.config.multiMode) {
                angular.isArray($scope.model.value.image) ? angular.noop() : $scope.model.value.image = [];
                $scope.model.value.image.push(newModel);
            } else {
                $scope.model.value.image = newModel;
            }

            fillMediaContentCache(newModel.mediaId);
        }

        function processConfig() {
            $scope.config = $scope.model.config.imageSettings;
            $scope.config.imageExtentions = getImageExtentions();
        }

        function fillMediaContentCache(mediaId) {
            if (!mediaId) return defaultMedia;

            mediaResource.getById(mediaId).then(data => {
                data.src = data.mediaLink;
                localCache.set(mediaId, data);
            });
        }

        function isImageSelected(newMedia) {
            if (!$scope.model.value.image) return false;

            if (angular.isArray($scope.model.value)) {
                var item = _.find($scope.model.value.images, item => item.mediaId === newMedia.id);

                return angular.isDefined(item);
            }

            return $scope.model.value.image.mediaId === newMedia.id;
        }

        function removeDefaultItem() {
            if (angular.isArray($scope.model.value.image)) {
                $scope.model.value.image = $scope.model.value.image.filter(item => {
                    return angular.isDefined(item.mediaId) && item.mediaId !== null && item.mediaId !== 0;
                });
            }
        }

        function getDefaultControlModel() {
            if ($scope.model.validation.mandatory) {
                return null;
            }

            const model = {
                crops: angular.copy($scope.config.crops),
                focalPoint: angular.copy($scope.config.focalPoint)
            };

            let result;
            if ($scope.config.multiMode) {
                result = [model];
            }
            else {
                result = model;
            }

            return angular.copy(result);
        }

        function updateCropSettings(item) {
            if (item) {

                var cropsClone = item.crops.slice(0);
                var cropsHaveSomeCoordinates = cropsClone.filter(x => x.coordinates != null).length > 0;

                item.crops = angular.copy($scope.config.crops);
                item.focalPoint = angular.copy($scope.config.focalPoint);

                if (cropsHaveSomeCoordinates) {
                    angular.forEach(item.crops, function (crop) {
                        crop.coordinates = cropsClone.find(function (el) {
                            return el.alias === crop.alias;
                        }).coordinates;
                    });
                }
            }
        }

        self.getGlobalState = function () {
            if ($scope.config.multiMode) return true;

            // We have an image that isn't an array and it has mediaId
            if ($scope.model.value && $scope.model.value.image && $scope.model.value.image.mediaId) return false;

            return true;
        }

        function init() {
            processConfig();

            // Important $scope.model.value may has a video property.
            // Don't make $scope.model.value as null the space of this controller has to be $scope.model.value.image
            if (angular.isString($scope.model.value) || $scope.model.value === null || $scope.model.value.image === null)
                return $scope.model.value = { image: null };

            if (angular.isArray($scope.model.value.image))
                $scope.model.value.image.forEach(item => fillMediaContentCache(item.mediaId));

            // Here image is defined and isn't an array.
            fillMediaContentCache($scope.model.value.image.mediaId);
        }

        init();
    }

    controller.$inject = ["$scope", "editorService", "mediaResource", "notificationsService"];

    angular.module("umbraco").controller("imagePickerExtendedController", controller);
    angular.module("umbraco").directive("ublImagePicker", () => {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/ImagePickerExtended/image-picker-extended.html'
        }
    });
})(angular);