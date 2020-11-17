

var InvestDataTables
var ProductionTables
var CotangentDataTable
var EditorCotangent
var editorInvest
var editorProduct
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
    LoadInvestDataTable();
    LoadProductionDataTable();
    init();
    BtnClick();
    BtnRecordEdit();
    CotangentDataTables();

    var Status = $('#Status').val();
    if (Status == CompletedBatch) {
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
        $('#BtnCheckBatchNo').hide()
        $('#BtnSave').hide();
        $('#BtnEdit').show();
        $('#BtnApprove').hide();
        $('#OutputBathNoArea').hide();
        //$('#BtnCheckProductionBatchNo').hide()
        //$('#ProductForm').hide();
        InvestDataTables.column(11).visible(false);
        ProductionTables.column(9).visible(false);
        CotangentDataTable.column(9).visible(false);

    }
    else if (Status == PendingBatch) {
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
        $('#BtnCheckBatchNo').show();
        $('#BtnEdit').hide();
        $('#BtnApprove').show();
        $('#BtnSave').hide();

    }
    else if (Status == Modified) {
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
        InvestDataTables.column(11).visible(false);
        ProductionTables.column(9).visible(false);
        CotangentDataTable.column(9).visible(false);


        $('#BtnSave').hide();
        $('#InputBathNoArea').hide();
        $('#OutputBathNoArea').hide();
    }
    else if (Status == CloseBatch) {
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
        InvestDataTables.column(11).visible(false);
        ProductionTables.column(9).visible(false);
        CotangentDataTable.column(9).visible(false);

        $('#BtnSave').hide();
        $('#InputBathNoArea').hide();
        $('#OutputBathNoArea').hide();
    }
    else {
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
        $('#BtnCheckBatchNo').show()
        $('#BtnEdit').hide()
        $('#BtnApprove').hide()
    }
 

    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#InvestDataTables').DataTable().table().node().style.width = '';
        $('#ProductionDataTables').DataTable().table().node().style.width = '';
        $('#CotangentDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });

    InvestDataTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        editorInvest.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
    });

    InvestDataTables.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        editorInvest.remove($(this).closest('tr'), {
            title: '刪除',
            message: '確定要刪除?',
            buttons: '確定'
        });
    });


    ProductionTables.on('click', '#btnDeleteProductionTable', function (e) {
        DeleteRateLoss();
        editorProduct.remove($(this).closest('tr'), {
            title: '刪除',
            message: '確定要刪除?',
            buttons: '確定'
        });
    });

    ProductionTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        editorProduct.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
        editorProduct.on('preSubmit', function (e, d) {
            var Roll_Ream_Wt = this.field('SecondaryQuantity');
            //var Weight = this.field('PrimaryQuantity');

            //if (Weight.val() === '') {
            //    this.field('PrimaryQuantity').error('請勿空白');
            //    return false;
            //}
            if (Roll_Ream_Wt.val() === '') {
                this.field('SecondaryQuantity').error('請勿空白');
                return false;
            }

            return true;
        });

    });

    CotangentDataTable.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        EditorCotangent.remove($(this).closest('tr'), {
            title: '刪除',
            message: '確定要刪除?',
            buttons: '確定'
        });
    });

    CotangentDataTable.on('click', '#btnInsert', function (e) {
        e.preventDefault();
        DeleteRateLoss();
        EditorCotangent.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
        EditorCotangent.on('preSubmit', function (e, d) {
            var SecondaryQuantity = this.field('SecondaryQuantity');
            if (SecondaryQuantity.val() === '') {
                this.field('SecondaryQuantity').error('請勿空白');
                return false;
            }
            return true;
        });
    });


    InvestDataTables.on('select', function (e, dt, items, indexes) {
        var rowData = InvestDataTables.rows(indexes).data();
        displaytext(rowData)
    });


    InvestDataTables.on('deselect', function (e, dt, items, indexes) {
        clear()
    });
})

