(function () {
    'use strict';

    angular
        .module('umbraco')
        .directive('externalLink', externalLinkDirective);

    externalLinkDirective.$inject = [];

    function externalLinkDirective() {
        return {
            scope: {
                externalLink: "=ngModel",
                useAltText: '='
            },
            require: 'ngModel',
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/ExternalLink/externalLink.html',
            link: function (scope, element, attrs, ngModelCtrl) {

                if (!scope.externalLink || (!scope.externalLink.url && !scope.externalLink.altText)) {
                    scope.externalLink = { url: "", altText: "" };
                }

                scope.externalLink.useAltText = scope.useAltText;

                function validateUrl(textval) {
                    var urlregex = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/;
                    return urlregex.test(textval);
                }

                scope.validateLink = function () {
                    var isValid = true;

                    if (scope.externalLink.url) {
                        isValid = validateUrl(scope.externalLink.url);
                    }

                    scope.isRequired = !isValid;
                    ngModelCtrl.$setValidity('externalLink', isValid);
                }
                scope.isError = function () {
                    return ngModelCtrl.$error.externalLink;
                }

            }
        };
    }

})();