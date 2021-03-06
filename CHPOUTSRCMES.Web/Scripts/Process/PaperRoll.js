var PaperRollInvestDataTables
var PaperRollProductionDataTables
var EditorInvest
var EditorProduction

/// <summary>
/// 待核准
/// </summary>
const PendingBatch = "2";
/// <summary>
/// 已完工
/// </summary>
const CompletedBatch = "3";
/// <summary>
/// 關帳
/// </summary>
const CloseBatch = "4";
/// <summary>
/// 已修改
/// </summary>
const Modified = "5";

$(document).ready(function () {

    BtnRecord();
    init();
    LoadPaperRollInvestDataTable();
    LoadPaperRollProductionDataTable();
    onclick();

    var Status = $('#Status').val();
    if (Status == CompletedBatch) {
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
        $('#BtnProcess_Batch_no').hide();
        $("#BtnSave").hide();
        $('#BtnEdit').show();
        $('#BtnApprove').hide();
        $('#OutputBathNoArea').hide();
        ///隱藏按鈕
        PaperRollInvestDataTables.column(9).visible(false);
        PaperRollProductionDataTables.column(8).visible(false);
    }
    else if (Status == PendingBatch) {
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
        $('#BtnCheckBatchNo').show();
        $('#BtnEdit').hide();
        $('#BtnApprove').show();
        $('#BtnSave').hide();
    }
    else if (Status == Modified) {
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
        PaperRollInvestDataTables.column(9).visible(false);
        PaperRollProductionDataTables.column(8).visible(false);

        $('#BtnSave').hide();
        $('#InputBathNoArea').hide();
        $('#OutputBathNoArea').hide();
    }
    else if (Status == CloseBatch) {
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
        PaperRollInvestDataTables.column(9).visible(false);
        PaperRollProductionDataTables.column(8).visible(false);

        $('#BtnSave').hide();
        $('#InputBathNoArea').hide();
        $('#OutputBathNoArea').hide();
    } else {
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
        $('#BtnProcess_Batch_no').show();
        $('#BtnEdit').hide();
        $('#BtnApprove').hide();
    }


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#PaperRollInvestDataTables').DataTable().table().node().style.width = '';


        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });

    PaperRollInvestDataTables.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        EditorInvest.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });

    PaperRollInvestDataTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        EditorInvest.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
    });

    PaperRollProductionDataTables.on('click', '#btnDeleteProductionTable', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        EditorProduction.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });

    PaperRollProductionDataTables.on('click', '#btnEdit', function (e) {
        DeleteRateLoss();
        e.preventDefault();
        EditorProduction.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });

        EditorProduction.on('preSubmit', function (e, d) {
            var Weight = this.field('PrimaryQuantity');
            if (Weight.val() === '') {
                this.field('PrimaryQuantity').error('請勿空白');
                return false;
            }
            return true;
        });
    });


})

function init() {

    //條碼欄位事件
    $('#PaperRoll_Invest_Barcode').change(function (e) {
        var Barcode = $('#PaperRoll_Invest_Barcode').val();
        var OspDetailInId = $("#OspDetailInId").val();
        $.ajax({
            url: '/Process/CheckStockBarcode',
            type: 'post',
            datatype: 'json',
            data: { Barcode: Barcode, OspDetailInId: OspDetailInId },
            success: function (data) {
                if (data.resultDataModel.Success == false) {
                    swal.fire(data.resultDataModel.Msg);
                    ClearTextPaperRoll();
                } else {
                    PaperRollDispalyText(data);
                }
            },
            error: function () {
                swal.fire("檢查庫存條碼失敗");
            }
        });
    });


    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#PaperRoll_Production_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $('#BtnPaperRollBarcodeSave').click();
        }
    });




    $("#Process_Batch_no").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnOrderNumber").click();
        }
    });


}

