(function (angular) {
    'usse strict';

    angular.module('umbraco')
        .constant("eventsManagementConfig", {
            description: {
                toolbar: "bold | italic | alignleft | aligncenter | alignright | bullist"
            },
            startDate: {
                useTime: false,
                useDate: true,
                useSeconds: false,
                offsetTime: "1",
                format: "DD.MM.YYYY"
            },
            endDate: {
                useTime: false,
                useDate: true,
                useSeconds: false,
                offsetTime: "1",
                format: "DD.MM.YYYY"
            },
            media: {
                disableFolderSelect: "1",
                multiPicker: "1",
                onlyImages: "0"
            }
        });

})(angular);