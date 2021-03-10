$(document).ready(function () {

    $('#ddlTransferType').change(function () {

        var TransferType = $("#ddlTransferType").val();
        if (TransferType == "請選擇") {
            $('#Content').empty();
            return;
        }

        $.ajax({
            url: "/StockTransaction/GetContent",
            type: "GET",
            dataType: 'html',
            data: {
                TransferType: TransferType
            },
            success: function (data) {
                $('#Content').empty();
                $('#Content').html(data);

                if (TransferType == "O") {
                    OutBoundInit();
                } else if (TransferType == "I") {
                    InBoundInit();
                } else {
                    TransferReasonInit();
                }
            },
            error: function () {
                swal.fire('更新內容失敗');
            },
            complete: function (data) {


            }

        })


    })




});
