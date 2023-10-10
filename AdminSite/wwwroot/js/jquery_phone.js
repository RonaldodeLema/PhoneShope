$(".delete").click(function () {
    const itemData = $(this).attr("data-item");
    const phone = JSON.parse(itemData);
    $("#deleteId").val(phone?.Id)
    $("#deleteName").val(phone?.Name)
    $("#deletePhone").modal("show");
    $('#btnDelete').on('click', function () {
        $.ajax({
            url: "Phone/Delete/" + $('#deleteId').val(),
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

