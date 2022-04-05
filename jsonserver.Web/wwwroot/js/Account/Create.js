$("#Name").on("keyup", () => {
    if (!$("form").valid()) {
        $("#Name").addClass("is-invalid");
    } else {
        $("#Name").removeClass("is-invalid");
    }
});