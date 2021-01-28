
$(document).ready(function () {
    $('#BtnSave').click(function () {
        var Note = $('#textarea').val()
        var OspHeaderId = $('#OspHeaderId').val()
        ShowWait(function () {
            $.ajax({
                url: "/Process/EditSave",
                type: "POST",
                data: { Note: Note, OspHeaderId: OspHeaderId },
                dataType: "JSON",
                success: function (data) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        location.href = '/Process/Index'
                    } else {
                        swal.fire(data.resultModel.Msg);
                    }

                },
                error: function () {
                    swal.fire("寫入備註失敗");
                }
            })
        });
    })
});