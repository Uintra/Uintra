(function (angular) {
    'use strict';

    angular.module('umbraco')
        .constant("notificationSettingsConfig", {
            description: {
                toolbar: "styleselect | bold | italic | underline | linksPicker | numlist | bullist | removeformat",
                style_formats: [
                    { title: 'Heading 1', block: 'h1' },
                    { title: 'Heading 2', block: 'h2' },
                    { title: 'Heading 3', block: 'h3' },
                    { title: 'Normal', block: 'p' }
                ]
            }
        });

})(angular);