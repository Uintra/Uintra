angular
    .module('umbraco')
    .controller('smtpSettingsController', ['$scope', 'sentMailsService',
        function ($scope, sentMailsService) {

            $scope.isSaveBtnDisable = false;
            $scope.smtpSettings = {};

            sentMailsService.getSmtpSettings()
                .then(
                    function (resolve) {
                        $scope.smtpSettings = resolve.data;
                    },
                    function (reject) {
                        alert('Fail. See details in console. ');
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