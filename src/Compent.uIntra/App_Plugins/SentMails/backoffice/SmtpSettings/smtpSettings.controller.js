angular.module('umbraco').controller('smtpSettingsController', ['$scope', 'sentMailsService',
    function ($scope, sentMailsService) {
        var self = this;

        self.isSaveBtnDisable = false;

        sentMailsService.getSmtpSettings().then(function (response) {
            self.smtpSettings = response.data;
        });

        self.save = function () {
            self.isSaveBtnDisable = true;

            sentMailsService.saveSmtpSettings(self.smtpSettings).then(function (response) {
                self.isSaveBtnDisable = false;
                alert(response.data);
            });
        };
    }
    ]);