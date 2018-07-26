import ajax from "./../../Core/Content/scripts/Ajax";

const searchBoxElement = $(".js-user-list-filter");
const tableBody = $(".js-user-list-table tbody");
const button = $(".js-user-list-button");
const sortLinks = $(".js-user-list-sort-link");
const searchActivationDelay = 256;
const url = "/umbraco/surface/UserList/GetUsers";

let ascendingClassName = "_asc";
let descendingClassName = "_desc";
let lastRequestXlassName = "last";

let searchTimeout;
let request;
let displayedAmount;
let amountPerRequest;

let controller = {
    init: function () {

        init();
        button.click(onButtonClick);
        sortLinks.click(onSortClick);
        searchBoxElement.on("input", onSearchStringChanged);

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
                .then((result, arg2, arg3) => {
                    var rows = $(result.data).filter("tr");
                    tableBody.children().remove();
                    tableBody.append(rows);
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
                    updateUI(rows);
                });
        }

        function updateUI(rows) {
            if (rows.hasClass(lastRequestXlassName))
                button.hide();
            else button.show();
        }
    }
}

export default controller;