(function () {
    var relatedUserPickerController = function ($scope, $http) {
        var self = this;

        var config = {
            onSelectionChanged: onSelectionChanged,
            usersDataSource: function() {
                return getAllowedUsers($scope.model.value);
            },
            selectedUserId: $scope.model.value
        }
        self.userPickerConfig = config;

        function onSelectionChanged(selectedUserId) {
            $scope.model.value = selectedUserId;
        }

        function getAllowedUsers(selectedUserId) {
            return $http.get('/umbraco/backoffice/api/UserApi/NotAssignedToMemberUsers?selectedUserId=' + selectedUserId);
        }
    }
    angular.module('umbraco').controller('relatedUserPickerController', ['$scope', '$http', relatedUserPickerController]);
})();