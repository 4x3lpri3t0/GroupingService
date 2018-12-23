function Save() {
    var parent = $(this).parent().parent(); // tr
    var tdName = parent.children("td:nth-child(1)");
    var tdSkill = parent.children("td:nth-child(2)");
    var tdRemoteness = parent.children("td:nth-child(3)");
    var tdButton = parent.children("td:nth-child(4)");

    var name = tdName.children("input[type=text]").val();
    var skill = tdSkill.children("input[type=text]").val();
    var remote = tdRemoteness.children("input[type=text]").val();

    tdName.html(name);
    tdSkill.html(skill);
    tdRemoteness.html(remote);
    tdButton.hide();

    $('#btnNewPlayer').prop("disabled", false);
}; 

function AddPlayer() {
    $("#tblData tbody").append(
        "<tr>" +
        "<td><input type='text'/></td>" +
        "<td><input type='text'/></td>" +
        "<td><input type='text'/></td>" +
        "<td><img src='images/disk.png' class='btnSave' title='Save and send to server'></td>" +
        "</tr>");
    
    $('.btnSave').on('click', Save);
    $('#btnNewPlayer').prop("disabled", true);
}; 

$(document).ready(function () {
    var name = "pindonga";
    var skill = 0.7;
    var remoteness = 42;

    $.ajax({
        url: '/addUser?name=' + name + '&skill=' + skill + '&remoteness=' + remoteness,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            // Do something with the result
        }
    });
});