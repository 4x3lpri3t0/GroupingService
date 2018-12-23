function Save() {
    var parent = $(this).parent().parent(); // tr
    var tdName = parent.children("td:nth-child(1)");
    var tdSkill = parent.children("td:nth-child(2)");
    var tdRemoteness = parent.children("td:nth-child(3)");
    var tdButton = parent.children("td:nth-child(4)");

    var name = tdName.children("input[type=text]").val();
    var skill = tdSkill.children("input[type=text]").val();
    var remoteness = tdRemoteness.children("input[type=text]").val();

    tdName.html(name);
    tdSkill.html(skill);
    tdRemoteness.html(remoteness);
    tdButton.hide();

    $('#btnNewPlayer').prop("disabled", false);

    $.ajax({
        url: '/addUser?name=' + name + '&skill=' + skill + '&remoteness=' + remoteness,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            // Success notification
            $(".alert").show();
            $(".alert").fadeTo(1000, 500).slideUp(500, function () {
                $(".alert").slideUp(500);
            }); 
            
        }
    });    
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

function GetResultsHandler() {
    if (this.status == 200) {
        // Create a new Blob object using the response data of the onload object
        var blob = new Blob([this.response], { type: 'text/plain' });
        //Create a link element, hide it, direct it towards the blob, and then 'click' it programatically
        let a = document.createElement("a");
        a.style = "display: none";
        document.body.appendChild(a);
        //Create a DOMString representing the blob and point the link element towards it
        let url = window.URL.createObjectURL(blob);
        a.href = url;
        a.download = 'grouping_results.txt';
        // Programatically click the link to trigger the download
        a.click();
        // Release the reference to the file by revoking the Object URL
        window.URL.revokeObjectURL(url);
    } else {
        // Something went wrong
        console.log("Request to server failed...") // TODO: More realistic error handling
    }
}

function GetResults() {
    var request = new XMLHttpRequest();
    request.onload = GetResultsHandler;
    request.open('GET', '/download');
    request.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
    request.send();
}

$(document).ready(function () {
    // TODO
});