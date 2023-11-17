$('#create').click(function () {
    $("#createPayment").modal("show");
    $('#btnCreate').click(function () {
        const name = $("#createName").val();
        const description = $("#createDescription").val();
        if(name===""||description===""){
            $("#inform").text("Name or Description is Empty!")
            return;
        }
        const totalRow = $(this).attr("data-item")
        $.ajax({
            url: '/Payment/Create/',
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
            },
            error: function (response) {
                console.log(JSON.stringify(response))
            }
        });
    });
});
$(".edit").click(function () {
    const itemData = $(this).attr("data-item");
    const payment = JSON.parse(itemData);
    $("#editId").val(payment?.Id)
    $("#editName").val(payment?.Name)
    $("#editDescription").val(payment?.Description)
    $("#editPayment").modal("show");
    $('#btnEdit').on('click', function () {
        $.ajax({
            url: 'Payment/Edit/',
            type: 'POST',
            async: true,
            cache: false,
            data: JSON.stringify({
                "Id": $("#editId").val(),
                "Name": $("#editName").val(),
                "Description": $("#editDescription").val(),
                "CreatedAt":payment?.CreatedAt,
                "CreatedBy":payment?.CreatedBy
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
    const payment = JSON.parse(itemData);
    $("#deleteId").val(payment?.Id)
    $("#deleteName").val(payment?.Name)
    $("#deletePayment").modal("show");
    $('#btnDelete').on('click', function () {
        $.ajax({
            url: "Payment/Delete/" + $('#deleteId').val(),
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