function init() {
    //1 有殘捲 0 無殘捲
    $('#Invest_Remaining_Weight').attr("disabled", true)
    $('#Invest_Remnant').change(function (e) {
        var Remnant = $("#Invest_Remnant").val();
        if (Remnant == 1) {
            $('#Invest_Remaining_Weight').attr("disabled", false);
        } else {
            $('#Invest_Remaining_Weight').val("");
            $('#Invest_Remaining_Weight').attr("disabled", true);

        }

    });

    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#Production_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnProductionSave").click();
        }
    });


    $("#Cotangent_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnCotangentSave").click();
        }
    });



    ///輸入投入條碼
    $('#Invest_Barcode').change(function (e) {
        var Barcode = $('#Invest_Barcode').val();
        var OspDetailInId = $("#OspDetailInId").val();
        $.ajax({
            url: '/Process/CheckStockBarcode',
            type: 'post',
            datatype: 'json',
            data: { Barcode: Barcode, OspDetailInId: OspDetailInId },
            success: function (data) {
                if (data.resultDataModel.Success == false) {
                    swal.fire(data.resultDataModel.Msg);
                    ClearText();
                } else {
                    DispalyText(data);
                }
            },
            error: function () {
                swal.fire("失敗");
            }
        });

    });






}

function BtnClick() {

    //投入驗證單號
    $('#BtnCheckBatchNo').click(function (e) {
        var BatchNo = $('#InputBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        $.ajax({
            url: '/Process/CheckBatchNo',
            datatype: 'json',
            type: "POST",
            data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.resultModel.Success) {
                    DisplayInvestEnable(false);
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {

            }
        });
    });

    //列印入庫單
    $('#BtnPurchase').click(function () {
        if ($('#InvestDataTables').DataTable().data().length == 0) {
            swal.fire("投入無資料，請先輸入資料。");
            return;
        }


        var table = $('#ProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }
        var CotangentDataTable = $('#CotangentDataTables').DataTable();
        var Cotangent = CotangentDataTable.columns(4).data()[0];
        if (CotangentDataTable.data().length != 0) {
            if (Cotangent == 0) {
                swal.fire("請先輸入餘切重量。");
                return;
            }
        }
        var OspHeaderId = $('#OspHeaderId').val();
        window.open("/Home/OspStock/?OspHeaderId=" + OspHeaderId);
    });


    //投出驗證公單號
    $('#BtnCheckProductionBatchNo').click(function (e) {
        var BatchNo = $('#OutBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        $.ajax({
            url: '/Process/CheckBatchNo',
            datatype: 'json',
            type: "POST",
            data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.resultModel.Success) {
                    DisplayProductionEnable(false);
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
            error: function () {

            }
        });
    });

    //投入儲存條碼儲存按鈕
    $('#BtnProcessSave').click(function () {
        var Barcode = $('#Invest_Barcode').val().trim();
        var Remnant = $('#Invest_Remnant').val()
        var Remaining_Weight = $('#Invest_Remaining_Weight').val()
        var OspDetailInId = $("#OspDetailInId").val()
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

        InvestSaveBarcode(Barcode, Remnant, Remaining_Weight, OspDetailInId)
        $('#Invest_Barcode').select();
    });


    //產出產生明細
    $('#BtnProduct_detail').click(function () {
        var Production_Roll_Ream_Qty = $('#Production_Roll_Ream_Qty').val().trim();
        var Production_Roll_Ream_Wt = $('#Production_Roll_Ream_Wt').val().trim();
        var Cotangent = $('#Cotangent').val();
        var InvestDataTables = $('#InvestDataTables').DataTable().data();
        var OspDetailOutId = $("#OspDetailOutId").val();
        var OspDetailInId = $("#OspDetailInId").val();
        var OspHeaderId = $("#OspHeaderId").val()

        if (InvestDataTables.length == 0) {
            swal.fire("請先新增投入條碼。");
            return;
        }

        if (Production_Roll_Ream_Qty.length == 0) {
            swal.fire("棧板數不得空白");
            return;
        }
        if (Production_Roll_Ream_Wt.length == 0) {
            swal.fire("令數不得空白");
            return;
        }
        if (Cotangent == 1) {
            var CotangentDataTable = $('#CotangentDataTables').DataTable();
            if (CotangentDataTable.data().length > 0) {
                swal.fire("已有餘切資料，無法新增");
                return;
            }
        }
        CreateProduct(Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId, OspDetailInId, OspHeaderId);
    });
    //產出入庫
    $('#BtnProductionSave').click(function () {

        var Production_Barcode = $('#Production_Barcode').val();
        var table = $('#ProductionDataTables').DataTable();
        var OspDetailOutId = $("#OspDetailOutId").val()
        if (Production_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return;
        }
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }
        ChangeProductionStauts(Production_Barcode, OspDetailOutId);
        $('#Production_Barcode').select();
    });
    //餘切入庫
    $('#BtnCotangentSave').click(function () {
        var Cotangent_Barcode = $('#Cotangent_Barcode').val();
        var OspDetailOutId = $("#OspDetailOutId").val()
        if (Cotangent_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return
        }

        ChagneCotangent(Cotangent_Barcode, OspDetailOutId);
        $('#Cotangent_Barcode').select();
    });

    //計算損號
    $('#BtnCalculate').click(function () {
        var OspDetailInId = $("#OspDetailInId").val()
        var OspDetailOutId = $("#OspDetailOutId").val()
        var table = $('#ProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。")
            return
        }

        $.ajax({
            "url": "/Process/Loss",
            "type": "POST",
            "datatype": "json",
            "data": { OspDetailInId: OspDetailInId, OspDetailOutId: OspDetailOutId},
            success: function (data) {
                if (data.resultDataModel.Success) {
                    $('#InvestWeight').html(data.resultDataModel.Data.InvestWeight);
                    $('#ProductWeight').html(data.resultDataModel.Data.ProductWeight);
                    $('#Production_Loss').html(data.resultDataModel.Data.LossWeight);
                    $('#Rate').html(data.resultDataModel.Data.Rate);
                } else {
                    swal.fire(data.resultDataModel.Msg)
                }
            }
        });


    });


    //存檔入庫
    $('#BtnSave').click(function () {
        var loss = $('#Production_Loss').text();
        var Process_Batch_no = $('#Process_Batch_no').text()
        var OspDetailOutId = $("#OspDetailOutId").val();

        var table = $('#ProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }

        if ($('#InvestDataTables').DataTable().data().length == 0) {
            swal.fire("投入無資料，請先輸入資料。");
            return;
        }

        if (loss == 0) {
            swal.fire("損耗量未計算");
            return;
        }


        $.ajax({
            url: '/Process/_Locator/',
            type: 'POST',
            datatype:'json',
            data: { OspDetailOutId: OspDetailOutId },
            success: function (result) {
                $('body').append(result);
                Open($('#SubinventoryModal'));
            }
        });

    });



    //列印標籤紙捲
    $('#BtnRePrint').click(function () {
        var Status = "捲筒"
        PrintLableParameter(InvestDataTables, "/Process/RePrintLabel", "2",Status);
    })


    //列印標籤紙捲
    $('#BtnLabel').click(function () {
        var table = $('#ProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }
        var CotangentDataTable = $('#CotangentDataTables').DataTable();
        var Cotangent = CotangentDataTable.columns(4).data()[0];
        if (CotangentDataTable.data().length != 0) {
            if (Cotangent == 0) {
                swal.fire("請先輸入餘切重量。");
                return;
            }
        }
        PrintLable(ProductionTables, "/Process/GeProductLabels", "1", CotangentDataTable, "1");
    })



}


//初始化投入table
function LoadInvestDataTable() {

    editorInvest = new $.fn.dataTable.Editor({
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
                        'HasRemaint': d.data[key]['HasRemaint'],
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
            success: function (data) {
                if (data.resultModel.Success) {
                    InvestDataTables.ajax.reload(null, false);
                } else {
                    swal.fire(data.resultModel.Msg);
                }
               
            }
        },
        table: "#InvestDataTables",
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

    editorInvest.hide('OspPickedInId');

    editorInvest.on('preSubmit', function (e, d) {
        var Remnant
        var Remaining_Weight
        $.each(d.data, function (key, values) {
            Remnant = d.data[key]['HasRemaint']
            Remaining_Weight = d.data[key]['RemainingQuantity']
        })

        if (Remnant == "有") {
            if (Remaining_Weight == '') {
                this.field('RemainingQuantity').error('請勿空白')
                return false;
            }
        } else {

        }

        return true;
    });

    editorInvest.dependent('HasRemaint', function (val, data, callback) {
        if (val == '無') {
            editorInvest.field('RemainingQuantity').set('');
        }
        return val === '無' ?
            { hide: 'RemainingQuantity' } : { show: 'RemainingQuantity' };

    });

    var OspHeaderId = $("#OspHeaderId").val()
    InvestDataTables = $('#InvestDataTables').DataTable({
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
        columnDefs: [{
            orderable: false, targets: [0, 11], width: "60px",
        }],
        buttons: [
            'selectAll',
            'selectNone',
            {
                extend: 'excel',
                text: '匯出Excel'
            },
            {
                text: '刪除',
                className: "BtnAllDelete",
                enabled: false,
                name:'delete',
                action: function () {
                    var selectRowData = InvestDataTables.rows('.selected').data();
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
                            $.ajax({
                                url: '/Process/AllDelete',
                                datatype: 'json',
                                type: "POST",
                                data: { OspPickedInId: OspPickedInId },
                                success: function (data) {
                                    if (data.resultModel.Success) {
                                        InvestDataTables.ajax.reload(null, false);
                                    } else {
                                        swal.fire(data.resultModel.Msg);
                                    }
                                },
                                error: function () {

                                }
                            });
                        }
                    });
                }
            },
        ],
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
            { data: "HasRemaint", "name": "殘捲", "autoWidth": true, "className": "dt-body-center" },
            { data: "BasicWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "寬幅", "autoWidth": true, "className": "dt-body-center" },
            { data: "LotNumber", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "PrimaryQuantity", "name": "原重", "autoWidth": true, "className": "dt-body-right" },
            { data: "RemainingQuantity", "name": "餘重", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>' +
                        '&nbsp|&nbsp' + '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            }
        ],

    });
}
//儲存條碼
function InvestSaveBarcode(Barcode, Remnant, Remaining_Weight, OspDetailInId) {
    $.ajax({
        "url": "/Process/SaveInvestBarcode",
        "type": "POST",
        "datatype": "json",
        "data": { Barcode: Barcode, Remnant: Remnant, Remaining_Weight: Remaining_Weight, OspDetailInId: OspDetailInId },
        success: function (data) {
            if (data.resultModel.Success) {
                InvestDataTables.ajax.reload(null, false);
                ClearRateLoss();
            } else {
                swal.fire(data.resultModel.Msg);
            }
        }


    });

}

