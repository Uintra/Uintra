function panelContainerResource($q, $http, umbRequestHelper) {
    const baseUrl = "/umbraco/backoffice/api/panelContainer/";

    return {
        getLocalPanelTypes: function (nodeTypeAlias) {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getLocalPanelTypes?nodeTypeAlias=" + nodeTypeAlias),
                "Failed to get local panel types");
        },
        getGlobalPanelTypes: function (nodeTypeAlias, nodeId) {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getGlobalPanelTypes?nodeTypeAlias=" + nodeTypeAlias + "&nodeId=" + nodeId),
                "Failed to get global panel types");
        },
        getRequiredPanelTypes: function (nodeTypeAlias) {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getRequiredPanelTypes?nodeTypeAlias=" + nodeTypeAlias),
                "Failed to get required panel types");
        },
        getAllPanelTypes: function () {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getAllPanelTypes"),
                "Failed to get required panel types");
        }
    };
}
angular.module("umbraco.resources").factory("panelContainerResource", panelContainerResource);
