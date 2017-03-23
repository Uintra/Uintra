(function () {
    'use strict';

    angular
        .module('umbraco')
        .directive('internalLink', internalLinkDirective);

    internalLinkDirective.$inject = ["dialogService"];

    function internalLinkDirective(dialogService) {
        return {
            scope: {
                internalLink: "="
            },
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/InternalLink/internalLink.html',
            link: function internalLinkController(scope) {
                scope.openContentPicker = function () {
                    dialogService.contentPicker({
                        multiPicker: false,
                        filterCssClass: "not-allowed not-published",
                        callback: function (content) {
                            scope.internalLink = {
                                id: content.id,
                                name: content.name,
                                path: content.path,
                                icon: content.icon
                            }
                        }
                    });
                }

                scope.removeContent = function () {
                    scope.internalLink = undefined;
                }
            }
        };
    }

})();