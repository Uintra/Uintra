(function (angular) {
    'use strict';

    const controller = function ($scope, editorService, contentResource, overlayService) {
        var vm = this;
        vm.culture = param("cculture") || param("mculture");

        function param(name) {
            var arr = (window.location.href.split(name + '=')[1] || '').split('&');
            return arr && arr.length > 0 ? arr[0] : "";
        }

        if (!$scope.model.value) {
            $scope.model.value = "";
            vm.scriptValue = "";
        }
        else {
            vm.scriptValue = $scope.model.value;
        }

        vm.scriptValueChanged = function () {
            $scope.model.value = vm.scriptValue;
        }

        vm.scriptValueBlured = function () {
            var scriptOpenTag = '<script type="application/ld+json">';
            var scriptEndTag = '</script>';
            var value = vm.scriptValue ? vm.scriptValue.trim() : "";

            if (value && value.indexOf(scriptOpenTag) !== 0) {
                value = scriptOpenTag + "\n" + value;
            }

            if (value && value.indexOf(scriptEndTag) === -1) {
                value = value + "\n" + scriptEndTag;
            }
            vm.scriptValue = value;
            $scope.model.value = value;
        }

        vm.initCopyFrom = function () {
            if (vm.scriptValue.length > 0) {
                overlayService.open({
                    content: "Are you sure you want to replace current microdata?",
                    submitButtonLabel: "Confirm",
                    closeButtonLabel: "Cancel",
                    submit: function () {
                        contentPicker();
                        overlayService.close();
                    },
                    close: function () {
                        overlayService.close();
                    }
                });
            } else {
                contentPicker();
            }
        };

        function contentPicker() {
            var options = {
                multiPicker: false,
                submit: function (model) {
                    contentResource.getById(model.selection[0].id).then(function (response) {
                        var variant = response.variants.find(function (x) {
                            return x.language.culture === vm.culture
                        });

                        variant.tabs.forEach(function (tab) {
                            tab.properties.forEach(function (property) {
                                if (property.alias === "microdata" && property.value) {
                                    $scope.model.value = property.value;
                                    vm.scriptValue = property.value;
                                }
                                return;
                            });
                        });
                    });
                    editorService.close();
                },
                close: function () {
                    editorService.close();
                }
            };

            editorService.contentPicker(options);
        }

    };

    controller.$inject = ["$scope", "editorService", "contentResource", "overlayService"];

    angular.module("umbraco").controller("ubaselineMicrodataController", controller);

})(angular);

angular.module('umbraco').directive('jsonText', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attr, ngModel) {
            function into(input) {
                return JSON.parse(input);
            }
            function out(data) {
                return angular.toJson(data, 4);
            }
            ngModel.$parsers.push(into);
            ngModel.$formatters.push(out);

        }
    };
});