var FlatInvestTable
var FlatProductionDataTables
var EditorFlatProduction
var EditorFlatInvest

$(document).ready(function () {
    BtnRecord();
    init();
    onBtnClick();
    LoadFlatInvestTable();
    LoadFlatProductionDataTable();

    var Status = $('#Status').val();
    if (Status == "已完工" || Status == "待核准") {
         //完工紀錄使用
        DsiplayFlatHide();
        DisplayInvestFlatEnable(true);
        DisplayProductionFlatEnable(true);
        ///隱藏按鈕
        FlatInvestTable.column(6).visible(false);
        FlatProductionDataTables.column(9).visible(false);
    } else {
        DisplayInvestFlatEnable(true);
        DisplayProductionFlatEnable(true);
        DsiplayFlatShow();
    }


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#FlatInvestDataTables').DataTable().table().node().style.width = '';
        $('#FlatProductionDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });

    FlatInvestTable.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        $('#Rate').html("");
        EditorFlatInvest.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });

    FlatProductionDataTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        $('#Rate').html("");
        EditorFlatProduction.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });

        EditorFlatProduction.on('preSubmit', function (e, d) {
            var Roll_Ream_Wt = this.field('SecondaryQuantity');
            if (Roll_Ream_Wt.val() === '') {
                this.field('SecondaryQuantity').error('請勿空白');
                return false;
            }

            return true;
        });

    });

    FlatProductionDataTables.on('click', '#btnDeleteFlatProductionTable', function (e) {
        $('#Production_Loss').html("");
        $('#Rate').html("");
        e.preventDefault();
        EditorFlatProduction.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });


})





function init() {
    //交換事件
    $('#Flat_Invest_Barcode').change(function (e) {
        var OspDetailInId = $("#OspDetailInId").val();
        var Barcode = $('#Flat_Invest_Barcode').val()
        $.ajax({
            url: '/Process/CheckStockBarcode',
            type: 'post',
            datatype: 'json',
            data: { Barcode: Barcode, OspDetailInId: OspDetailInId },
            success: function (data) {
                if (data.resultDataModel.Success == false) {
                    swal.fire("條碼無資料");
                    FlatClearText();
                } else {
                    FlatDispalyText(data);
                }
            },
            error: function () {
                swal.fire("失敗");
            }
        });

    });

    $("#Flat_Invest_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnFlat_ProcessSave").click();
        }
    });

    //在關鍵字input按下Enter，執行n送出 
    $("#Flat_Production_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $('#BtnFlatBarcodeSave').click();
        }
    });


    $("#Process_Batch_no").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnOrderNumber").click();
        }
    });


}

