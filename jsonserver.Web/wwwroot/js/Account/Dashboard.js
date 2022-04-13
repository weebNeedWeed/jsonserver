const handleDeleteJson = (jsonId) => {
    $.ajax({
        contentType: "application/x-www-form-urlencoded",
        url: "/Account/DeleteJson",
        method: "DELETE",
        data: {
            jsonId: jsonId
        },
        success: () => {
            location.reload();
        }
    });
}