(function (angular) {
    'use strict';

    angular.module('umbraco')
        .constant("newsManagementConfig", {
            description: {
                toolbar: "styleselect | bold | italic | underline | linksPicker | numlist | bullist | removeformat",
                style_formats: [
                    { title: 'Heading 1', block: 'h1' },
                    { title: 'Heading 2', block: 'h2' },
                    { title: 'Heading 3', block: 'h3' },
                    { title: 'Normal', block: 'p' }
                ]
            },
            publishDate: {
                enableTime: true,
                time_24hr: true,
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