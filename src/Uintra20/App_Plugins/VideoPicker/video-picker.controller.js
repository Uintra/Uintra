(function (angular) {
    'use strict';

    const controller = function ($scope) {
        var self = this;
       
        init();

        function init() 
        {
            if (angular.isString($scope.model.value))
            {
                $scope.model.value = {video: null};
            }
            if ($scope.model.value && !$scope.model.value.video)
            {
                $scope.model.value.video = JSON.parse(angular.toJson({
                    desktop: null,
                    mobile: null
                }))
            }
        }
    };

    controller.$inject = ["$scope", "$location", "editorService", "videoPickerService"];

    angular.module("umbraco").controller("videoPickerController", controller);

    angular.module("umbraco").directive("ublVideoPicker", () => {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/VideoPicker/video-picker.html'
        }
    });

    angular.module("umbraco").directive("ublVideoConfigurator", () => {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/VideoPicker/ubl-video-configurator.html',
        }
    });
    angular.module("umbraco").directive("ublVideoThumbnail", () => {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/VideoPicker/ubl-video-thumbnail.html',
        }
    });

    angular.module("umbraco").directive("ublVideoSection", () => {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/VideoPicker/ubl-video-section.html',
            scope: {
                model: '=',
                settings: '=',
                mandatory: '='
            },
            controller: ['$scope', '$location', 'editorService', 'videoPickerService','contentResource', videoSectionDirectiveController],
            controllerAs: 'vm'
        }
    });

    function videoSectionDirectiveController ($scope, $location, editorService, videoPickerService, contentResource) {
        const self = this;      
        
        init(); 

        self.addVideo = function () {
            editorService.mediaPicker({
                multiPicker: false,
                onlyImages: false,
                startNodeId: -1,
                submit: function (model) {
                    var content = model.selection[0];
                    if (content.isFolder) {
                        alert("Folders are disallowed!");
                        return;
                    }

                    const extensionUmbracoItem = getByAlias(content.properties, "umbracoExtension");
                    const extension = extensionUmbracoItem ? extensionUmbracoItem.value : null;

                    const fileExtension = content.extension;

                    if (extension !== "mp4" && fileExtension !== "mp4") {
                        alert("Only mp4 video is allowed!");
                        return;
                    }

                    $scope.model.video.id = content.id;
                    $scope.model.video.name = content.name;
                    editorService.close();
                },
                close: function () {
                    editorService.close();
                }
            });
        }

        self.editMedia = function (id) {
            $location.path('media/media/edit/' + id);
        }

        self.showAddVideo = function () {
            return !$scope.model.video.id;
        }

        self.isMediaSelected = function () {
            return $scope.model.video.id;
        }

        self.removeVideo = function () {
            $scope.model.video = getVideoDefault();
        }

        self.getVideoTypeName = function (videoTypeName) 
        {
            return self.umbraco.text === videoTypeName ? "Media" : videoTypeName;
        }

        self.isThumbnailLoading = false;

        self.loadDefaultThumbnail = function () {

            function onSuccess(successResponse) {
                $scope.model.thumbnail.url = successResponse.data;
                $scope.model.thumbnail.id = "";
                self.isThumbnailLoading = false;
            }

            function onError(errorResponse) {
                self.isThumbnailLoading = false;
                $scope.model.video.thumbnail.url = "";
                var error = {
                    errorMsg: "Incorrect video source data",
                    data: {
                        ExceptionMessage: errorResponse.data.exceptionMessage,
                        Message: errorResponse.data.message,
                        ExceptionType: errorResponse.data.exceptionType,
                        StackTrace: errorResponse.data.stackTrace
                    }
                };
            }

            if (!isTypeEmpty() && !isCodeEmpty()) {
                self.isThumbnailLoading = true;
                videoPickerService.getThumbnailUrl($scope.model.video.code, $scope.model.type).then(onSuccess, onError);
            }
        }

        self.changeVideoType = function () 
        {
            $scope.model.video = getVideoDefault();
            $scope.model.thumbnail = getThumbnailDefault();    
        }

        self.hasVideoType = function () {
            return Number.isInteger($scope.model.type);
        }

        self.disabledLoadDefaultThumbnail = function () {
            const umbracoType = self.umbraco.value === $scope.model.type;

            return isTypeEmpty() || isCodeEmpty() || umbracoType;
        }

        self.loadThumbnail = function () {
            self.isThumbnailLoading = true;

            editorService.mediaPicker({
                multiPicker: false,
                onlyImages: true,
                startNodeId: -1,
                submit: function (model) {
                    const media = model.selection[0];

                    $scope.model.thumbnail.url = media.image;
                    $scope.model.thumbnail.id = media.id;
                    self.isThumbnailLoading = false;

                    editorService.close();
                },
                close: function () {
                    editorService.close();
                }
            });
        }

        self.disabledLoadThumbnail = function () {
            const mediaIdEmpty = !$scope.model.video.id;

            const result = isTypeEmpty() || (isCodeEmpty() && mediaIdEmpty);
            return result;
        }

        self.clearThumbnail = function () {
            $scope.model.thumbnail = getThumbnailDefault();
        }

        self.disabledClearThumbnail = function () {
            const defaultThumbnailEmpty = !$scope.model.thumbnail;
            const thumbnailLoaded = !self.isThumbnailLoading;

            return defaultThumbnailEmpty && thumbnailLoaded;
        }
        self.getTipByType = (type) => {
            let _type = self.videoTypes.find(t => t.value === type);
            return _type && _type.tip || "";
        }

        function init() {
            self.youtube = { value: 0, text: "Youtube", sortOrder: 1 , tip: 'Enter only video code. Example r0haWB1Uezk'};
            self.vimeo = { value: 1, text: "Vimeo", sortOrder: 2, tip: 'Enter only video code. Example 66656697' };
            self.umbraco = { value: 2, text: "Umbraco", sortOrder: 4, tip: 'Supporting formats: .mp4' };
            self.twentyThree = { value: 3, text: "TwentyThree", sortOrder: 3, tip: 'For example https://video.twentythree.net/marketing-expert-series-scott-brinker' };
            self.videoTypes = [self.youtube, self.vimeo, self.umbraco, self.twentyThree];

            self.settings = $scope.settings;
            self.videoRequired = $scope.mandatory;

            if (!$scope.model) 
            {
                $scope.model = getInitialValue();
            }
        }

        function getVideoDefault() 
        {
            const config = {
                id: "",
                code: "",
                name: ""
            };

            return JSON.parse(JSON.stringify(config));
        }
        
        function getThumbnailDefault() {
            const config = {
                id: "",
                url: ""
            }

            return JSON.parse(JSON.stringify(config));
        }               

        function getInitialValue()
        {
            let config = {
                type: "",
                video: getVideoDefault(),
                thumbnail: getThumbnailDefault(),
                loop: true
            }

            return JSON.parse(angular.toJson(config))
        }

        function getByAlias(items, alias) {
            return _.findWhere(items, { alias: alias });
        }

        function isTypeEmpty() {
            return $scope.model.type === "";
        }

        function isCodeEmpty() {
            return !$scope.model.video.code;
        }
    } 
    
})(angular);