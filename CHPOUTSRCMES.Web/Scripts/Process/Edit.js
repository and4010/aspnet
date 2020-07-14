$(document).ready(function () {
    $('#BtnSave').click(function () {
        var remark = $('#textarea').val()
        var Process_Detail_Id = $('#Process_Detail_Id').val()
        $.ajax({
            url: "/Process/EditSave",
            type: "POST",
            data: { remark: remark, Process_Detail_Id: Process_Detail_Id},
            dataType: "JSON",
            success: function (Boolean) {
                if (Boolean) {
                    location.href = '/Process/Index'
                }

            }
        })
    })



});