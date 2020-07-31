﻿var PaperRolldataTablesHeader
var FlatdataTablesHeader
///狀態  0已入庫 1待入庫 2 取消
$(document).ready(function () {
    EnableBarcode(true)
    LoadPaperRollHeard();
    LoadFlatHeader();

    ////取得狀態
    var Status = $("#Status").val();
    //if (Status == "0") {
        PaperRolldataTablesBody();
        FlatdataTablesBody();
    //} else {
    //    PaperRolldataTablesBody(Status);
    //    FlatdataTablesBody(Status);
    //}
    callDailog(Status);
    init(Status);


    //紙捲表身編輯事件
    $('#PaperRolldataTablesBody tbody').on('click', '.btn-edit', function (e) {

        var data = $('#PaperRolldataTablesBody').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        }
        var CabinetNumber = $('#CabinetNumber').val();
        var CreateDate = $('#CreateDate').val();
        $.ajax({
            url: '/Purchase/RollEditParameter/',
            type: "POST",
            data: { id: id, CabinetNumber: CabinetNumber, CreateDate: CreateDate },
            success: function (data) {
                window.location.href = "RollEdit?Id=" + data.id + "&CabinetNumber=" + data.cabinetNumber + "&CreateDate=" +data.CreateDate; 
            },
            error: function() {
                swal("失敗")
            }

        })
    })

    //紙捲表身檢視事件
    $('#PaperRolldataTablesBody tbody').on('click', '#btnView', function (e) {

        var data = $('#PaperRolldataTablesBody').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        }
        var CabinetNumber = $('#CabinetNumber').val();
        var CreateDate = $('#CreateDate').val();
        $.ajax({
            url: '/Purchase/RollViewParameter/',
            type: "POST",
            data: { id: id, CabinetNumber: CabinetNumber, CreateDate: CreateDate },
            success: function (data) {
                window.location.href = "RollView?Id=" + data.id + "&CabinetNumber=" + data.cabinetNumber + "&CreateDate=" + data.CreateDate;
            },
            error: function () {
                swal("失敗")
            }

        })
    })


    //平張表身編輯事件
    $('#FlatdataTablesBody tbody').on('click', '.btn-edit', function (e) {
        var data = $('#FlatdataTablesBody').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        }
        var CabinetNumber = $('#CabinetNumber').val();
        $.ajax({
            url: '/Purchase/FlatEditParameter/',
            type: "POST",
            data: { id: id, CabinetNumber: CabinetNumber },
            success: function (data) {
                window.location.href = "FlatEdit?Id=" + data.id + "&CabinetNumber=" + data.cabinetNumber; 
            },
            error: function () {

                $.swal("失敗")
            }

        })
    })
    //平張表身檢視事件
    $('#FlatdataTablesBody tbody').on('click', '#btnView', function (e) {

        var data = $('#FlatdataTablesBody').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        }
        var CabinetNumber = $('#CabinetNumber').val();
        $.ajax({
            url: '/Purchase/FlatViewParameter/',
            type: "POST",
            data: { id: id, CabinetNumber: CabinetNumber },
            success: function (data) {
                window.location.href = "FlatView?Id=" + data.id + "&CabinetNumber=" + data.cabinetNumber;
            },
            error: function () {

                $.swal("失敗")
            }

        })

    });




    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#PaperRolldataTablesHeader').DataTable().table().node().style.width = '';
        $('#FlatdataTablesBody').DataTable().table().node().style.width = '';
        $('#PaperRolldataTablesBody').DataTable().table().node().style.width = '';
        $('#FlatdataTablesHeader').DataTable().table().node().style.width = '';


        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });



    //列印標籤紙捲
    $('.row-std').on('click', '.btn-label', function (e) {
        PrintLable($('#PaperRolldataTablesBody').DataTable(), "/Home/GetLabel", "4");
    })

    //列印標籤平張
    $('.row-std').on('click', '.btn-label2', function (e) {
        PrintLable($('#FlatdataTablesBody').DataTable(), "/Home/GetLabel", "4");
    })



});




