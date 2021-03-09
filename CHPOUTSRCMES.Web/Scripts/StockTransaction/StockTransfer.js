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


function GetItemNumberList() {

    var SubinventoryCode = $("#ddlOutSubinventory").val();
    var Locator = $("#ddlOutLocator").val();
    var ddlItemNumber = $("#ddlItemNumber");

    $.ajax({
        url: "/StockTransaction/GetItemNumberList",
        type: "post",
        data: {
            SubinventoryCode: SubinventoryCode,
            Locator: Locator

        },
        success: function (data) {
            $('#ddlItemNumber').empty();
            for (var i = 0; i < data.length; i++) {
                $('#ddlItemNumber').append($('<option></option>').val(data[i].Value).html(data[i].Text));
            }

            ddlItemNumber.combobox('autocomplete', ddlItemNumber[0][0].value, ddlItemNumber[0][0].text);
            $("#ddlItemNumber").change();

        },
        error: function () {
            swal.fire('更新料號失敗');
        },
        complete: function (data) {


        }

    })
}






