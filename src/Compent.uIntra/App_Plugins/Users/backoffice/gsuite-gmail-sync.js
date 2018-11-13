(function () {
    var controller = function ($scope, $http) {

        $scope.gmailSettings = {
            clientId: "",
            clientSecret: "",
            domain: "",
            user: ""
        };

        $scope.sync = function () {
            var displayMessageContainer = document.getElementById("googleSyncResult");
            displayMessageContainer.innerText = "Synchronization in progress...";

            $http.post("/sync/users", $scope.gmailSettings).then(function (response) {
                if (!response.data.success) {
                    displayMessageContainer.innerText = response.data.message;
                    return;
                }
                if (!response.data.url) {
                    displayMessageContainer.innerText = "Sync is complete!";
                    return;
                }

                var dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : screen.left;
                var dualScreenTop = window.screenTop !== undefined ? window.screenTop : screen.top;

                var w = 600;
                var h = 500;

                var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
                var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

                var left = width / 2 - w / 2 + dualScreenLeft;
                var top = height / 2 - h / 2 + dualScreenTop;

                var childWindow = window.open(response.data.url, 'name', 'width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
                if (childWindow === null) {
                    displayMessageContainer.innerText = "You need to allow popups for current site";
                }
                else if (childWindow.focus) {
                    childWindow.focus();
                }
            }, function (response) { displayMessageContainer.innerText = "Internal server error. Status: " + response.status; });
        };
    };

    controller.$inject = ["$scope", "$http"];
    angular.module('umbraco').controller('gmailSyncController', controller);
})();