function PaperNavsNumber() {
    var CabinetNumber = $("#CabinetNumber").val();
    $.ajax({
        url: '/Purchase/PaperNumber',
        type: 'POST',
        datatype: 'json',
        data: { CabinetNumber: CabinetNumber },
        success: function (data) {
            $('#PaperRollSpan').text(data.PaperTotle);
        },
        error: function (data) {

        }
    });

}

function FlatNavsNumber() {
    var CabinetNumber = $("#CabinetNumber").val();
    $.ajax({
        url: '/Purchase/FlatNumber',
        type: 'POST',
        datatype: 'json',
        data: { CabinetNumber: CabinetNumber },
        success: function (data) {
            $('#FlatSpan').text(data.FlatTotle);
        },
        error: function (data) {

        }
    });



}

//紙捲匯入Dialog
function callDailog(status) {

    //紙捲匯入Dialog
    $("#BtnImportRoll").click(function () {

        if (status == "0") {
            swal.fire("狀態已入庫，無法匯入檔案");
            return;
        }

        $.ajax({
            url: '/Purchase/_ImportBodyRoll/',
            type: "POST",
            success: function (result) {
                $('body').append(result);
                Open($('#PaperRollModal'));
            },
            error: function () {
                swal.fire("失敗");
            }
        });

    });


    //平張匯入Dialog
    $("#BtnImportFlat").click(function () {
        $.ajax({
            url: '/Purchase/ImportBodyFlat',
            type: "POST",
            success: function (result) {
                $('body').append(result);
                Open($('#FlatModal'));
            },
            error: function () {
                swal.fire("失敗");
            }
        });
    });

}

