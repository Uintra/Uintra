(function (angular) {
    'use strict';

    var controller = function ($scope, $routeParams, memberService) {

        function initFormSubmitEventListener() {
            $scope.$on("formSubmitted", function (ev, args) {
                var memberId = $routeParams.id;
                memberService.memberChanged(memberId);
            });
        }

        initFormSubmitEventListener();
    };

    angular.module("umbraco").controller('memberChangesWatcherController', ['$scope', '$routeParams', 'memberService', controller]);
})(angular);