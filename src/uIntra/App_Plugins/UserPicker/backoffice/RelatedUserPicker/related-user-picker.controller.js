(function () {
    var relatedUserPickerController = function ($scope, $http) {
        var self = this;

        var config = {
            onSelectionChanged: onSelectionChanged,
            usersDataSource: getAllowedUsers,
            selectedUserId: $scope.model.value
        }
        self.userPickerConfig = config;

        function onSelectionChanged(selectedUserId) {
            $scope.model.value = selectedUserId;
        }

        function getAllowedUsers() {
            return $http.get('/umbraco/backoffice/api/UserApi/NotAssignedToMemberUsers');
        }
    }
    angular.module('umbraco').controller('relatedUserPickerController', ['$scope', '$http', relatedUserPickerController]);
})();