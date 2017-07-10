angular.module('umbraco').controller('columnSettingsController', ['$scope', 'sentMailsService',
    function ($scope, sentMailsService) {
        var self = this;

        self.isSaveBtnDisable = false;

        sentMailsService.getColumnSettings().then(function (response) {
            self.columnSettings = response.data;
        });

        self.save = function () {
            self.isSaveBtnDisable = true;

            sentMailsService.saveColumnSettings(self.columnSettings).then(function (response) {
                self.isSaveBtnDisable = false;
                alert(response.data);
            });
        };
    }
]);