import ajax from "./../../Core/Content/scripts/Ajax";
import confirm from "./../../Core/Controls/Confirm/Confirm";

require("./user-list.css");

const searchBoxElement = $(".js-user-list-filter");
const searchButton = $(".js-search-button");
const table = $(".js-user-list-table");
const tableBody = $(".js-user-list-table .js-tbody");
const button = $(".js-user-list-button");
const sortLinks = $(".js-user-list-sort-link");
const displayedRows = $(".js-user-list-row");
const emptyResultLabel = $(".js-user-list-empty-result");
const searchActivationDelay = 256;
const url = "/umbraco/surface/UserList/GetUsers";
const excludeUserFromGroupUrl = "/umbraco/surface/UserList/ExcludeUserFromGroup";

let ascendingClassName = "_asc";
let descendingClassName = "_desc";
let lastRequestClassName = "last";

let searchTimeout;
let request;
let displayedAmount;
let amountPerRequest;
let confirmTitle;
let confirmText;

let controller = {
    init: function () {

        if (tableBody.length === 0)
            return;
        init();
        button.click(onButtonClick);
        sortLinks.click(onSortClick);
        searchBoxElement.on("input", onSearchStringChanged);
        searchBoxElement.on("keypress", onKeyPress);
        searchButton.click(onSearchClick);
        addDetailsHandler(displayedRows);
        addRemoveUserFromGroupHandler(displayedRows);

        function init() {
            request = window.userListConfig.request;
            displayedAmount = window.userListConfig.displayedAmount;
            amountPerRequest = window.userListConfig.amountPerRequest;
            request.groupId = new URL(window.location.href).searchParams.get("groupId");
            confirmTitle = table.data("title");
            confirmText = table.data("text");
        }

        function onSearchClick(e) {
            const query = searchBoxElement.val();
            if (query) {
                search(query);
            }
        }

        function onKeyPress(e) {
            if (e.which === 13 || e.KeyCode === 13 || e.charCode === 13) {
                search(searchBoxElement.val());
                e.preventDefault();
                e.stopPropagation();
            }
        }

        function onButtonClick(event) {
            request.skip = tableBody.children("div").length;
            request.take = amountPerRequest;

            ajax.post(url, request)
                .then(result => {
                    var rows = $(result.data).filter("div");
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    addRemoveUserFromGroupHandler(rows);
                    updateUI(rows);
                });
        }

        function onSortClick(event) {
            event.preventDefault();
            var link = $(this);
            var direction = link.hasClass(ascendingClassName) ? 1 : 0;
            request.take = request.skip + request.take;
            request.skip = 0;
            request.orderingString = link.data("order-by");
            request.orderingDirection = direction;

            ajax.post(url, request)
                .then((result) => {
                    var rows = $(result.data).filter("div");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    addRemoveUserFromGroupHandler(rows);
                    sortLinks.removeClass(ascendingClassName + " " + descendingClassName);
                    link.addClass(direction === 0 ? ascendingClassName : descendingClassName);
                    updateUI(rows);
                });
        }

        function onSearchStringChanged() {
            clearTimeout(searchTimeout);
            const searchString = searchBoxElement.val();
            searchTimeout = setTimeout(() => search(searchString), searchActivationDelay);
        }

        function search(searchString) {
            request.skip = 0;
            request.take = displayedAmount;
            request.text = searchString;

            ajax.post(url, request)
                .then(result => {
                    var rows = $(result.data).filter("div");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    addRemoveUserFromGroupHandler(rows);
                    updateUI(rows);
                });
        }

        function updateUI(rows) {
            if (tableBody.children("div").length === 0) emptyResultLabel.show();
            else emptyResultLabel.hide();
            if (rows.hasClass(lastRequestClassName) || rows.length === 0) button.hide();
            else button.show();
        }

        function addDetailsHandler(rows) {
            rows.click(function () {
                var profileUrl = $(this).data("profile");
                location.href = profileUrl;
            });
        }
        function addRemoveUserFromGroupHandler(rows) {
            var deleteButtons = rows.find(".js-user-list-delete");
            deleteButtons.click(function (e) {
                e.preventDefault();
                e.stopPropagation();

                confirm.showConfirm(confirmTitle, confirmText, () => {
                    var row = $(this).closest(".js-user-list-row");
                    var userId = row.data("id");
                    ajax.post(excludeUserFromGroupUrl, { userId: userId })
                        .then(function (result) {
                            if (result.data) {
                                row.remove();
                                request.skip = request.skip - 1;
                            }
                        });
                }, () => { }, confirm.defaultSettings);
            });
        }

        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
    }
};

export default controller;