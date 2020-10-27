angular
    .module('umbraco')
    .controller('smtpSettingsController', ['$scope', 'sentMailsService',
        function ($scope, sentMailsService) {

            $scope.isSaveBtnDisable = false;
            $scope.loading = true;
            $scope.smtpSettings = {};
            $scope.isBadResponse = true;

            sentMailsService.getSmtpSettings()
                .then(
                    function (resolve) {
                        $scope.smtpSettings = resolve.data;
                        $scope.loading = false;
                        $scope.isBadResponse = false;
                    },
                    function (reject) {
                        alert('Fail. See details in console. ');
                        $scope.loading = false;
                        console.log(reject);
                    });

            $scope.save = function () {
                $scope.isSaveBtnDisable = true;

                sentMailsService.saveSmtpSettings($scope.smtpSettings)
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