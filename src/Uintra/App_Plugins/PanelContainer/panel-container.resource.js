function panelContainerResource($q, $http, umbRequestHelper) {
    const baseUrl = "/umbraco/backoffice/api/panelContainer/";

    function getCulture() {
        return param("cculture") || param("mculture") || "";
    }

    function param(name) {
        var arr = (window.location.href.split(name + '=')[1] || '').split('&');
        return arr && arr.length > 0 ? arr[0] : "";
    }

    return {
        getLocalPanelTypes: function (nodeTypeAlias) {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getLocalPanelTypes?nodeTypeAlias=" + nodeTypeAlias),
                "Failed to get local panel types");
        },
        getGlobalPanelTypes: function (nodeTypeAlias, nodeId) {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getGlobalPanelTypes?nodeTypeAlias=" + nodeTypeAlias + "&nodeId=" + nodeId + "&culture=" + getCulture()),
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
        },
        getGlobalPanelAnchors: function (nodeId) {
            return umbRequestHelper.resourcePromise(
                $http.get(baseUrl + "getGlobalPanelAnchors?nodeId=" + nodeId + "&culture=" + getCulture()),
                "Failed to get global panel anchors");
        }
    };
}
angular.module("umbraco.resources").factory("panelContainerResource", panelContainerResource);
