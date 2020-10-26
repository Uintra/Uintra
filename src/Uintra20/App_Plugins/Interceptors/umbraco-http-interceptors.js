angular.module('umbraco.services').config([
    '$httpProvider',
    function ($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                'response': function (response) {
                    if (response.config.url.includes('/umbraco/backoffice/UmbracoApi/Content/PostSave')) {
                        if (response.status === 200) {
                            var event = new CustomEvent('nodeSaved', { detail: response.data });
                            document.dispatchEvent(event);
                        }
                    }
                    return response;
                }
            };
        });
    }
]);