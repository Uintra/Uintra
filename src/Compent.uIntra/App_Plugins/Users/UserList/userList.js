import ajax from "./../../Core/Content/scripts/Ajax";

const searchBoxElement = $(".js-user-list-filter");
const tableBody = $(".js-user-list-table tbody");
const button = $(".js-user-list-button");
const sortLinks = $(".js-user-list-sort-link");
const searchActivationDelay = 256;

let ascendingClassName = "_asc";
let descendingClassName = "_desc";

let searchTimeout;

let controller = {
    init: function () {

        if (tableBody.children("tr").length < window.userListConfig.displayedAmount)
            button.hide();

        button.click(function (event) {
            var skip = tableBody.children("tr").length;
            window.userListConfig.request.skip = skip;
            window.userListConfig.request.take = window.userListConfig.amountPerRequest;

            ajax.post("/umbraco/surface/UserList/GetUsers", window.userListConfig.request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.append(rows);
                    updateLoadMoreButton(rows.length);
                });
        });

        sortLinks.click(function (event) {
            event.preventDefault();
            var link = $(this);
            var direction = link.data("direction");
            var hasDirection = direction !== undefined;
            window.userListConfig.request.skip = 0;
            window.userListConfig.request.take = window.userListConfig.displayedAmount;
            window.userListConfig.request.orderBy = link.data("order-by");
            console.log(window.userListConfig.request.orderBy);
            window.userListConfig.request.direction = hasDirection ? +!direction : 0;
            ajax.post("/umbraco/surface/UserList/GetUsers", window.userListConfig.request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    sortLinks.removeData("direction");
                    sortLinks.removeClass(ascendingClassName + " " + descendingClassName);
                    var currentDirection = window.userListConfig.request.direction;
                    link.data("direction", currentDirection);
                    link.addClass(currentDirection === 0 ? ascendingClassName : descendingClassName);
                    updateLoadMoreButton(rows.length);
                });
        });

        function updateLoadMoreButton(rowsCount) {
            if (rowsCount < window.userListConfig.amountPerRequest)
                button.hide();
            else button.show();
        }

        function onSearchStringChanged() {
            clearTimeout(searchTimeout);
            const searchString = searchBoxElement.val();
            searchTimeout = setTimeout(() => search(searchString), searchActivationDelay);
        }

        function search(searchString) {
            window.userListConfig.request.skip = 0;
            window.userListConfig.request.take = window.userListConfig.displayedAmount;
            window.userListConfig.request.query = searchString;

            ajax.post("/umbraco/surface/UserList/GetUsers", window.userListConfig.request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    updateLoadMoreButton(rows.length);
                });
        }

        if (!searchBoxElement) return;
        $(searchBoxElement).on("input", onSearchStringChanged);

    }
}

export default controller;