function onBtnClick() {

        //投入驗證單號
    $('#BtnProcess_Batch_no').click(function (e) {
        var BatchNo = $('#ProcessBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        $.ajax({
            url: '/Process/CheckBatchNo',
            datatype: 'json',
            type: "POST",
            data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.resultModel.Success) {
                    DisplayInvestFlatEnable(false);
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {

            }
        });

    });


    //投出驗證單號
    $('#BtnProcess_Production_Batch_no').click(function (e) {
        var BatchNo = $('#ProcessProductionBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        $.ajax({
            url: '/Process/CheckBatchNo',
            datatype: 'json',
            type: "POST",
            data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.resultModel.Success) {
                    DisplayProductionFlatEnable(false);
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {

            }
        });

    });



    $('#BtnFlat_ProcessSave').click(function () {
        var Barcode = $('#Flat_Invest_Barcode').val().trim();
        var Remnant = "";
        var Remaining_Weight = "";
        var OspDetailInId = $("#OspDetailInId").val()
        if (Barcode.length == 0) {
            swal.fire("投入條碼不得空白");
            return;
        }

        FlatInvestSaveBarcode(Barcode, Remnant, Remaining_Weight, OspDetailInId);
        $('#Flat_Invest_Barcode').select();
    });


    //產出產生明細
    $('#BtnFlat_Product_detail').click(function () {
        var Flat_Production_Roll_Ream_Qty = $('#Flat_Production_Roll_Ream_Qty').val().trim();
        var Flat_Production_Roll_Ream_Wt = $('#Flat_Production_Roll_Ream_Wt').val().trim();
        var FlatInvestDataTables = $('#FlatInvestDataTables').DataTable().data();
        var OspDetailOutId = $("#OspDetailOutId").val();


        if (FlatInvestDataTables.length == 0) {
            swal.fire("請先新增投入條碼。");
            return;
        }

        if (Flat_Production_Roll_Ream_Qty.length == 0) {
            swal.fire("棧板數不得空白");
            return;
        }
        if (Flat_Production_Roll_Ream_Wt.length == 0) {
            swal.fire("令數不得空白");
            return;
        }

        FlatProductionDetail(Flat_Production_Roll_Ream_Qty, Flat_Production_Roll_Ream_Wt, OspDetailOutId);

    })
      //產出入庫
    $('#BtnFlatBarcodeSave').click(function () {
        var Production_Barcode = $('#Flat_Production_Barcode').val();
        var table = $('#FlatProductionDataTables').DataTable();
        var OspDetailOutId = $("#OspDetailOutId").val()
        if (Production_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return
        }
        if (table.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return
        }
        ChangeFlatProductionStauts(Production_Barcode, OspDetailOutId);
        $('#Flat_Production_Barcode').select();
    })

    //計算損耗
    $('#BtnCalculate').click(function () {
        var OspDetailInId = $("#OspDetailInId").val()
        var OspDetailOutId = $("#OspDetailOutId").val()
        var Productiontable = $('#FlatProductionDataTables').DataTable();
        if (FlatInvestTable.data().length == 0) {
            swal.fire("投入無資料，請先輸入資料。")
            return
        }
        if (Productiontable.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。")
            return
        }


        $.ajax({
            "url": "/Process/Loss",
            "type": "POST",
            "datatype": "json",
            "data": { OspDetailInId: OspDetailInId, OspDetailOutId: OspDetailOutId },
            success: function (data) {
                if (data.resultDataModel.Success) {
                    $('#Production_Loss').html(data.resultDataModel.Data.LossWeight);
                    $('#Rate').html(data.resultDataModel.Data.Rate + "%");
                } else {
                    swal.fire(data.resultDataModel.Msg)
                }
            }
        });

    });


    $('#BtnSave').click(function () {
        var loss = $('#Production_Loss').text();
        var Process_Batch_no = $('#Process_Batch_no').text()
        var OspDetailOutId = $("#OspDetailOutId").val();
        if (loss == 0) {
            swal.fire("損耗量未計算");
            return;
        }


        $.ajax({
            url: '/Process/_Locator/',
            type: 'POST',
            datatype: 'json',
            data: { OspDetailOutId: OspDetailOutId },
            success: function (result) {
                $('body').append(result);
                Open($('#SubinventoryModal'));
            }
        });

    });

    //列印標籤紙捲
    $('#BtnRePrint').click(function () {
        var Status = "平版"
        PrintLableParameter(FlatInvestTable, "/Process/RePrintLabel", "2", Status);
    });



    //列印標籤紙捲
    $('#BtnLabel').click(function () {
        PrintLable(FlatProductionDataTables, "/Process/GeProductLabels", "1");
    });


}

//投入初始化table
function LoadFlatInvestTable() {

    EditorFlatInvest = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/InvestEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var InvestList = [];
                $.each(d.data, function (key, value) {

                    var Invest = {
                        'OspPickedInId': d.data[key]['OspPickedInId'],
                        'Remnant': d.data[key]['Remnant'],
                        'Remaining_Weight': d.data[key]['Remaining_Weight']
                    }
                    InvestList.push(Invest);
                });

                var data = {
                    'Action': d.action,
                    'InvestList': InvestList
                }

                return JSON.stringify(data);
            },
            success: function () {
                LoadFlatInvestTable();
            }
        },
        table: "#FlatInvestDataTables",
        idSrc: 'OspPickedInId',
        fields: [
            {
                name: "OspPickedInId"
            },
        ],
    });

    EditorFlatInvest.hide('OspPickedInId');


    var OspHeaderId = $("#OspHeaderId").val()
    FlatInvestTable = $('#FlatInvestDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/InvestLoadTable",
            "type": "POST",
            "datatype": "json",
            "data": { OspHeaderId: OspHeaderId }
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        columnDefs: [{
            orderable: false, targets: [0, 6], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "OspPickedInId", "name": "ID", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "StockId", "name": "ID", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "InventoryItemNumber", "name": "料號", "autoWidth": true, "className": "dt-body-left"},
            { data: "SecondaryQuantity", "name": "令數(RE)", "autoWidth": true, "className": "dt-body-right"},
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            }
        ],

    });
}

