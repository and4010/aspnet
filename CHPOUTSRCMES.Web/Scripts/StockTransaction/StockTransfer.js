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
                //SubinventoryInit();

                if (TransferType == "O") {
                    //$('#Content').append('<link rel="stylesheet" href="~/bower_components/datatables/media/css/dataTables.bootstrap.min.css" type="text/css" />');
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



////沒用到
//function GetShipmentNumberList(selectValue, selectText) {

//    var InSubinventoryCode = $("#ddlInSubinventory").val();
//    var InLocator = $("#ddlInLocator").val();
//    var OutSubinventoryCode = $("#ddlOutSubinventory").val();
//    var OutLocator = $("#ddlOutLocator").val();

//    $.ajax({
//        url: "/StockTransaction/GetShipmentNumberList",
//        type: "post",
//        data: {
//            OutSubinventoryCode: OutSubinventoryCode,
//            OutLocator: OutLocator,
//            InSubinventoryCode: InSubinventoryCode,
//            InLocator: InLocator
//        },
//        success: function (data) {
//            $('#ddlShipmentNumber').empty();
//            for (var i = 0; i < data.length; i++) {
//                $('#ddlShipmentNumber').append($('<option></option>').val(data[i].Value).html(data[i].Text));
//            }

//            //$("#ddlShipmentNumber").val("");
//            $("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);

//            //$("#ddlShipmentNumber").val(selectValue).trigger('change');

//        },
//        error: function () {
//            swal.fire('更新出貨編號失敗');
//        },
//        complete: function (data) {


//        }

//    })

//}
////沒用到
//function GetSubinventoryTransferNumberList(selectValue, selectText) {

//    var InSubinventoryCode = $("#ddlInSubinventory").val();
//    var InLocator = $("#ddlInLocator").val();
//    var OutSubinventoryCode = $("#ddlOutSubinventory").val();
//    var OutLocator = $("#ddlOutLocator").val();

//    $.ajax({
//        url: "/StockTransaction/GetSubinventoryTransferNumberList",
//        type: "post",
//        data: {
//            OutSubinventoryCode: OutSubinventoryCode,
//            OutLocator: OutLocator,
//            InSubinventoryCode: InSubinventoryCode,
//            InLocator: InLocator
//        },
//        success: function (data) {
//            $('#ddlShipmentNumber').empty();
//            for (var i = 0; i < data.length; i++) {
//                $('#ddlShipmentNumber').append($('<option></option>').val(data[i].Value).html(data[i].Text));
//            }

//            //$("#ddlSubinventoryTransferNumber").val("");
//            $("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);

//            //$("#ddlShipmentNumber").val(selectValue).trigger('change');


//        },
//        error: function () {
//            swal.fire('更新移轉編號失敗');
//        },
//        complete: function (data) {


//        }

//    })
//}
//沒用到
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

            //$('#PACKING_TYPE_LABEL').hide();
            //$('#PACKING_TYPE').hide();
            //$('#PACKING_TYPE').html("");
            //$('#ddlItemNumber').data('uiAutocomplete')._trigger('select');
            //ddlItemNumber.trigger("select");
            //ddlItemNumber.change(function() {
            //    $(this).next().val($(this).children(':selected').text());     
            //});

        },
        error: function () {
            swal.fire('更新料號失敗');
        },
        complete: function (data) {


        }

    })
}






