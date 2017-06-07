(function (angular) {
    'usse strict';

    angular.module('umbraco')
        .constant("bulletinsManagementConfig", {
            description: {
                toolbar: "bold | italic | alignleft | aligncenter | alignright | bullist"
            },
            publishDate: {
                enableTime: true,
                time_24hr: true,
                dateFormat: "d/m/Y H:i"
            },

            media: {
                disableFolderSelect: "1",
                multiPicker: "1",
                onlyImages: "0"
            }
        });

})(angular);