function onclick() {


    //列印標籤紙捲
    $('#BtnRePrint').click(function () {
        var Status = "捲筒"
        PrintLableParameter(PaperRollInvestDataTables, "/Process/RePrintLabel", "2", Status);
    });

    //列印入庫單
    $('#BtnPurchase').click(function () {
        var table = $('#PaperRollProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }
        var OspHeaderId = $('#OspHeaderId').val();
        window.open("/Home/OspPaperRollerStock/?OspHeaderId=" + OspHeaderId);
    });

    //列印成品標籤紙捲
    $('#BtnLabel').click(function () {
        var table = $('#PaperRollProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }
        PrintLable(PaperRollProductionDataTables, "/Process/GePaperRollerProductLabels", "1");
    });


    //投入
    $('#BtnProcess_Batch_no').click(function (e) {
        var BatchNo = $('#ProcessBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        ShowWait(function () {
            $.ajax({
                url: '/Process/CheckBatchNo',
                datatype: 'json',
                type: "POST",
                data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
                success: function (data) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        DisplayInvestPaperRollEnable(false);
                    } else {
                        swal.fire(data.resultModel.Msg);
                    }
                },
                error: function () {
                    swal.fire('檢查工單號失敗')
                }
            });
        });
    });

    //投出驗證單號
    $('#BtnProcess_Production_Batch_no').click(function (e) {
        var BatchNo = $('#ProcessProductionBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        ShowWait(function () {
            $.ajax({
                url: '/Process/CheckBatchNo',
                datatype: 'json',
                type: "POST",
                data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
                success: function (data) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        DisplayProductionPaperRollEnable(false);
                    } else {
                        swal.fire(data.resultModel.Msg);
                    }
                },
                error: function () {
                    swal.fire('檢查工單號失敗')
                }
            });
        });
    });



    //儲存條碼
    $('#Btn_PaperRoll_ProcessSave').click(function () {
        var Barcode = $('#PaperRoll_Invest_Barcode').val().trim();
        var Remnant = $('#PaperRoll_Invest_Remnant').val();
        var Remaining_Weight = "";
        var OspDetailInId = $("#OspDetailInId").val();
        if (Barcode.length == 0) {
            swal.fire("投入條碼不得空白");
            return;
        }
        if (Remnant == "1") {
            if (Remaining_Weight.length == 0) {
                swal.fire("餘重不得空白");
                return;
            }

        }
        PaperInvestSaveBarcode(Barcode, Remnant, Remaining_Weight, OspDetailInId);
        $('#Btn_PaperRoll_ProcessSave').select();
    });


    //產出產生明細
    $('#Btn_PaperRoll_Product_detail').click(function () {
        var PaperRoll_Weight = $('#PaperRoll_Weight').val().trim();
        var PaperRoll_Lot_Number = $('#PaperRoll_Lot_Number').val().trim();
        var PaperRollInvestDataTables = $('#PaperRollInvestDataTables').DataTable().data();
        var OspDetailOutId = $("#OspDetailOutId").val();


        if (PaperRollInvestDataTables.length == 0) {
            swal.fire("請先新增投入條碼。");
            return;
        }

        if (PaperRoll_Weight.length == 0) {
            swal.fire("重量不得空白");
            return;
        }
        if (PaperRoll_Lot_Number.length == 0) {
            swal.fire("捲號不得空白");
            return;
        }

        PaperRollProductionDetail(PaperRoll_Weight, PaperRoll_Lot_Number, OspDetailOutId);

    })

    $('#BtnPaperRollBarcodeSave').click(function () {

        var Production_Barcode = $('#PaperRoll_Production_Barcode').val();
        var table = $('#PaperRollProductionDataTables').DataTable();
        var OspDetailOutId = $("#OspDetailOutId").val();
        if (Production_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return;
        }
        if (table.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return;
        }
        PaperRollChangeProductionStauts(Production_Barcode, OspDetailOutId);
        $('#PaperRoll_Production_Barcode').select();
    })

    //計算損失
    $('#BtnCalculate').click(function () {
        var OspDetailInId = $("#OspDetailInId").val()
        var OspDetailOutId = $("#OspDetailOutId").val()
        var table = $('#PaperRollProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。")
            return
        }

        ShowWait(function () {
            $.ajax({
                "url": "/Process/Loss",
                "type": "POST",
                "datatype": "json",
                "data": { OspDetailInId: OspDetailInId, OspDetailOutId: OspDetailOutId },
                success: function (data) {
                    if (data.resultDataModel.Success) {
                        CloseWait();
                        $('#InvestWeight').html(data.resultDataModel.Data.InvestWeight);
                        $('#ProductWeight').html(data.resultDataModel.Data.ProductWeight);
                        $('#Production_Loss').html(data.resultDataModel.Data.LossWeight);
                        $('#Rate').html(data.resultDataModel.Data.Rate);
                    } else {
                        swal.fire(data.resultDataModel.Msg)
                    }
                },
                error: function () {
                    swal.fire('計算損耗失敗')
                }
            });
        });
    });

    $('#BtnReturn').click(function () {
        window.location.href = '/Process/Index';

    });

    $('#BtnSave').click(function () {
        var loss = $('#Production_Loss').text();
        var Process_Batch_no = $('#Process_Batch_no').text()
        var OspDetailOutId = $("#OspDetailOutId").val();
        var PaperRollInvestDataTables = $('#PaperRollInvestDataTables').DataTable().data();

        if (PaperRollInvestDataTables.length == 0) {
            swal.fire("請先新增投入資料。");
            return;
        }

        var table = $('#PaperRollProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }

        if (!loss) {
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
}

//初始化投入table
function LoadPaperRollInvestDataTable() {


    EditorInvest = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/InvestEdit',
            "type": "POST",
            "dataType": "json",
            "contentType": 'application/json',
            data: function (d) {
                var InvestList = [];
                $.each(d.data, function (key, value) {

                    var Invest = {
                        'OspPickedInId': d.data[key]['OspPickedInId'],
                        'RemainingQuantity': d.data[key]['RemainingQuantity']
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
                PaperRollInvestDataTables.ajax.reload(null, false);
            }
        },
        table: "#PaperRollInvestDataTables",
        idSrc: 'OspPickedInId',
        fields: [
            {
                label: "殘捲",
                name: "HasRemaint",
                type: "select",
                options: [
                    { label: "無", value: "無" },
                    { label: "有", value: "有" },
                ]
            },
            {
                label: "餘重:",
                name: "RemainingQuantity",
                attr: {
                    type: "number",
                    min: "0"
                }
            }
            ,
            {
                label: "ID:",
                name: "OspPickedInId",
            }
        ],
        i18n: {
            edit: {
                button: "編輯",
                title: "更改原因",
                submit: "確定",
            },
            remove: {
                button: '刪除',
                title: "刪除",
                submit: "確定",
                confirm: {
                    "_": "你確定要刪除這筆資料?",
                    "1": "你確定要刪除這筆資料?"
                }
            },
        }
    });

    EditorInvest.hide('OspPickedInId');

    EditorInvest.on('preSubmit', function (e, d) {
        var Remnant
        var Remaining_Weight
        $.each(d.data, function (key, values) {
            Remnant = d.data[key]['HasRemaint']
            Remaining_Weight = d.data[key]['RemainingQuantity']
        });

        if (Remnant == "有") {
            if (Remaining_Weight == '') {
                this.field('RemainingQuantity').error('請勿空白');
                return false;
            }
        } else {

        }

        return true;
    });

    EditorInvest.dependent('HasRemaint', function (val, data, callback) {
        if (val == '無') {
            EditorInvest.field('RemainingQuantity').set('');
        }
        return val === '無' ?
            { hide: 'RemainingQuantity' } : { show: 'RemainingQuantity' };


    });

    var OspHeaderId = $("#OspHeaderId").val();
    PaperRollInvestDataTables = $('#PaperRollInvestDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
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
            'selectNone',
            {
                extend: 'excel',
                text: '匯出Excel',
                filename: function () {
                    return moment().format("YYYYMMDDHHmmss");
                }
            },
            {
                text: '刪除',
                className: "BtnAllDelete",
                enabled: false,
                name: 'delete',
                action: function () {
                    var selectRowData = PaperRollInvestDataTables.rows('.selected').data();
                    if (selectRowData.length == 0) {
                        swal.fire("請先選取刪除項目")
                        return;
                    }
                    var OspPickedInId = [];
                    var barcode = [];
                    for (i = 0; i < selectRowData.length; i++) {
                        OspPickedInId.push(selectRowData.pluck('OspPickedInId')[i]);
                        barcode.push(selectRowData.pluck('Barcode')[i])
                    }
                    swal.fire({
                        title: '刪除',
                        text: '確定要刪除??' + barcode,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "確定",
                        cancelButtonText: "取消"
                    }).then(function (result) {
                        if (result.value) {
                            ShowWait(function () {
                                $.ajax({
                                    url: '/Process/AllDelete',
                                    datatype: 'json',
                                    type: "POST",
                                    data: { OspPickedInId: OspPickedInId },
                                    success: function (data) {
                                        if (data.resultModel.Success) {
                                            CloseWait();
                                            PaperRollInvestDataTables.ajax.reload(null, false);
                                        } else {
                                            swal.fire(data.resultModel.Msg);
                                        }
                                    },
                                    error: function () {
                                        swal.fire('刪除失敗');
                                    }
                                });
                            });
                            
                        }
                    });
                }
            },
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
            { data: "OspPickedInId", "name": "ID", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "StockId", "name": "ID", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "BasicWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
            { data: "LotNumber", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "PrimaryQuantity", "name": "原重", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            }
        ],

    });



}

//儲存投入條碼
function PaperInvestSaveBarcode(Barcode, Remnant, Remaining_Weight, OspDetailInId) {
    ShowWait(function () {
        $.ajax({
            "url": "/Process/SaveInvestBarcode",
            "type": "POST",
            "datatype": "json",
            "data": { Barcode: Barcode, Remnant: Remnant, Remaining_Weight: Remaining_Weight, OspDetailInId: OspDetailInId },
            success: function (data) {
                if (data.resultModel.Success) {
                    CloseWait();
                    PaperRollInvestDataTables.ajax.reload(null, false);
                    ClearRateLoss();
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {
                swal.fire('儲存失敗');
            }

        });
    });
    
}


//初始化產出table
function LoadPaperRollProductionDataTable() {

    EditorProduction = new $.fn.dataTable.Editor({
        ajax: {
            url: '/Process/PaperRollerProductionEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var ProductionList = [];
                $.each(d.data, function (key, value) {

                    var Production = {
                        'OspPickedOutId': d.data[key]['OspPickedOutId'],
                        'PrimaryQuantity': d.data[key]['PrimaryQuantity']
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
                PaperRollProductionDataTables.ajax.reload(null, false);
            }
        },
        table: "#PaperRollProductionDataTables",
        idSrc: 'OspPickedOutId',
        fields: [
            {
                label: "重量",
                name: "PrimaryQuantity"
            },
            {
                name: "OspPickedOutId"
            },
        ],
        i18n: {
            edit: {
                button: "更改",
                title: "更改",
                submit: "確定"
            },
        }

    });

    EditorProduction.field('OspPickedOutId').hide();

    var OspHeaderId = $("#OspHeaderId").val();
    PaperRollProductionDataTables = $('#PaperRollProductionDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        destroy: true,
        processing: true,
        serverSide: true,
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
            'selectNone',
            {
                extend: 'excel',
                text: '匯出Excel',
                filename: function () {
                    return moment().format("YYYYMMDDHHmmss");
                }
            },
            {
                text: '刪除',
                className: "BtnProductionAllDelete",
                enabled: false,
                name: 'delete',
                action: function () {
                    var selectRowData = PaperRollProductionDataTables.rows('.selected').data();
                    if (selectRowData.length == 0) {
                        swal.fire("請先選取刪除項目")
                        return;
                    }
                    var OspPickedOutId = [];
                    var barcode = [];
                    for (i = 0; i < selectRowData.length; i++) {
                        OspPickedOutId.push(selectRowData.pluck('OspPickedOutId')[i]);
                        barcode.push(selectRowData.pluck('Barcode')[i])
                    }
                    swal.fire({
                        title: '刪除',
                        text: '確定要刪除??' + barcode,
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "確定",
                        cancelButtonText: "取消"
                    }).then(function (result) {
                        if (result.value) {
                            ShowWait(function () {
                                $.ajax({
                                    url: '/Process/ProductionChooseDelete',
                                    datatype: 'json',
                                    type: "POST",
                                    data: { OspPickedOutId: OspPickedOutId },
                                    success: function (data) {
                                        if (data.resultModel.Success) {
                                            CloseWait();
                                            PaperRollProductionDataTables.ajax.reload(null, false);
                                        } else {
                                            swal.fire(data.resultModel.Msg);
                                        }
                                    },
                                    error: function () {
                                        swal.fire('刪除失敗')
                                    }
                                });
                            });
                        }
                    });
                }
            }
        ],
        columnDefs: [{
            orderable: false, targets: [0, 8], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "OspPickedOutId", "name": "", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Product_Item", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "LotNumber", "name": "捲數", "autoWidth": true, "className": "dt-body-center" },
            { data: "PrimaryQuantity", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return 'KG';
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
                    return '<button class="btn btn-primary btn-sm" id= "btnEdit">編輯</button>' + '&nbsp|&nbsp' +
                        '<button class = "btn btn-danger btn-sm" id = "btnDeleteProductionTable">刪除</button>';
                }
            }
        ],

    });

}

///產生明細
function PaperRollProductionDetail(PaperRoll_Weight, PaperRoll_Lot_Number, OspDetailOutId) {
    ShowWait(function () {
        $.ajax({
            "url": "/Process/PaperRollCreateProduction",
            "type": "POST",
            "datatype": "json",
            "data": {
                PaperRoll_Weight: PaperRoll_Weight,
                PaperRoll_Lot_Number: PaperRoll_Lot_Number,
                OspDetailOutId: OspDetailOutId
            },
            success: function (data) {
                if (data != null) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        PaperRollProductionDataTables.ajax.reload(null, false);
                        ClearRateLoss();
                    } else {
                        swal.fire(data.resultModel.Msg);
                    }
                }
            },
            error: function () {
                swal.fire('產生明細失敗')
            }
        })
    });

}

//產出條碼入庫
function PaperRollChangeProductionStauts(Production_Barcode, OspDetailOutId) {
    ShowWait(function () {
        $.ajax({
            "url": "/Process/ProductionChangeStatus",
            "type": "POST",
            "datatype": "json",
            "data": { Production_Barcode: Production_Barcode, OspDetailOutId: OspDetailOutId },
            success: function (data) {
                if (data.resultModel.Success) {
                    CloseWait();
                    PaperRollProductionDataTables.ajax.reload(null, false);
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {
                swal.fire('轉已入庫失敗')
            }
        })
    });
}


function ClearTextPaperRoll() {
    $('#PaperRoll_Invest_Original_Weight').html("");
    $('#PaperRoll_Item_No').html("");
    $('#PaperRoll_Invest_Lot_Number').html("");
    $('#PaperRoll_Invest_Barcode').val('');
}

function PaperRollDispalyText(data) {
    $('#PaperRoll_Invest_Original_Weight').html(data.resultDataModel.Data.PrimaryAvailableQty);
    $('#PaperRoll_Invest_Lot_Number').html(data.resultDataModel.Data.LotNumber);
}

function EnableBarcode(boolean) {
    $('#PaperRoll_Invest_Barcode').attr('disabled', boolean);
    $('#Btn_PaperRoll_Product_detail').attr('disabled', boolean);

}

function DeleteRateLoss() {
    ClearRateLoss();
    var OspHeaderId = $('#OspHeaderId').val();
    $.ajax({
        url: '/Process/DeleteRate',
        datatype: 'json',
        type: "POST",
        data: { OspHeaderId: OspHeaderId },
        success: function (data) {
           
        },
        error: function () {
            swal.fire('刪除得率失敗')
        }
    });
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
        ShowWait(function () {
            $.ajax({
                url: '/Process/ChangeHeaderStatus/',
                dataType: 'json',
                type: 'post',
                data: { OspHeaderId: OspHeaderId, Locator: Locator },
                success: function (data) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        window.location.href = '/Process/Index';
                    } else {
                        swal.fire(data.resultModel.Msg)
                    }
                },
                error: function () {
                    swal.fire('完工存檔失敗')
                }
            });
        });
    });

    modal_dialog.modal('show');

}

