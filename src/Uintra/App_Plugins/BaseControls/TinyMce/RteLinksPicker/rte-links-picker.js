(function (angular) {
    'use strict';

    var defaultConfig = {
        defaultCaption: "",
        useAltText: false,
        types: [],
        //urlRegex: /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i,
        linkTypes: ["internal", "external", "email"], // internal | extenral | email | media
        linkTargets: [
            {
                name: "New window",
                value: "_blank"
            },
            {
                name: "This window",
                value: "_self"
            }
        ],
        internalPicker: {
            startNode: null,
            allowedAliases: []
        }
    };

    var mediaPickerFactory = function ($q, dialogService) {
        var pickContent = function () {
            var deferred = $q.defer();
            dialogService.mediaPicker({
                disableFolderSelect: true,
                multiPicker: false,
                filterCssClass: "not-allowed not-published",
                callback: deferred.resolve
            });
            return deferred.promise;
        };

        function map(target, content) {
            target.id = content.id;
            target.icon = content.icon;
            target.link = content.image;

            if (target.caption) {
                target.name = content.name;
            } else {
                target.caption = target.name = content.name;
            }
            return target;
        }

        return function (target) {
            return pickContent()
                .then(function (content) { return map(target, content) });
        }
    }

    var internalPickerFactory = function ($q, dialogService, contentResource, entityResource, config) {
        var getStartNodeId = function () {
            return entityResource.getByQuery(config.startNodeXPath, -1, "Document");
        }

        var pickContent = function (response) {
            var deferred = $q.defer();
            var startNodeId = config.startNode;
            if (response.id) {
                startNodeId = response.id;
            }

            dialogService.contentPicker({
                multiPicker: false,
                filterCssClass: "not-allowed not-published",
                filter: config.allowedAliases.join(','),
                startNodeId: startNodeId,
                callback: deferred.resolve
            });
            return deferred.promise;
        };

        var mapContentTo = function (target) {
            return function (content) {
                target.id = content.id;
                target.icon = content.icon;

                if (target.caption) {
                    target.name = content.name;
                } else {
                    target.caption = target.name = content.name;
                }
                return target;
            };
        };

        var mapContentResourceTo = function (model) {
            return function (resource) {
                model.link = resource.urls.join() || "";
                return model;
            };
        };

        return function (model) {
            return getStartNodeId()
                .then(pickContent)
                .then(mapContentTo(model))
                .then(function (m) { return contentResource.getById(m.id); })
                .then(mapContentResourceTo(model));
        };
    };

    var init = function ($scope, internalPicker, mediaPicker, contentResource, mediaResource) {
        var mapContentTo = function(target) {
            return function (content) {
                target.icon = content.icon;
                target.name = content.name;
                target.link = content.urls[0] || "";
            }
        }

        var mapMediaTo = function (target) {
            return function (content) {
                target.icon = content.icon;
                target.name = content.name;
                target.link = content.image;
            }
        }

        $scope.linkTypes = {
            Internal: 0,
            External: 1,
            Email: 2,
            Media: 3
        };

        $scope.showType = function (type) {
            return $scope.config.linkTypes.indexOf(type) > -1;
        }

        var updater = function (o) {
            $scope.model = o;
        };

        var encode = function (str) {
            return btoa(str);
        }

        $scope.processCaptionChange = function (link) {
            link.caption = (link.prettyCaption);
        }

        $scope.processEmailChange = function (link) {
            link.link = "mailTo:" + link.prettyLink;
            link.caption = link.caption ? link.caption : link.prettyLink;
            link.prettyCaption = link.prettyLink;
        }

        $scope.internalPicker = internalPicker;
        $scope.mediaPicker = mediaPicker;

        $scope.addInternalLink = function () {
            internalPicker({ target: "_self", caption: $scope.caption, type: $scope.linkTypes.Internal }).then(updater);
        };

        $scope.addEmailLink = function () {
            updater({ link: "", caption: $scope.caption, type: $scope.linkTypes.Email });
        };

        $scope.addExternalLink = function () {
            updater({ link: "", caption: $scope.caption, target: "_self", type: $scope.linkTypes.External });
        };

        $scope.addMediaLink = function () {
            mediaPicker({ caption: $scope.caption, target: "_self", type: $scope.linkTypes.Media }).then(updater);
        }

        $scope.getMainClass = function () {
            return {
                '_can-add-link': $scope.model == null,
                '_value-selected': $scope.model != null,
                '_use-alt-text': $scope.config.useAltText
            }
        };

        if ($scope.model && !$scope.model.name) {
            switch ($scope.model.type) {
                case $scope.linkTypes.Internal.toString():
                    contentResource.getById($scope.model.id).then(mapContentTo($scope.model));
                    break;
                case $scope.linkTypes.Media.toString():
                    mediaResource.getById($scope.model.id).then(mapMediaTo($scope.model));
                    break;
            }
        }
    };

    var factory = function ($q, dialogService, contentResource, entityResource, mediaResource) {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/TinyMce/RteLinksPicker/rte-links-picker.html',
            scope: {
                caption: '=',
                model: '=',
                configModel: '=config'
            },
            link: function ($scope) {
                $scope.config = angular.extend({}, defaultConfig, $scope.configModel);

                var internalPicker = internalPickerFactory($q, dialogService, contentResource, entityResource, $scope.config.internalPicker);
                var mediaPicker = mediaPickerFactory($q, dialogService);
                init($scope, internalPicker, mediaPicker, contentResource, mediaResource);
            }
        };
    }

    factory.$inject = ['$q', 'dialogService', 'contentResource', 'entityResource', 'mediaResource'];

    angular.module('umbraco').directive('rteLinksPicker', factory);
})(angular);
