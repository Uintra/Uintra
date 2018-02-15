(function (angular) {
    'use strict';
    var userPicker = function () {
        var controller = function ($scope) {
            var config = $scope.userPickerConfig;

            var defaultOption = {
                id: "-1",
                name: "not selected"
            }
            
            $scope.defaultOption = defaultOption;

            $scope.userSelected = function (user) {
                config.onSelectionChanged(user.id);
            }

            function init() {
                config.usersDataSource().then(function (result) {
                    $scope.users = [defaultOption].concat(result.data);
                    $scope.selectedUser = getSelectedUserById($scope.users, config.selectedUserId);
                });
            }

            init();
        };

        function getSelectedUserById(users, id) {
            return users.find(function (user) {
                return user.id.toString() === id;
            });
        }

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/UserPicker/UserPicker/user-picker.directive.html',
            replace: true,
            scope: {
                userPickerConfig: '=config'
            },
            controller: ['$scope', controller]
        };
    }

    angular.module("umbraco").directive('userPicker', [userPicker]);
})(angular);