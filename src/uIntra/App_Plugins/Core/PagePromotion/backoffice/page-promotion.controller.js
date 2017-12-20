(function (angular) {
    'use strict';

    var controller = function ($scope, editorState) {
        var self = this;
        $scope.model.hideLabel = true;

        self.promoteOnCentralFeedChange = function () {
            if ($scope.model.value.promoteOnCentralFeed) {
                $scope.model.value.publishDate = new Date();

                var currentEditorState = editorState.getCurrent();

                if (!$scope.model.value.title) {
                    $scope.model.value.title = currentEditorState.name;
                }
            }
        }
    };

    angular.module('umbraco').controller('pagePromotionController', ['$scope', 'editorState', controller]);
})(angular);