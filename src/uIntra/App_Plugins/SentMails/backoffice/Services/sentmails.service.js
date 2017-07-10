angular.module('umbraco.resources')
    .factory('sentMailsService', ['$http', function ($http) {
        return {
            getAllSentMails: function (data) {
                return $http.post("/umbraco/backoffice/SentMails/SentMailsApi/GetAllSentMails", data);
            },
            getColumnSettings: function () {
                return $http.get("/umbraco/backoffice/SentMails/SentMailsApi/GetColumnSettings");
            },
            getSmtpSettings: function () {
                return $http.get("/umbraco/backoffice/SentMails/SentMailsApi/GetSmtpSettings");
            },
            saveColumnSettings: function(data) {
                return $http.post("/umbraco/backoffice/SentMails/SentMailsApi/SaveColumnSettings", data);
            },
            saveSmtpSettings: function (data) {
                return $http.post("/umbraco/backoffice/SentMails/SentMailsApi/SaveSmtpSettings", data);
            }
        };
    }
    ]);