function OpenApproveDialog(modal_dialog) {
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
        ShowWait(function () {
            $.ajax({
                url: '/Process/ApproveHeaderStauts/',
                dataType: 'json',
                type: 'post',
                data: { OspHeaderId: OspHeaderId, Locator: Locator },
                success: function (data) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        window.location.href = '/Process/Index';
                    } else {
                        swal.fire(data.resultModel.Msg)
                    }

                },
                error: function () {
                    swal.fire('核准失敗')
                }
            });
        });
    });

    modal_dialog.modal('show');

}

function DisplayInvestPaperRollEnable(boolean) {
    $('#PaperRoll_Invest_Barcode').attr('disabled', boolean);
    $('#Btn_PaperRoll_ProcessSave').attr('disabled', boolean);
    $('#BtnRePrint').attr('disabled', boolean);
    PaperRollInvestDataTables.button(3).enable(!boolean);
}

function DisplayProductionPaperRollEnable(boolean) {
    $('#PaperRoll_Weight').attr('disabled', boolean);
    $('#PaperRoll_Specification').attr('disabled', boolean);
    $('#PaperRoll_Lot_Number').attr('disabled', boolean);
    $('#Btn_PaperRoll_Product_detail').attr('disabled', boolean);
    $('#PaperRoll_Production_Barcode').attr('disabled', boolean);
    $('#BtnPaperRollBarcodeSave').attr('disabled', boolean);
    $('#BtnCalculate').attr('disabled', boolean);
    $('#BtnLabel').attr('disabled', boolean);
    $('#BtnPurchase').attr('disabled', boolean);
    PaperRollProductionDataTables.button(3).enable(!boolean);

}


