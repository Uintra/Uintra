import ajax from "./../../Core/Content/scripts/Ajax";

let controller = {
    init: function () {
        var table = $("#userListTable");

        $("#loadMore").click(function (event) {
            ajax.post("/umbraco/surface/UserList/GetUsers", window.userListConfig)
                .then(result => {
                    var rows = $(result.data);
                    table.append(rows);
                    window.userListConfig.index++;
                });
        });
    }
}

export default controller;