//投入條碼儲存
function FlatInvestSaveBarcode(Barcode, Remnant, Remaining_Weight, OspDetailInId) {
    $.ajax({
        "url": "/Process/SaveInvestBarcode",
        "type": "POST",
        "datatype": "json",
        "data": { Barcode: Barcode, Remnant: Remnant, Remaining_Weight:Remaining_Weight,OspDetailInId: OspDetailInId},
        success: function (data) {
            if (data.resultModel.Success) {
                LoadFlatInvestTable();
            } else {
                swal.fire(data.resultModel.Msg);
            }
        }


    });

}

//產出初始化table
function LoadFlatProductionDataTable() {

    EditorFlatProduction = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/ProductionEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var ProductionList = [];
                $.each(d.data, function (key, value) {

                    var Production = {
                        'OspPickedOutId': d.data[key]['OspPickedOutId'],
                        'SecondaryQuantity': d.data[key]['SecondaryQuantity']

                    }
                    ProductionList.push(Production);
                });

                var data = {
                    'Action': d.action,
                    'ProductionList': ProductionList
                }

                return JSON.stringify(data);
            },
            success: function () {
                LoadFlatProductionDataTable();
            }
        },
        table: "#FlatProductionDataTables",
        idSrc: 'OspPickedOutId',
        fields: [
            //{
            //    label: "重量",
            //    name: "Weight"
            //},
            {
                name: "OspPickedOutId"
            },
            {
                label: "令數",
                name: "SecondaryQuantity"
            }
        ],
    });

    EditorFlatProduction.field('OspPickedOutId').hide();


    var OspHeaderId = $("#OspHeaderId").val()
    FlatProductionDataTables = $('#FlatProductionDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/ProductionLoadDataTables",
            "type": "POST",
            "datatype": "json",
            "data": { OspHeaderId: OspHeaderId },
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        columnDefs: [{
            orderable: false, targets: [0, 9], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "OspPickedOutId", "name": "條碼號", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Product_Item", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PrimaryQuantity", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "PrimaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return 'KG';
                }
            },
            { data: "SecondaryQuantity", "name": "令數", "autoWidth": true, "className": "dt-body-right"},
            {
                data: "SecondaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return 'RE';
                }
            },
            {
                data: "Status", "name": "狀態", "autoWidth": true, "className": "dt-body-center", "render": function (data) {
                    if (data == "已入庫") {
                        return data;
                    } else {
                        return '<span style="color:red">' + data + '</span>';
                    }
                }
            },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-primary btn-sm" id = "btnEdit">編輯</button>' +
                        '&nbsp|&nbsp'+ '<button class = "btn btn-danger btn-sm" id = "btnDeleteFlatProductionTable">刪除</button>';
                }
            }
        ],

    });

}

//產生明細
function FlatProductionDetail(Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, OspDetailOutId) {

    $.ajax({
        "url": "/Process/CreateProduction",
        "type": "POST",
        "datatype": "json",
        "data": {
            Production_Roll_Ream_Qty: Production_Roll_Ream_Qty,
            Production_Roll_Ream_Wt: Production_Roll_Ream_Wt,
            OspDetailOutId: OspDetailOutId
        },
        success: function (data) {
            if (data.resultModel.Success) {
                LoadFlatProductionDataTable();
            } else {
                swal.fire(data.resultModel.Msg);
            }
        }
    })

}
///產出條碼待入庫
function ChangeFlatProductionStauts(Production_Barcode, OspDetailOutId) {
    $.ajax({
        "url": "/Process/ProductionChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { Production_Barcode: Production_Barcode, OspDetailOutId: OspDetailOutId },
        success: function (data) {
            if (data.resultModel.Success) {
                LoadFlatProductionDataTable();
            } else {
                swal.fire(data.resultModel.Msg);
            }
        }
    });
}


