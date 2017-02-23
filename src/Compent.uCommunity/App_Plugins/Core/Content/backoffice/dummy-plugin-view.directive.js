(function () {
    'use strict';

    var directiveFactory = function (editorState) {
        return {
            restrict: "E",
            template: '<div class="dummy-plugin-view-holder"><div class="title-holder"><span class="{{icon}}"></span><span class="text" ng-bind="text"></span></div></div>',
            replace: true,
            scope: {
                text: '@'
            },
            link: function ($scope, $elem, $attrs) {
                $scope.currentState = editorState.current;
                $scope.icon = "icon " + $scope.currentState.icon;
            }
        }
    }

    directiveFactory.$inject = ['editorState'];

    angular.module('umbraco').directive("dummyView", directiveFactory);
})();