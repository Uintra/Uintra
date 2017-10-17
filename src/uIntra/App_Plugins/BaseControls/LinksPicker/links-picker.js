(function (angular) {
    'use strict';
    var isSingleMode = function (mode) {
        var modes = ['single', 'multiple'];
        var validMode = modes.indexOf(mode) > -1;
        if (!validMode) {
            throw new Error('Links Picker: mode "' + mode + '"! Allowed modes "' + modes.join('","') + '"');
        }
        return mode === modes[0];
    };

    var defaultConfig = {
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
                multiPicker: false,
                filterCssClass: "not-allowed not-published",
                callback: deferred.resolve
            });
            return deferred.promise;
        };

        function map(target, content) {
            target.id = content.id;
            target.caption = target.name = content.name;
            target.icon = content.icon;
            target.link = content.image;
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
                target.caption = target.name = content.name;
                target.icon = content.icon;
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


    var init = function ($scope, internalPicker, mediaPicker) {
        $scope.linkTypes = {
            Internal: 0,
            External: 1,
            Email: 2,
            Media: 3
        };

        $scope.showType = function (type) {
            return $scope.config.linkTypes.indexOf(type) > -1;
        }

        var objectRemover = function () {
            $scope.model = null;
        };
        var arrayRemover = function (o) {
            $scope.model.splice($scope.model.indexOf(o), 1);
        };

        $scope.removeLink = $scope.isSingleMode ? objectRemover : arrayRemover;

        var objectSetter = function (o) {
            $scope.model = o;
        };
        var arraySetter = function (o) {
            ($scope.model = $scope.model || []).push(o);
        };
        var updater = $scope.isSingleMode ? objectSetter : arraySetter;        

        $scope.processCaptionChange = function (link) {
            link.caption = link.prettyCaption;
        }

        $scope.processEmailChange = function (link) {
            link.link = "mailTo:" + link.prettyLink;
            link.caption = link.caption ? link.caption : link.prettyLink;
            link.prettyCaption = link.prettyLink;
        }

        $scope.internalPicker = internalPicker;
        $scope.mediaPicker = mediaPicker;

        $scope.addInternalLink = function () {
            internalPicker({ target: "_self", type: $scope.linkTypes.Internal }).then(updater);
        };

        $scope.addEmailLink = function () {
            updater({ link: "", caption: "", type: $scope.linkTypes.Email });
        };

        $scope.addExternalLink = function () {
            updater({ link: "", caption: "", target: "_self", type: $scope.linkTypes.External });
        };

        $scope.addMediaLink = function () {
            mediaPicker({ target: "_self", type: $scope.linkTypes.Media }).then(updater);
        }
        $scope.getRepeaterModel = function () {
            return $scope.isSingleMode ? ($scope.model != null ? [$scope.model] : []) : $scope.model;
        };

        $scope.getMainClass = function () {
            return {
                '_can-add-link': $scope.isSingleMode && $scope.model == null || !$scope.isSingleMode,
                '_value-selected': angular.isArray($scope.model) ? $scope.model.length : $scope.model != null,
                '_use-alt-text': $scope.config.useAltText,
                '_single-mode': $scope.isSingleMode
            }
        };

        $scope.sortableOptions = {
            items: "tr:not(._not-sortable)",
            placeholder: "ui-sortable-placeholder",
            axis: 'y'
        };
    };

    var factory = function ($q, dialogService, contentResource, entityResource) {
        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/LinksPicker/links-picker.html',
            scope: {
                model: '=',
                configModel: '=config',
                mode: '@'
            },
            link: function ($scope) {
                $scope.config = angular.extend({}, defaultConfig, $scope.configModel);
                $scope.isSingleMode = isSingleMode($scope.mode);

                var internalPicker = internalPickerFactory($q, dialogService, contentResource, entityResource, $scope.config.internalPicker);
                var mediaPicker = mediaPickerFactory($q, dialogService);
                init($scope, internalPicker, mediaPicker);
            }
        };
    }

    factory.$inject = ['$q', 'dialogService', 'contentResource', 'entityResource'];

    angular.module('umbraco').directive('linksPicker', factory);
})(angular);
