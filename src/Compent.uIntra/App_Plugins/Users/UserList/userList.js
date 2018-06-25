import ajax from "./../../Core/Content/scripts/Ajax";

const searchBoxElement = $(".js-user-list-filter");
const tableBody = $(".js-user-list-table tbody");
const button = $(".js-user-list-button");
const sortLinks = $(".js-user-list-sort-link");
const searchActivationDelay = 256;

let searchTimeout;

let controller = {
    init: function () {

        button.click(function (event) {
            var skip = tableBody.children("tr").length;
            window.userListConfig.request.skip = skip;
            window.userListConfig.request.take = window.userListConfig.amountPerRequest;

            ajax.post("/umbraco/surface/UserList/GetUsers", window.userListConfig.request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.append(rows);
                });
        });

        sortLinks.click(function (event) {
            event.preventDefault();
            window.userListConfig.request.skip = 0;
            window.userListConfig.request.take = window.userListConfig.displayedAmount;
            window.userListConfig.request.orderBy = $(this).data("orderBy");
            ajax.post("/umbraco/surface/UserList/GetUsers", window.userListConfig.request)
                .then(result => {
                    var rows = $(result.data).filter("tr");
                    tableBody.children().remove();
                    tableBody.append(rows);
                });
        });

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
                });
        }

        if (!searchBoxElement) return;
        $(searchBoxElement).on("input", onSearchStringChanged);

    }
}

export default controller;