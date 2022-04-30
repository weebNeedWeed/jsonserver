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

let openList = [];

const handleToggleInput = (jsonId) => {
    const td = document.getElementById(jsonId);
    const oldName = td.innerText;

    if (openList.findIndex(elm => elm.id === jsonId) === -1) {
        openList.push({
            id: jsonId,
            name: oldName
        });

        const input = document.createElement("input");
        input.className = "input";

        const submitButton = document.createElement("button");
        submitButton.className = "action";
        submitButton.onclick = () => {
            const newName = input.value;

            $.ajax({
                url: "/Account/EditJson",
                contentType: "application/x-www-form-urlencoded",
                method: "PUT",
                data: {
                    JsonId: jsonId,
                    Name: newName
                },
                success: () => {
                    td.innerHTML = newName;

                    openList = [...openList].filter(elm => elm.id !== jsonId);
                },
                error: () => {
                    $("#modal-body").text("Failed to change name!");
                    $("#successModal").modal("show");
                }
            });
        };

        submitButton.innerHTML = '<i class="fa-solid fa-check"></i>';

        td.innerHTML = "";

        td.appendChild(input);
        td.appendChild(submitButton);

        input.focus();

        return;
    }

    const item = openList.find(elm => elm.id === jsonId);

    td.innerHTML = item.name;

    openList = [...openList].filter(elm => elm.id !== jsonId);
}