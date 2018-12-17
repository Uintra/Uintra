(function (angular) {
    'use strict';

    angular.module('umbraco')
        .constant("eventsManagementConfig", {
            description: {
                toolbar: "styleselect | bold | italic | underline | linksPicker | numlist | bullist | removeformat",
                style_formats: [
                    { title: 'Heading 1', block: 'h1' },
                    { title: 'Heading 2', block: 'h2' },
                    { title: 'Heading 3', block: 'h3' },
                    { title: 'Normal', block: 'p' }
                ]
            },
            startDate: {
                enableTime: true,
                time_24hr: true,
                dateFormat: "d/m/Y H:i"
            },
            endDate: {
                enableTime: true,
                time_24hr: true,
                dateFormat: "d/m/Y H:i"
            },
            publishDate: {
                useTime: true,
                useDate: true,
                useSeconds: false,
                time_24hr: true,
                enableTime: true,
                dateFormat: "d/m/Y H:i"
            },
            filterDate: {
                enableTime: false,
                time_24hr: true,
                dateFormat: "d/m/Y"
            },
            media: {
                disableFolderSelect: "1",
                multiPicker: "1",
                onlyImages: "0"
            }
        });

})(angular);