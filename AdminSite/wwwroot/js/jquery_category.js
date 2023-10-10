$('#create').click(function () {
    $("#createCategory").modal("show");
    $('#btnCreate').click(function () {
        const name = $("#createName").val();
        const description = $("#createDescription").val();
        if(name===""||description===""){
            $("#inform").text("Name or Description is Empty!")
            return;
        }
        const totalRow = $(this).attr("data-item")
        $.ajax({
            url: '/Category/Create/',
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            async: true,
            cache: false,
            data: JSON.stringify({
                "Name": name,
                "Description": description
            }),
            success: function (response) {
                console.log(response);
                location.reload();
                // const category = JSON.parse(response);
                // console.log(JSON.stringify(category))
                // const tbody = $('#dataTable tbody');
                // const newRow = $('<tr>');
                //
                // const newRowSTTCell = $('<td>');
                // newRowSTTCell.text(totalRow);
                //
                // const newRowNameCell = $('<td>');
                // newRowNameCell.text(category.Name);
                //
                // const newRowDescriptionCell = $('<td>');
                // newRowDescriptionCell.text(category.Description);
                //
                // const newRowCreateAtCell = $('<td>');
                // newRowCreateAtCell.text(category.CreatedAt);
                //
                // const newRowUpdateAtCell = $('<td>');
                // newRowUpdateAtCell.text(category.UpdatedAt);
                // const newRowActionCell = $('<td>');
                // newRowActionCell.html(" <a class='edit btn btn-danger text-white' data-item='@item.ToJson()'>Edit</a> |\n" +
                //     "                <a class='delete btn btn-dark text-white' data-item='@item.ToJson()'>Delete</a> |\n" +
                //     "                <a asp-action='Details' class='btn btn-secondary text-white' asp-route-id='@item.Id'>Details</a>");
                // newRow.append(newRowSTTCell)
                // newRow.append(newRowNameCell);
                // newRow.append(newRowDescriptionCell);
                // newRow.append(newRowCreateAtCell);
                // newRow.append(newRowUpdateAtCell);
                // newRow.append(newRowActionCell);
                // tbody.append(newRow);
            },
            error: function (response) {
                console.log(JSON.stringify(response))
            }
        });
    });
});
$(".edit").click(function () {
    const itemData = $(this).attr("data-item");
    const category = JSON.parse(itemData);
    $("#editId").val(category?.Id)
    $("#editName").val(category?.Name)
    $("#editDescription").val(category?.Description)
    $("#editCategory").modal("show");
    $('#btnEdit').on('click', function () {
        $.ajax({
            url: 'Category/Edit/',
            type: 'POST',
            async: true,
            cache: false,
            data: JSON.stringify({
                "Id": $("#editId").val(),
                "Name": $("#editName").val(),
                "Description": $("#editDescription").val(),
                "CreatedAt":category?.CreatedAt,
                "CreatedBy":category?.CreatedBy
            }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                console.log(response);
                location.reload();
            },
            error: function (response) {
                console.log(JSON.stringify(response))
            }
        });
    })
});
$(".delete").click(function () {
    const itemData = $(this).attr("data-item");
    const category = JSON.parse(itemData);
    $("#deleteId").val(category?.Id)
    $("#deleteName").val(category?.Name)
    $("#deleteCategory").modal("show");
    $('#btnDelete').on('click', function () {
        $.ajax({
            url: "Category/Delete/" + $('#deleteId').val(),
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {
                console.log(response);
                location.reload();
            },
            error: function (response) {
                console.log(JSON.stringify(response))
            }
        });
    })
});

