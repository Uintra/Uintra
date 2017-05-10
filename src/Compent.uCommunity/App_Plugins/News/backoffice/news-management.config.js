(function (angular) {
    'usse strict';

    angular.module('umbraco')
        .constant("newsManagementConfig", {
            description: {
                toolbar: "bold | italic | alignleft | aligncenter | alignright | bullist"
            },
            publishDate: {
                useTime: false,
                useDate: true,
                useSeconds: false,
                offsetTime: "1",
                dateFormat: "d/m/Y"
            },
            media: {
                disableFolderSelect: "1",
                multiPicker: "1",
                onlyImages: "0"
            }
        });

})(angular);