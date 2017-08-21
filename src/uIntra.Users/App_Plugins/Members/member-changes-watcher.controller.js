(function (angular) {
    'use strict';

    var controller = function ($scope, memberService) {

        function initFormSubmitEventListener() {
            $scope.$on("formSubmitted", function (ev, args) {
                var memberId = args.scope.content.id !== 0 ? args.scope.content.key : null;

                if (memberId) {
                    memberService.memberChanged(memberId);
                    return;
                }

                args.scope.$watch('page.saveButtonState', function () {
                    if (args.scope.page.saveButtonState === "success") {
                        memberService.memberChanged(args.scope.content.key);
                    }
                });
            });
        }

        initFormSubmitEventListener();
    };

    angular.module("umbraco").controller('memberChangesWatcherController', ['$scope', 'memberService', controller]);
})(angular);