(function (angular) {
    'use strict';

    var controller = function ($scope, memberService) {

        function initFormSubmitEventListener() {
            $scope.$on("formSubmitted", function (ev, args) {
                if (!args.scope.content) {
                    return;
                }

                if (args.scope.content.id !== 0) {
                    memberService.memberChanged(args.scope.content.key)
                        .then(() => location.reload());

                }

                args.scope.$watch('page.saveButtonState', function () {
                    if (args.scope.content.id !== 0) {
                        memberService.memberChanged(args.scope.content.key)
                            .then(() => location.reload());
                    }
                });
            });
        }

        initFormSubmitEventListener();
    };

    angular.module("umbraco").controller('memberChangesWatcherController', ['$scope', 'memberService', controller]);
})(angular);