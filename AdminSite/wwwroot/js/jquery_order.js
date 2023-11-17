
$(".edit").click(function () {
    const itemData = $(this).attr("data-item");
    const id = $(this).attr("id");
    $("#editId").val(id)
    $("#editStatus").val(itemData).text(itemData)
    $("#editOrder").modal("show");
});