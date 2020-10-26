angular
    .module('umbraco')
    .controller('columnSettingsController', ['$scope', 'sentMailsService',
        function ($scope, sentMailsService) {

            $scope.isSaveBtnDisable = false;
            $scope.columnSettings = [];

            sentMailsService.getColumnSettings()
                .then(
                    function (resolve) {
                        $scope.columnSettings = resolve.data;
                    },
                    function (reject) {
                        alert('Fail. See details in console. ');
                        console.log(reject);
                    });

            $scope.save = function () {
                $scope.isSaveBtnDisable = true;

                sentMailsService.saveColumnSettings($scope.columnSettings)
                    .then(
                        function (resolve) {
                            $scope.isSaveBtnDisable = false;
                            alert(resolve.data);
                        },
                        function (reject) {
                            $scope.isSaveBtnDisable = false;
                            alert('Fail. See details in console. ');
                            console.log(reject);
                        });
            };
        }
    ]);