function DsiplayPaperRollHide() {
    $('#BtnProcess_Batch_no').hide()
    $("#BtnSave").hide();
    $('#BtnEdit').show()
    $('#BtnApprove').show()

}

function DsiplayPaperRollShow(Status) {
    $('#BtnProcess_Batch_no').show()
    $('#BtnEdit').hide()
    if (Status == CompletedBatch) {
        $('#BtnApprove').hide()
    }
}

function ClearRateLoss() {
    $('#InvestWeight').html("");
    $('#ProductWeight').html("");
    $('#Production_Loss').html("");
    $('#Rate').html("");
}

//完工紀錄使用
function BtnRecord() {
    $('#BtnEdit').click(function () {
        var BatchNo = $('#ProcessBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        ShowWait(function () {
            $.ajax({
                url: '/Process/FinishedEdit',
                datatype: 'json',
                type: "POST",
                data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
                success: function (data) {
                    if (data.Success) {
                        CloseWait();
                        location.href = "/Process/PaperRoll/" + data.Data;
                    } else {
                        swal.fire(data.Msg);
                    }
                },
                error: function () {
                    swal.fire('修改失敗');
                }
            });
        });
    });

    $('#BtnApprove').click(function () {

        var loss = $('#Production_Loss').text();
        var OspDetailOutId = $("#OspDetailOutId").val();
        var PaperRollInvestDataTables = $('#PaperRollInvestDataTables').DataTable().data();

        if (PaperRollInvestDataTables.length == 0) {
            swal.fire("請先新增投入資料。");
            return;
        }

        var table = $('#PaperRollProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }

        if (!loss) {
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
                OpenApproveDialog($('#SubinventoryModal'));
            }
        });

    });

}
//完工紀錄使用
function changeStaus() {
    var OspHeaderId = $("#OspHeaderId").val();
    ShowWait(function () {
        $.ajax({
            url: '/Process/EditHeaderStatus/',
            dataType: 'json',
            type: 'post',
            data: { OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.resultModel.Success) {
                    CloseWait();
                    DisplayInvestPaperRollEnable(false);
                    DisplayProductionPaperRollEnable(false);
                    PaperRollInvestDataTables.column(9).visible(true);
                    PaperRollProductionDataTables.column(8).visible(true);
                } else {
                    swal.fire(data.resultModel.Msg)
                }
            },
            error: function () {
                swal.fire('完工紀錄工單號狀態更改失敗')
            }

        });
    });
    
}