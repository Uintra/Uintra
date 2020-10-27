(function (angular) {
    'use strict';

    var imageExtensions = ["jpeg", "jpg", "gif", "bmp", "png", "tiff", "tif", "svg"];
    var svgExtension = ['svg'];

    function checkIsSingleMode(mode) {
        var modes = ['single', 'multiple'];
        var validMode = modes.indexOf(mode) > -1;
        if (!validMode) {
            throw new Error('MultiImageCropper: Mode "' + mode + '" is not allowed! Allowed modes: ' + modes.join(', '));
        }
        return mode === modes[0];
    }

    var factory = function (dialogService) {
        var link = function ($scope) {
            var isSignleMode = checkIsSingleMode($scope.mode);

            var createNewValueObject = function (mediaContent) {
                var item = {
                    mediaId: mediaContent.id,
                    name: mediaContent.name,
                    src: mediaContent.image
                }

                if ($scope.isImage(item.src)) {
                    item.crops = angular.copy($scope.config.crops);
                    item.focalPoint = angular.copy($scope.config.focalPoint);
                }

                return item;
            }

            var addNewItem = function (mediaContent) {
                if (isSignleMode) {
                    $scope.model = createNewValueObject(mediaContent);
                } else {
                    angular.isArray($scope.model) ? angular.noop() : $scope.model = [];
                    $scope.model.push(createNewValueObject(mediaContent));
                }
            }

            $scope.isSvgImage = function(src) {
                if (!src) return false;
                var extention = src.split('.')[1];
                return svgExtension.indexOf(extention) > -1;
            }

            $scope.isImage = function (src) {
                if (!src) return false;
                var extention = src.split('.')[1];
                return imageExtensions.indexOf(extention) > -1;
            }

            $scope.noop = angular.noop;

            $scope.pickMedia = function () {
                dialogService.mediaPicker({
                    multiPicker: !isSignleMode,
                    startNodeId: $scope.config.mediaFolderId,
                    callback: function (content) {
                        angular.isArray(content) ? content.forEach(addNewItem) : addNewItem(content);
                    }
                });
            }

            $scope.removeItem = function (index) {
                isSignleMode ? $scope.model = null : $scope.model.splice(index, 1);
            }

            $scope.editCrop = function (media, index) {
                $scope.editCropOverlay = {
                    view: "/App_Plugins/BaseControls/MultiImageCropper/edit-crop-overlay.html",
                    title: "Crop edit",
                    image: angular.copy(media),
                    show: true,
                    submit: function (model) {
                        var image = model.image;
                        isSignleMode ? $scope.model = image : $scope.model.splice(index, 1, image);
                        $scope.editCropOverlay.show = false;
                    },
                    close: function () {
                        $scope.editCropOverlay.show = false;
                    }
                };
            }

            $scope.getRepeatableModel = function () {
                return angular.isArray($scope.model) ? $scope.model : [$scope.model];
            }

            $scope.getGlobalClassModel = function () {
                return {
                    '_single-mode': isSignleMode,
                    '_value-selected': $scope.model && (isSignleMode || $scope.model.length > 0)
                }
            }
        }

        return {
            scope: { model: '=', config: '=', mode: '@' },
            restrict: 'E',
            replace: false,
            templateUrl: '/App_Plugins/BaseControls/MultiImageCropper/multi-image-cropper.html',
            link: link
        }
    }
    factory.$inject = ["dialogService"];

    var maxCropsSizeFilterFactory = function (interpolate) {
        return function (crops) {
            if (!angular.isArray(crops)) return "";

            var sorted = crops.sort(function (a, b) {
                if (a.width < b.width) return -1;
                else if (a.width > b.width) return 1;
                else return 0;
            });

            var max = sorted[sorted.length - 1];
            return interpolate("{width}x{height}", { width: max.width, height: max.height });
        }
    }
    maxCropsSizeFilterFactory.$inject = ['interpolateFilter'];

    angular.module('umbraco').directive('multiImageCropper', factory);
    angular.module('umbraco').filter('maxCropSize', maxCropsSizeFilterFactory);
})(angular);