$('#create').click(function () {
    $("#createAdmin").modal("show");
});
$(".delete").click(function () {
    const itemData = $(this).attr("data-item");
    $("#deleteId").val(itemData)
    $("#deleteAdmin").modal("show");
});

