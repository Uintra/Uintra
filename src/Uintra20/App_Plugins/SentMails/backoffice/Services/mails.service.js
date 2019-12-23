angular
    .module('umbraco.resources')
    .factory('sentMailsService', ['$http',
        function ($http) {
            var routePrefix = '/umbraco/backoffice/SentMails/SentMailsApi/';
            return {

                getAllSentMails: function (data) {
                    return $http.post(routePrefix + 'mails', data);
                },

                getColumnSettings: function () {
                    return $http.get(routePrefix + 'columnSettings');
                },

                getSmtpSettings: function () {
                    return $http.get(routePrefix + 'smtpSettings');
                },

                saveColumnSettings: function (data) {
                    return $http.post(routePrefix + 'columnSettings', data);
                },

                saveSmtpSettings: function (data) {
                    return $http.post(routePrefix + 'smtpSettings', data);
                }
            };
        }
    ]);

