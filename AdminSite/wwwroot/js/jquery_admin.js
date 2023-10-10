$('#create').click(function () {
    $("#createAdmin").modal("show");
});
$(".delete").click(function () {
    const itemData = $(this).attr("data-item");
    const Admin = JSON.parse(itemData);
    $("#deleteId").val(Admin?.Id)
    $("#deleteAdmin").modal("show");
});

