import ajax from "./../../Core/Content/scripts/Ajax";
import confirm from "./../../Core/Controls/Confirm/Confirm";

require("./user-list.css");

const searchBoxElement = $(".js-user-list-filter");
const tableBody = $(".js-user-list-table tbody");
const button = $(".js-user-list-button");
const sortLinks = $(".js-user-list-sort-link");
const displayedRows = $(".js-user-list-row");
const searchActivationDelay = 256;
const url = "/umbraco/surface/UserList/GetUsers";
const detailsUrl = "/umbraco/surface/UserList/Details";

let ascendingClassName = "_asc";
let descendingClassName = "_desc";
let lastRequestClassName = "last";

let searchTimeout;
let request;
let displayedAmount;
let amountPerRequest;

let controller = {
    init: function () {

        if (tableBody.length === 0)
            return;
        init();
        button.click(onButtonClick);
        sortLinks.click(onSortClick);
        searchBoxElement.on("input", onSearchStringChanged);
        addDetailsHandler(displayedRows);

        function init() {
            request = window.userListConfig.request;
            displayedAmount = window.userListConfig.displayedAmount;
            amountPerRequest = window.userListConfig.amountPerRequest;
        }

        function onButtonClick(event) {
            request.skip = tableBody.children("tr").length;
            request.take = amountPerRequest;

            ajax.post(url, request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    updateUI(rows);
                });
        }

        function onSortClick(event) {
            event.preventDefault();
            var link = $(this);
            var direction = link.hasClass(ascendingClassName) ? 1 : 0;
            request.skip = 0;
            request.take = displayedAmount;
            request.orderBy = link.data("order-by");
            request.direction = direction;

            ajax.post(url, request)
                .then((result) => {
                    var rows = $(result.data).filter("tr");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    addDetailsHandler(rows);
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
            request.query = searchString;

            ajax.post(url, request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    updateUI(rows);
                });
        }

        function updateUI(rows) {
            if (rows.hasClass(lastRequestClassName))
                button.hide();
            else button.show();
        }

        function addDetailsHandler(rows) {
            rows.click(function () {
                var data = { id: $(this).data("id") };
                ajax.post(detailsUrl, data)
                    .then(result => {
                        confirm.alert(
                            "Detailed info",
                            result.data);
                    });
            });
        }
    }
}

export default controller;