//初始化產出table
function LoadProductionDataTable() {

    editorProduct = new $.fn.dataTable.Editor({
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
                ProductionTables.ajax.reload(null, false);
                CotangentDataTable.ajax.reload(null, false);
            }
        },
        table: "#ProductionDataTables",
        idSrc: 'OspPickedOutId',
        fields: [
            //{
            //    label: "重量",
            //    name: "PrimaryQuantity"
            //},
            {
                name: "OspPickedOutId"
            },

            {
                label: "令數",
                name: "SecondaryQuantity"
            },
        ],
        i18n: {
            edit: {
                button: "新增",
                title: "更改原因",
                submit: "確定"
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

    editorProduct.field('OspPickedOutId').hide();


    var OspHeaderId = $("#OspHeaderId").val()
    ProductionTables = $('#ProductionDataTables').DataTable({
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
                text: '匯出Excel'
            },
            {
                text: '刪除',
                className: "BtnProductionAllDelete",
                enabled: false,
                name: 'delete',
                action: function () {
                    var selectRowData = ProductionTables.rows('.selected').data();
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
                            $.ajax({
                                url: '/Process/ProductionChooseDelete',
                                datatype: 'json',
                                type: "POST",
                                data: { OspPickedOutId: OspPickedOutId },
                                success: function (data) {
                                    if (data.resultModel.Success) {
                                        ProductionTables.ajax.reload(null, false);
                                    } else {
                                        swal.fire(data.resultModel.Msg);
                                    }
                                },
                                error: function () {

                                }
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
            { data: "OspPickedOutId", "name": "條碼號", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Product_Item", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PrimaryQuantity", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "PrimaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    return 'KG';
                }
            },
            { data: "SecondaryQuantity", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "SecondaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
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
                        '&nbsp|&nbsp' + '<button class = "btn btn-danger btn-sm" id = "btnDeleteProductionTable">刪除</button>';
                }
            }
        ],

    });

}

//產生明細
function CreateProduct(Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Cotangent, OspDetailOutId, OspDetailInId, OspHeaderId) {

    $.ajax({
        "url": "/Process/CreateProduction",
        "type": "POST",
        "datatype": "json",
        "data": {
            Production_Roll_Ream_Qty: Production_Roll_Ream_Qty,
            Production_Roll_Ream_Wt: Production_Roll_Ream_Wt,
            Cotangent: Cotangent,
            OspDetailOutId: OspDetailOutId,
            OspDetailInId: OspDetailInId,
            OspHeaderId: OspHeaderId
        },
        success: function (data) {
            if (data != null) {
                if (data.resultModel.Success) {
                    ProductionTables.ajax.reload(null, false);
                    CotangentDataTable.ajax.reload(null, false);
                    ClearRateLoss();
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            }
        }
    });

}

///產出條碼待入庫
function ChangeProductionStauts(Production_Barcode, OspDetailOutId) {
    $.ajax({
        "url": "/Process/ProductionChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { Production_Barcode: Production_Barcode, OspDetailOutId: OspDetailOutId },
        success: function (data) {
            if (data.resultModel.Success) {
                ProductionTables.ajax.reload(null, false);
            } else {
                swal.fire(data.resultModel.Msg);
            }
        }
    })
}

//餘切初始化Table
function CotangentDataTables() {


    EditorCotangent = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/CotangentEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var CotangentList = [];
                $.each(d.data, function (key, value) {

                    var Cotangent = {
                        'OspCotangentId': d.data[key]['OspCotangentId'],
                        'SecondaryQuantity': d.data[key]['SecondaryQuantity']

                    }
                    CotangentList.push(Cotangent);
                });

                var data = {
                    'Action': d.action,
                    'CotangentList': CotangentList
                }

                return JSON.stringify(data);
            },
            success: function () {
                ProductionTables.ajax.reload(null, false);
                CotangentDataTable.ajax.reload(null, false);
            }
        },
        table: "#CotangentDataTables",
        idSrc: 'OspCotangentId',
        fields: [
            //{
            //    label: "公斤",
            //    name: "Kg",
            //    attr: {
            //        type: "number",
            //        min: "0"
            //    }
            //},
            {
                label: "令數:",
                name: "SecondaryQuantity",
                attr: {
                    type: "number",
                    min: "0"
                }
            }
            ,
            {
                name:"OspCotangentId"
            }
        ],
        i18n: {
            edit: {
                button: "新增",
                title: "更改原因",
                submit: "確定"
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

    EditorCotangent.field('OspCotangentId').hide();

    var OspHeaderId = $("#OspHeaderId").val()
    CotangentDataTable = $('#CotangentDataTables').DataTable({
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
            "url": "/Process/CotangentDataTables",
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
                text: '匯出Excel'
            },
            {
                text: '刪除',
                className: "BtnCotangentDelete",
                enabled: false,
                name: 'delete',
                action: function () {
                    var selectRowData = CotangentDataTable.rows('.selected').data();
                    if (selectRowData.length == 0) {
                        swal.fire("請先選取刪除項目")
                        return;
                    }
                    var OspCotangentId = [];
                    var barcode = [];
                    for (i = 0; i < selectRowData.length; i++) {
                        OspCotangentId.push(selectRowData.pluck('OspCotangentId')[i]);
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
                            $.ajax({
                                url: '/Process/CotangentChooseDelete',
                                datatype: 'json',
                                type: "POST",
                                data: { OspCotangentId: OspCotangentId },
                                success: function (data) {
                                    if (data.resultModel.Success) {
                                        CotangentDataTable.ajax.reload(null, false);
                                    } else {
                                        swal.fire(data.resultModel.Msg);
                                    }
                                },
                                error: function () {

                                }
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
            { data: "OspCotangentId", "name": "條碼號", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Related_item", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PrimaryQuantity", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "PrimaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    return 'KG';
                }
            },
            { data: "SecondaryQuantity", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "SecondaryUom", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
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
                    return '<button class="btn btn-primary btn-sm" id = "btnInsert">編輯</button>' +
                        '&nbsp|&nbsp' + '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            },
        ],
    });

}

///餘切條碼入庫
function ChagneCotangent(CotangentBarcode, OspDetailOutId) {
    $.ajax({
        "url": "/Process/CotangentChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { CotangentBarcode: CotangentBarcode, OspDetailOutId: OspDetailOutId },
        success: function (data) {
            if (data.resultModel.Success) {
                CotangentDataTable.ajax.reload(null, false);
            } else {
                swal.fire(data.resultModel.Msg);
            }
        }
    });
}

function DispalyText(data) {
    $('#Invest_Original_Weight').html(data.resultDataModel.Data.PrimaryAvailableQty);
    $('#Invest_Basic_Weight').html(data.resultDataModel.Data.BasicWeight);
    $('#Invest_Specification').html(data.resultDataModel.Data.Specification);
    $('#Invest_Lot_Number').html(data.resultDataModel.Data.LotNumber);
}

function ClearText() {
    $('#Invest_Original_Weight').html("");
    $('#Invest_Basic_Weight').html("");
    $('#Invest_Specification').html("");
    $('#Invest_Lot_Number').html("");
    $('#Invest_Remaining_Weight').val("");
    $('#Invest_Barcode').val("");
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

    //確認按鍵
    modal_dialog.on('click', '#btnSaveSubinventory', function (e) {
        var Locator = $('#dialg_Locator').val();
        var OspHeaderId = $("#OspHeaderId").val();
        $.ajax({
            url: '/Process/ChangeHeaderStatus/',
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
        $.ajax({
            url: '/Process/ApproveHeaderStauts/',
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


function DsiplayHide() {
    $('#BtnCheckBatchNo').hide()
    $('#BtnSave').hide();
    $('#BtnEdit').show()
    $('#BtnApprove').show()
    $('#BtnCheckProductionBatchNo').hide()
    $('#ProductForm').hide();
}

function DsiplayShow(Status) {
    $('#BtnCheckBatchNo').show()
    $('#BtnEdit').hide()
    $('#BtnApprove').show()
    $('#BtnSave').hide();
}

function ClearRateLoss() {
    $('#Production_Loss').html("");
    $('#Rate').html("");
    $('#InvestWeight').html("");
    $('#ProductWeight').html("");
}

//驗證disable投入
function DisplayInvestEnable(boolean) {
    $('#Invest_Barcode').attr('disabled', boolean);
    $('#Invest_Remnant').attr('disabled', boolean);
    $('#BtnProcessSave').attr('disabled', boolean);
    $('#BtnRePrint').attr('disabled', boolean);
    InvestDataTables.button(3).enable(!boolean);
}

//驗證disable產出
function DisplayProductionEnable(boolean) {

    $('#Production_Roll_Ream_Qty').attr('disabled', boolean);
    $('#Production_Roll_Ream_Wt').attr('disabled', boolean);
    $('#Cotangent').attr('disabled', boolean);
    $('#BtnProduct_detail').attr('disabled', boolean);
    $('#Production_Barcode').attr('disabled', boolean);
    $('#BtnProductionSave').attr('disabled', boolean);
    $('#BtnCotangentSave').attr('disabled', boolean);
    $('#Cotangent_Barcode').attr('disabled', boolean);
    $('#BtnCalculate').attr('disabled', boolean);
    $('#BtnLabel').attr('disabled', boolean);
    $('#BtnPurchase').attr('disabled', boolean);
    ProductionTables.button(3).enable(!boolean);
    CotangentDataTable.button(3).enable(!boolean);
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

        }
    });
}

function displaytext(rowData) {
    var Barcode = rowData.pluck('Barcode')[0]
    var Invest_Remnant = rowData.pluck('HasRemaint')[0]
    var Invest_Original_Weight = rowData.pluck('PrimaryQuantity')[0]
    var Invest_Remaining_Weight = rowData.pluck('RemainingQuantity')[0]
    var Invest_Basic_Weight = rowData.pluck('BasicWeight')[0]
    var Invest_Specification = rowData.pluck('Specification')[0]
    var Invest_Lot_Number = rowData.pluck('LotNumber')[0]



    $('#Invest_Barcode').text(Barcode);
    $('#Invest_Remnant').val(Invest_Remnant == "有" ? 1 : 0);
    $('#Invest_Original_Weight').text(Invest_Original_Weight);
    $('#Invest_Remaining_Weight').val(Invest_Remaining_Weight);
    $('#Invest_Basic_Weight').text(Invest_Basic_Weight);
    $('#Invest_Specification').text(Invest_Specification);
    $('#Invest_Lot_Number').text(Invest_Lot_Number);

}


function clear() {
    $('#Invest_Barcode').text("");
    $('#Invest_Remnant').val(0);
    $('#Invest_Original_Weight').text("");
    $('#Invest_Remaining_Weight').val("");
    $('#Invest_Basic_Weight').text("");
    $('#Invest_Specification').text("");
    $('#Invest_Lot_Number').text("");
}

//完工紀錄使用
function BtnRecordEdit() {
    $('#BtnEdit').click(function () {

        var BatchNo = $('#InputBatchNo').val();
        var OspHeaderId = $('#OspHeaderId').val();
        $.ajax({
            url: '/Process/FinishedEdit',
            datatype: 'json',
            type: "POST",
            data: { BatchNo: BatchNo, OspHeaderId: OspHeaderId },
            success: function (data) {
                if (data.Success) {
                    location.href = "/Process/Schedule/" + data.Data;
                } else {
                    swal.fire(data.Msg);
                }
            },
            error: function () {

            }
        });

    });

    $('#BtnApprove').click(function () {
        //var OspHeaderId = $("#OspHeaderId").val();
        //$.ajax({
        //    url: '/Process/ApproveHeaderStauts/',
        //    type: 'POST',
        //    datatype: 'json',
        //    data: { OspHeaderId: OspHeaderId },
        //    success: function (data) {
        //        if (data.resultModel.Success) {
        //            window.location.href = '/Process/Index';
        //        } else {
        //            swal.fire(data.resultModel.Msg)
        //        }
        //    }
        //});


        var loss = $('#Production_Loss').text();
        var OspDetailOutId = $("#OspDetailOutId").val();

        var table = $('#ProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。");
            return;
        }

        if ($('#InvestDataTables').DataTable().data().length == 0) {
            swal.fire("投入無資料，請先輸入資料。");
            return;
        }

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
                OpenApproveDialog($('#SubinventoryModal'));
            }
        });

    });



}