//初始化
function init(status) {
    //在關鍵字input按下Enter，執行n送出 紙捲
    $("input[name=PaperRollKeyword]").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnPaperRollSaveBarcode").click();
        }
    });

    //儲存紙捲條碼按鈕
    $("#BtnPaperRollSaveBarcode").click(function (e) {
        var barcode = $('#PaperRollBarcode').val();
        var table = $('#PaperRolldataTablesBody').DataTable();
        if (status == "0") {
            swal.fire("已入庫無法儲存條碼")
            e.preventDefault();
            $('#PaperRollBarcode').select();
            return;
        }
        if (barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return;
        }
        if (table.data().length == 0) {
            swal.fire("表身無資料，請先匯入資料。");
            return;
        }
        InsertPaperRollBarcode(barcode);
        $('#PaperRollBarcode').select();
    });

    //條碼平張按鍵事件
    $("input[name=FlatKeyword]").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnFlatSaveBarcode").click();
        }
    });

    //儲存平張條碼按鈕
    $("#BtnFlatSaveBarcode").click(function () {
        var barcode = $('#FlatBarcode').val();
        var table = $('#FlatdataTablesBody').DataTable();
        if (status == "0") {
            swal.fire("已入庫無法儲存條碼");
            $('#FlatBarcode').select();
            return;
        }
        if (barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return;
        }
        if (table.data().length == 0) {
            swal.fire("表身無資料，請先匯入資料。");
            return;
        }
        InsertFlatBarocde(barcode);
        $('#FlatBarcode').select();

    });

    //檢查櫃號按鍵事件
    $('input[name=CabinetNumber]').keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnCabinetNumber").click();
        }
    });

    //檢查櫃號按鈕
    $('#BtnCabinetNumber').click(function (e) {
        var InputCabinetNumber = $('#CheckCabinetNumber').val();
        var ViewCabinetNumber = $("#CabinetNumber").val();
        if (status == 0) {
            return
        }
        $.ajax({
            url: '/Purchase/CheckCabinetNumber',
            datatype: 'json',
            type: "POST",
            data: { InputCabinetNumber: InputCabinetNumber, ViewCabinetNumber: ViewCabinetNumber },
            success: function (data) {
                if (data.boolean) {
                    EnableBarcode(false);
                } else {
                    swal.fire("櫃號輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });

    //存檔入庫
    $("#btnSaveInvenorty").click(function () {
        var CabinetNumber = $("#CabinetNumber").val()
        var CreateDate = $("#CreateDate").val();
        var PaperRollSpan = $('#PaperRollSpan').text();
        var FlatSpan = $('#FlatSpan').text();
        var PaperRolldata = $('#PaperRolldataTablesBody').DataTable().column(15).data();
        var Flatdata = $('#FlatdataTablesBody').DataTable().column(10).data();

        if (PaperRollSpan != "0") {
            Swal.fire({
                title: '確定要返回?',
                text: "尚有資料未入庫",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: '確定',
                cancelButtonText: '取消'
            }).then(function (result) {
                if (result.value) {
                    window.location.href = '/Purchase/Index';
                }
            });
            return;
        }

        if (FlatSpan != "0") {
            Swal.fire({
                title: '確定要返回?',
                text: "尚有資料未入庫",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: '確定',
                cancelButtonText: '取消'
            }).then(function (result) {
                if (result.value) {
                    window.location.href = '/Purchase/Index';
                }
            });
            return;
        }

        $.ajax({
            url: '/Purchase/ReturnIndex',
            type: 'POST',
            datatype: 'json',
            data: { CabinetNumber: CabinetNumber, CreateDate: CreateDate },
            success: function (data) {
                if (data.boolean) {
                    window.location.href = '/Purchase/Index';
                }
            }
        });


    });

    //範例下載
    $('#BtnDownload').click(function () {
        //$.ajax({
        //    url: '/Purchase/DownloadFile',
        //    type: "POST",
        //    datatype: "json",
        //    success: function(){}

        //});
        window.open('/Purchase/DownloadFile', '_blank');


    })
}

//儲存紙捲條碼
function InsertPaperRollBarcode(barcode) {
    $.ajax({
        "url": "/Purchase/RollSaveBarcode",
        "type": "POST",
        "datatype": "json",
        "data": { barcode: barcode },
        success: function (data) {
            if (data.status == 1) {
                swal.fire("條碼已入庫");
                return;
            }
            if (data.status == 0) {
                PaperRolldataTablesBody();
                PaperNavsNumber();
            } else if (data.status == 2) {
                swal.fire("此無條碼");
            } else {
                swal.fire("此無條碼");
            }

        }
    });

}
//儲存平張條碼
function InsertFlatBarocde(barcode) {

    $.ajax({
        "url": "/Purchase/FlatSaveBarcode",
        "type": "POST",
        "datatype": "json",
        "data": { barcode: barcode },
        success: function (data) {
            if (data.status == 1) {
                swal.fire("條碼已入庫");
                return;
            }
            if (data.status == 0) {
                FlatdataTablesBody();
                FlatNavsNumber();
            } else if (data.status == 2) {
                swal.fire("此無條碼");
            } else {
                swal.fire("此無條碼");
            }

        }
    });

}


//彈出dialog
function Open(modal_dialog) {
    modal_dialog.modal({
        backdrop: "static",
        keyboard: true,
        show: true
    });

    modal_dialog.on('hidden.bs.modal', function (e) {
        $("div").remove(modal_dialog.selector);
    });

    modal_dialog.on('show.bs.modal', function (e) {
        $.validator.unobtrusive.parse('form');
    });


    modal_dialog.on('click', '#btnImport', function (e) {
        if (modal_dialog.selector == '#PaperRollModal') {
            ImportPaperRoll();
        } else if (modal_dialog.selector == '#FlatModal') {
            ImportFlat();
        }

    });

    modal_dialog.on('click', '#btnDailogPaperRoll', function (e) {
        var table = $('#ImportPaperRollTable').DataTable();
        if (table.column(0).data().length) {
            PaperRolldataTablesBody(4);
        }
        $('#PaperRollModal').modal('hide');


    });

    modal_dialog.on('click', '#BtnCancel', function () {
        var CabinetNumber = $("#CabinetNumber").val();
        var table = $('#ImportPaperRollTable').DataTable();
        if (table.column(0).data().length) {
            $.ajax({
                "url": "/Purchase/ExcelDelete",
                "type": "POST",
                "datatype": "json",
                "data": { CabinetNumber: CabinetNumber },
                success: function (data) {
                    swal.fire(data.result.Msg);
                },
                error: function (data) {
                    swal.fire(data);
                }
            });
        }
    });

    modal_dialog.on('click', '#btnDailogFlat', function (e) {
        $('#FlatModal').modal('hide');
        FlatdataTablesBody();
    });


    modal_dialog.modal('show');

}

//判斷匯入資料是否成功
function ImportPaperRoll() {


    var fileInput = $('#file').get(0).files;
    var CabinetNumber = $("#CabinetNumber").val();
    var formData = new FormData();
    if (fileInput.length > 0) {
        formData.append("file", fileInput[0]);
        formData.append("CabinetNumber", CabinetNumber);
    }
    $.ajax({
        "url": "/Purchase/UploadFileRoll",
        "type": "POST",
        "datatype": "json",
        "data": formData,
        success: function (data) {
            if (data.result.Success) {
                ImportPaperRollTable(data);
            } else {
                swal.fire(data.result.Msg);
            }


        },
        error: function (data) {
            swal.fire(data);
        },
        cache: false,
        contentType: false,
        processData: false
    });

}

//顯示dailog紙捲資料
function ImportPaperRollTable(data) {

    $('#ImportPaperRollTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        data: data.data,
        destroy: true,
        autoWidth: false,
        success: function () {

        },
        error: function () {
            $.swal.fire("失敗");
        },
        columns: [
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "Locator", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "BaseWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
            { data: "TheoreticalWeight", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "LotNumber", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },


        ]
    });

}

function ImportFlat() {
    var fileInput = $('#file').get(0).files;

    var formData = new FormData();
    if (fileInput.length > 0) { formData.append("file", fileInput[0]); }

    $.ajax({
        "url": "/Purchase/UploadFileFlat",
        "type": "POST",
        "datatype": "json",
        "data": formData,
        success: function (data) {
            if (data.result.Success) {
                ImportFlatTable(data);
            } else {
                swal.fire(data.result.Msg);
            }
        },
        error: function (data) {
            swal.fire(data);
        },
        cache: false,
        contentType: false,
        processData: false
    });

}

function ImportFlatTable(data) {
    $('#ImportFlatTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        data: data.data,
        destroy: true,
        autoWidth: false,
        processing: true,
        error: function () {
            $.swal.fire("失敗");
        },
        columns: [
            { data: "Id", "name": "項次", "autoWidth": true },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true },
            { data: "Locator", "name": "儲位", "autoWidth": true },
            { data: "Item_No", "name": "料號", "autoWidth": true },
            { data: "ReamWeight", "name": "令重", "autoWidth": true },
            { data: "PackingType", "name": "包裝方式", "autoWidth": true },
            { data: "Pieces_Qty", "name": "每件令數", "autoWidth": true },
            { data: "Qty", "name": "數量(頓)", "autoWidth": true },
            { data: "Status", "name": "入庫狀態", "autoWidth": true }
        ]
    });
}

//紙捲表頭
function LoadPaperRollHeard() {
    var CabinetNumber = $("#CabinetNumber").val();
    var Status = $("#Status").val();
    //紙捲表頭
    PaperRolldataTablesHeader = $('#PaperRolldataTablesHeader').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Purchase/RollHeader",
            "type": "POST",
            "datatype": "json",
            "data": { CabinetNumber: CabinetNumber, Status: Status }
        },
        columns: [
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "Locator", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "BaseWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
            { data: "RollReamQty", "name": "捲數/棧板數", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionQuantity", "name": "交易數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionUom", "name": "交易單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "PrimanyQuantity", "name": "主要數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUom", "name": "主要單位", "autoWidth": true, "className": "dt-body-center" }
        ]
    });

}
//平張表頭
function LoadFlatHeader() {
    //平張表頭
    var CabinetNumber = $("#CabinetNumber").val();
    var Status = $("#Status").val();
    FlatdataTablesHeader = $('#FlatdataTablesHeader').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Purchase/FlatHeader",
            "type": "POST",
            "datatype": "json",
            "data": { CabinetNumber: CabinetNumber, Status: Status},
        },
        columns: [
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "Locator", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "ReamWeight", "name": "令重", "autoWidth": true, "className": "dt-body-right" },
            { data: "RollReamQty", "name": "捲數/棧板數", "autoWidth": true, "className": "dt-body-right" },
            { data: "PackingType", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center" },
            { data: "Pieces_Qty", "name": "每件令數", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionQuantity", "name": "交易數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionUom", "name": "交易單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "TtlRollReam", "name": "總令數", "autoWidth": true, "className": "dt-body-right" },
            { data: "TtlRollReamUom", "name": "總令數單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "DeliveryQty", "name": "總公斤", "autoWidth": true, "className": "dt-body-right" },
            { data: "DeliveryUom", "name": "總公斤單位", "autoWidth": true, "className": "dt-body-center" },
        ]
    });
}

