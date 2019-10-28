(function (angular) {
    'use strict';

    const directive = function (assetsService, angularHelper) {
        return {
            restrict: "E",
            templateUrl: "/App_Plugins/PanelThemePicker/prevalue-editor/theme-selector.directive.html",
            scope: {
                name: "@",
                color: "="
            },
            link: function (scope, element, attrs) {
                const container = element[0];

                assetsService.loadCss("lib/spectrum/spectrum.css");
                assetsService.loadJs("lib/spectrum/spectrum.js").then(function () {

                    const colorSelectorValue = $(container.querySelector(".js-color-selector__value"));
                    colorSelectorValue.spectrum({
                        color: null,
                        showInitial: false,
                        chooseText: "choose",
                        cancelText: "cancel",
                        preferredFormat: "hex",
                        showInput: true,
                        clickoutFiresChange: true,
                        hide: function () {
                            element.find(".btn.add").show();
                        },
                        change: function (color) {
                            angularHelper.safeApply(scope, function () {
                                scope.color = color.toHexString();
                            });
                        },
                        show: function () {
                            element.find(".btn.add").hide();
                        }
                    });

                });
            }
        }
    }

    directive.$inject = ["assetsService", "angularHelper"];

    angular.module("umbraco").directive('themeSelector', directive);
})(angular);