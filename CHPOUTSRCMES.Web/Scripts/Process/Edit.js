
$(document).ready(function () {
    $('#BtnSave').click(function () {
        var Note = $('#textarea').val()
        var OspHeaderId = $('#OspHeaderId').val()
        $.ajax({
            url: "/Process/EditSave",
            type: "POST",
            data: { Note: Note, OspHeaderId: OspHeaderId},
            dataType: "JSON",
            success: function (data) {
                if (data.resultModel.Success) {
                    location.href = '/Process/Index'
                } else {
                    swal.fire(data.resultModel.Msg);
                }

            }
        })
    })



});