//紙捲表身
function PaperRolldataTablesBody() {
    var CabinetNumber = $("#CabinetNumber").val();
    var Status = $("#Status").val();
    $('#PaperRolldataTablesBody').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        //scrollX: true,
        destroy: true,
        autoWidth: false,
        serverSide: true,
        processing: true,
        columnDefs: [{
            orderable: false, targets: [0, 17], width: "70px",
        }],
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        ajax: {
            "url": "/Purchase/RollBody",
            "type": "POST",
            "datatype": "json",
            "data": { Status: Status, CabinetNumber: CabinetNumber }
        },
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "Locator", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "Status", "name": "入庫狀態", "autoWidth": true, "className": "dt-body-center", "render": function (data) {
                    if (data == "已入庫") {
                        return data;
                    } else {
                        return '<span style="color:red">' + data + '</span>';
                    }
                }
            },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "BaseWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
            { data: "TheoreticalWeight", "name": "理論重(KG)", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionQuantity", "name": "交易數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionUom", "name": "交易單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "PrimanyQuantity", "name": "主要數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUom", "name": "主要單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "LotNumber", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Remark", "name": "備註", "autoWidth": true, "className": "dt-body-center" },
            {
                data: null, "width": "40px", "render": function (data) {
                    if (Status == "1") {
                        return '<button class="btn btn-info btn-sm btn-edit">編輯</button>';
                    } else if (Status == "0") {
                        return '<button class = "btn btn-primary btn-sm" id = "btnView">檢視</button>';
                    } else {
                        return '<button class="btn btn-info btn-sm btn-edit">編輯</button>';
                    }
                }
            }
        ]
    });
}

