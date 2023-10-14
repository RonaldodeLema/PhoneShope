$('#create').click(function () {
    $("#createRole").modal("show");
});

$(".edit").click(function () {
    const id = $(this).attr("data-item");
    $.ajax({
        url: "Role/Details/" + id,
        type: 'GET',
        async: true,
        cache: false,
        success: function (response) {
            console.log(response)
            $("#editBody").html("")
            const roleClaims = JSON.parse(response.ListRoleClaim);
            const allRoleClaims = JSON.parse(response.ListAllClaim);
            const role = JSON.parse(response.Role);
            const roleName = role.Name;
            const $roleClaims = $('<ul></ul>');
            for (const roleClaim of allRoleClaims) {
                let $checkbox = $("");
                let newRoleClaim = roleClaim[0].toUpperCase() + roleClaim.substring(1)
                if(roleClaims.includes(roleClaim)){
                    $checkbox = $("<li><input type='checkbox' checked Name='Permissions' value='"+newRoleClaim+"' /> "+newRoleClaim.replace("_"," ")+'</li>');
                }
                else {
                    $checkbox = $("<li><input type='checkbox' Name='Permissions' value='"+newRoleClaim+"' /> "+newRoleClaim.replace("_"," ")+'</li>');
                }
                $roleClaims.append($checkbox);
            }
            const $editName = $('<input type="text" class="form-control" name="Name" id="editName" required="required" placeholder="Name" value="'+roleName+'">');
            const $editId = $('<input type="number" hidden="hidden" class="form-control" name="Id" id="editId" required="required" placeholder="Name" value="'+role.Id+'">');
            $('#editBody').append($editName).append($editId);
            $editName.before('<label for="editName">Name:</label>');
            $editName.after($roleClaims);
            $roleClaims.before('<h4>Permissions:</h4>');
            $roleClaims.after('<br/>');
            $("#editRole").modal("show");
        },
        error: function (response) {
            console.log(JSON.stringify(response))
        }
    });
});
$(".delete").click(function () {
    const itemData = $(this).attr("data-item");
    $("#deleteId").val(itemData)
    $("#deleteRole").modal("show");
});

