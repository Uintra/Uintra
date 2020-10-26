//adds the resource to umbraco.resources module:
angular.module('umbraco.resources').factory('enumResource',
    function ($q, $http) {
        //the factory object returned
        return {
            //this cals the Api Controller we setup earlier
            getMediaFolderTypeEnum: function (assemblyName, typeName) {
                return $http.get("/api/Enum/MediaFolderType");
            }
        };
    }
);