//平張表身
function FlatdataTablesBody() {
    var Status = $("#Status").val();
    var CabinetNumber = $("#CabinetNumber").val();
    $('#FlatdataTablesBody').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        //scrollX: true,
        destroy: true,
        autoWidth: false,
        serverSide: true,
        processing: true,
        columnDefs: [{
            orderable: false, targets: [0, 12], width: "70px"
        }],
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Purchase/FlatBody",
            "type": "POST",
            "datatype": "json",
            "data": { Status: Status, CabinetNumber: CabinetNumber }
        },
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "Locator", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "Status", "name": "入庫狀態", "autoWidth": true, "className": "dt-body-center", "render": function (data) {
                    if (data == "已入庫") {
                        return data;
                    } else {
                        return '<span style="color:red">' + data + '</span>';
                    }

                }
            },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "ReamWeight", "name": "令重", "autoWidth": true, "className": "dt-body-right" },
            { data: "PackingType", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center" },
            { data: "Pieces_Qty", "name": "每件令數", "autoWidth": true, "className": "dt-body-right" },
            { data: "Qty", "name": "數量(頓)", "autoWidth": true, "className": "dt-body-right" },
            { data: "Remark", "name": "備註", "autoWidth": true, "className": "dt-body-center" },
            {
                data: null, "width": "40px", "render": function (data) {
                    if (Status == "1") {
                        return '<button class="btn btn-info btn-sm btn-edit">編輯</button>';
                    } else if (Status == "0") {
                        return '<button class = "btn btn-primary btn-sm" id = "btnView">檢視</button>';
                    } else {
                        return '<button class="btn btn-info btn-sm btn-edit">編輯</button>';
                    }
                }
            }
        ],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
    });
}

//等待0.1秒後載入頁籤數字
$(window).load(function () {
    setTimeout(function () {
        PaperNavsNumber();
        FlatNavsNumber();
        //Something you want delayed.
    }, 100);//How long do you want the delay to be (in milliseconds)? 

});


function EnableBarcode(boolean) {
    $('#PaperRollBarcode').attr('disabled', boolean);
    $('#FlatBarcode').attr('disabled', boolean);
    $("#BtnImportRoll").attr('disabled', boolean);
}