function FlatDispalyText(data) {
}

function FlatClearText() {
 
    $('#Flat_Invest_Barcode').val('');
}

function EnableBarcode(boolean) {
    $('#Flat_Invest_Barcode').attr('disabled', boolean);
    $('#BtnFlat_Product_detail').attr('disabled', boolean);

}


//彈出dialog
function Open(modal_dialog, Process_Batch_no) {
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

    //確認按鍵
    modal_dialog.on('click', '#btnSaveSubinventory', function (e) {
        var Locator = $('#dialg_Locator').val();
        var OspHeaderId = $("#OspHeaderId").val();
        $.ajax({
            url: '/Process/ChangeHeaderStauts/',
            dataType: 'json',
            type: 'post',
            data: { OspHeaderId: OspHeaderId, Locator: Locator },
            success: function (data) {
                if (data.resultModel.Success) {
                    window.location.href = '/Process/Index';
                } else {
                    swal.fire(data.resultModel.Msg)
                }

            }
        });
    });



    modal_dialog.modal('show');

}
//完工紀錄使用
function BtnRecord() {
    $('#BtnEdit').click(function () {
        var BatchNo = $('#ProcessBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        $.ajax({
            url: '/Process/FinisheEdit',
            datatype: 'json',
            type: "POST",
            data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId},
            success: function (data) {
                if (data.resultModel.Success) {
                    changeStaus();
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {

            }
        });
    });

    $('#BtnApprove').click(function () {
        var OspHeaderId = $("#OspHeaderId").val();
        $.ajax({
            url: '/Process/ApproveHeaderStauts/',
            dataType: 'json',
            type: 'post',
            data: { OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.resultModel.Success) {
                    window.location.href = '/Process/Index';
                } else {
                    swal.fire(data.resultModel.Msg)
                }
            }
        });


    });

}



function DisplayInvestFlatEnable(boolean) {
    $('#Flat_Invest_Barcode').attr('disabled', boolean);
    $('#BtnFlat_ProcessSave').attr('disabled', boolean);


    
}

function DisplayProductionFlatEnable(boolean) {
    $('#Flat_Production_Roll_Ream_Qty').attr('disabled', boolean);
    $('#Flat_Production_Roll_Ream_Wt').attr('disabled', boolean);
    $('#BtnFlat_Product_detail').attr('disabled', boolean);
    $('#Flat_Production_Barcode').attr('disabled', boolean);
    $('#BtnFlatBarcodeSave').attr('disabled', boolean);
    $('#BtnCalculate').attr('disabled', boolean);
    $('#BtnPurchase').attr('disabled', boolean);
    $('#BtnLabel').attr('disabled', boolean);

}

function DsiplayFlatHide() {
    $('#BtnProcess_Batch_no').hide()
    $("#BtnSave").hide();
    $('#BtnEdit').show()
    $('#BtnApprove').show()

}

function DsiplayFlatShow() {
    $('#BtnProcess_Batch_no').show()
    $('#BtnEdit').hide()
    $('#BtnApprove').hide()
}


//完工紀錄使用
function changeStaus() {
    var OspHeaderId = $("#OspHeaderId").val();
    $.ajax({
        url: '/Process/EditHeaderStauts/',
        dataType: 'json',
        type: 'post',
        data: { OspHeaderId: OspHeaderId },
        success: function (data) {
            if (data.resultModel.Success) {
                DisplayInvestFlatEnable(false);
                DisplayProductionFlatEnable(false);
                FlatInvestTable.column(6).visible(true);
                FlatProductionDataTables.column(9).visible(true);
            } else {
                swal.fire(data.resultModel.Msg)
            }

        }
    });
}