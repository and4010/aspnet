var rowData;
var ProcessDataTables
/// <summary>
/// 待排單
/// </summary>
const WaitBatch = "0";
/// <summary>
/// 已排單
/// </summary>
const DwellBatch = "1";
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
/// <summary>
/// 已取消
/// </summary>
const Canceled ="6";

$(document).ready(function () {
    BtnEvent();
    search();
    
    //初始化日期
    $('#DueDateFrom').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#DueDateTo').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#CuttingDateFrom').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#CuttingDateTo').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    $("#PlanStartDateFrom").datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $("#PlanStartDateTo").datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    firstLoad();


    ProcessDataTables.on('click', '#btnRecord', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var BatchType = data.BatchType;
        if (data == null) {
            return false;
        }
        if (BatchType == "OSP") {
            window.location.href = '/Process/Schedule/' + data.OspHeaderId;
        }
        if (BatchType == "REP") {
            if (data.PackingType == "" || data.PackingType == null) {
                window.location.href = '/Process/PaperRoll/' + data.OspHeaderId;
            } else {
                window.location.href = '/Process/Flat/' + data.OspHeaderId;
            }
        }

    });

    ProcessDataTables.on('click', '#btnBefRecord', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var BatchType = data.BatchType;
        if (data == null) {
            return false;
        }
        if (BatchType == "OSP") {
            window.location.href = '/Process/Schedule/' + data.OrgOspHeaderId;
        }
        if (BatchType == "REP") {
            if (data.PackingType == "" || data.PackingType == null) {
                window.location.href = '/Process/PaperRoll/' + data.OrgOspHeaderId;
            } else {
                window.location.href = '/Process/Flat/' + data.OrgOspHeaderId;
            }
        }

    });

    ProcessDataTables.on('click', '#btnReEdit', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var BatchType = data.BatchType;
        if (data == null) {
            return false;
        }
        if (BatchType == "OSP") {
            window.location.href = '/Process/Schedule/' + data.OspHeaderId;
        }
        if (BatchType == "REP") {
            if (data.PackingType == "" || data.PackingType == null) {
                window.location.href = '/Process/PaperRoll/' + data.OspHeaderId;
            } else {
                window.location.href = '/Process/Flat/' + data.OspHeaderId;
            }

        }

    });

    ProcessDataTables.on('click', '#btnEdit', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        if (data == null) {
            return false;
        }
        window.location.href = '/Process/Edit/' + data.OspHeaderId;
    });


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#ProcessDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });



})

function ProcessLoadTable(Status, BatchNo, MachineNum, DueDateFrom, DueDateTo, CuttingDateFrom, CuttingDateTo, PlanStartDateFrom, PlanStartDateTo, Subinventory) {

    ProcessDataTables = $('#ProcessDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        destroy: true,
        processing: true,
        serverSide: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-3 width-s'l><'col-sm-6'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/TableResult",
            "type": "POST",
            "datatype": "json",
            "data": {
                Status: Status,
                BatchNo: BatchNo,
                MachineNum: MachineNum,
                DueDateFrom: DueDateFrom,
                DueDateTo: DueDateTo,
                CuttingDateFrom: CuttingDateFrom,
                CuttingDateTo: CuttingDateTo,
                Subinventory: Subinventory,
                PlanStartDateFrom: PlanStartDateFrom,
                PlanStartDateTo: PlanStartDateTo
            }
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons '
                },
            },
            buttons: [
                {
                    extend: 'excel',
                    text: '匯出Excel',
                    filename: function () {
                        return moment().format("YYYYMMDDHHmmss");
                    }
                },
                {
                    text: '排單',
                    className: 'margin-left-s btn-danger',
                    action: function () {
                        status_onclick();
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '更改排單日',
                    className: 'btn-danger',
                    action: function () {
                        edit_onclick();
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '關帳',
                    className: 'btn-danger',
                    action: function () {
                        closs_onclick();
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '裁切單',
                    className: 'margin-left-s btn-primary',
                    action: function () {
                        cutReceipt_onclick();
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '領料單',
                    className: 'btn-primary',
                    action: function () {
                        cutMaterial_onclick();
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
            ]
        },
      
        "order": [[23, "desc"]], //計畫開始時間排序
        columnDefs: [{
            orderable: false, targets: [0, 24], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            {
                data: "", "autoWidth": true, "render": function (data, type, row) {
                    var content = '';
                    var temp = [];

                    if (row.Status == CompletedBatch || row.Status == PendingBatch || row.Status == CloseBatch || row.Status == Modified) {
                        if (row.OrgOspHeaderId > 0) {
                            temp.push('<button class="btn btn-primary btn-sm" id = "btnBefRecord">前完工紀錄</button>');
                        }

                        if (row.Status == PendingBatch) {
                            temp.push('<button class="btn btn-danger btn-sm" id = "btnReEdit">修改</button>');
                        }
                        else {
                            temp.push('<button class="btn btn-primary btn-sm" id = "btnRecord">完工紀錄</button>');
                        }
                    }

                    if (row.Status == DwellBatch || row.Status == PendingBatch) {
                        temp.push('<button class="btn btn-primary btn-sm" id = "btnEdit">編輯備註</button>');
                    }

                    for (i = 0; i < temp.length; i++) {
                        content = content + temp[i];
                        if (i + 1 < temp.length) {
                            content = content + '&nbsp|&nbsp';
                        }
                    }

                    return content;

                }, orderable: false,
            },
            {
                data: "DueDate", "name": "需求日", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }

                }, "className": "dt-body-center"
            },
            {
                data: "CuttingDateFrom", "name": "裁切日(起)", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }, "className": "dt-body-center"
            },
            {
                data: "CuttingDateTo", "name": "裁切日(迄)", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }, "className": "dt-body-center"
            },
            { data: "BatchNo", "name": "工單號", "autoWidth": true, "className": "dt-body-center" },
            { data: "MachineNum", "name": "機台", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "Status", "name": "狀態", "autoWidth": true, "render": function (data, type, row) {
                    if (data == DwellBatch || data == PendingBatch) {
                        switch (row.BatchType) {
                            case "OSP":
                                return '<a href=/Process/Schedule/' + row.OspHeaderId + '> ' + GetStatusCode(data) + '</a>';
                            case "REP":
                                if (row.PackingType == "" || row.PackingType == null) {
                                    return '<a href=/Process/PaperRoll/' + row.OspHeaderId + '> ' + GetStatusCode(data) + '</a>';
                                } else {
                                    return '<a href=/Process/Flat/' + row.OspHeaderId + '> ' + GetStatusCode(data) + '</a>';
                                }
                        }
                    } else {
                        var status = GetStatusCode(data);
           
                        return status
                    }

                }, "className": "dt-body-center"
            },
            {
                data: "CustomerName", "name": "客戶名稱", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data) {
                        return data;
                    } else {
                        return '中華紙漿';
                    }
                }, "className": "dt-body-center"
            },
            { data: "DoPaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "DoBasicWeight", "name": "基重", "autoWidth": true, "className": "dt-body-right" },
            { data: "DoSpecification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
            { data: "OrderWeight", "name": "令重", "autoWidth": true, "className": "dt-body-right" },
            { data: "DoGrainDirection", "name": "絲向", "autoWidth": true, "className": "dt-body-center" },
            { data: "DoReamWt", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionUom", "name": "交易單位", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "PrimaryQuantity", "name": "重量", "autoWidth": true, "className": "dt-body-right", "mRender": function(data, type, row) {
                    if (data == null || data == 0) {
                        return "";
                    } else {
                        return Math.round(data * 1000) / 1000;
                    } 
                }
            },
            { data: "PrimaryUom", "name": "主要單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "DoPackingType", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center" },
            { data: "OspRemark", "name": "委外工單備註", "autoWidth": true, "className": "dt-body-center" },
            { data: "Note", "name": "生產備註", "autoWidth": true, "className": "dt-body-center" },
            { data: "SelectedInventoryItemNumber", "name": "組成成份料號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Product_Item", "name": "產品料號", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "PlanStartDate", "name": "計畫開始日期", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD HH:mm:ss');
                    } else {
                        return '';
                    }
                }, "className": "dt-body-center"
            },
            {
                data: "OrderNumber", "name": "訂單編號", "autoWidth": true, "className": "dt-body-center", "render": function (data, type, row) {
                    if (data != "0") {
                        return data;
                    }
                    return "";
                }
            },
            {
                data: "OrderLineNumber", "name": "明細行", "autoWidth": true, "className": "dt-body-center", "render": function (data, type, row) {
                    if (data != "0") {
                        return data;
                    }
                    return "";
                }
            }
            
        ],

    });

    ProcessDataTables.on('select', function (e, dt, type, indexes) {
        rowData = ProcessDataTables.rows(indexes).data();
    });

    ProcessDataTables.on('deselect', function (e, dt, type, indexes) {
        rowData = null;
    });

}


function BtnEvent() {
    $('#BtnStatus').click(status_onclick);

    $('#BtnCloss').click(closs_onclick);

    $('#BtnEdit').click(edit_onclick);

    $('#BtnCutReceipt').click(cutReceipt_onclick);

    $('#BtnCutMaterial').click(cutMaterial_onclick);
}


function windowOpen(url) {
    var wi = window.open('about:blank', '_blank');
    wi.location.href = url;
}

function sleep(time) {
    return new Promise( function (resolve) {
        setTimeout(resolve, time || 1000);
    } );
}

function status_onclick() {
    var selectRowData = ProcessDataTables.rows('.selected').data();
    if (selectRowData.length > 1) {
        swal.fire("排單只能選擇一項");
        return;
    }
    if (selectRowData.length > 1) {
        swal.fire("請先選擇一項");
        return;
    }
    var Status = selectRowData.pluck('Status')[0]
    if (Status != WaitBatch) {
        swal.fire("加工狀態不正確，重新選擇");
        return;
    }
    var OspHeaderId = selectRowData.pluck('OspHeaderId')[0]
    var PaperType = selectRowData.pluck('PaperType')[0]
    var SrcOspHeaderId = selectRowData.pluck('SrcOspHeaderId')[0]
    var BatchType = selectRowData.pluck('BatchType')[0]


    if (BatchType == "REP" && SrcOspHeaderId) {
        //又裁又代 須檢查來源裁切工單是否完工
        ShowWait(function () {
            $.ajax({
                "url": "/Process/CheckInsteadPaperOrderProcess",
                "type": "POST",
                "datatype": "json",
                "data": {
                    SrcOspHeaderId: SrcOspHeaderId
                },
                success: function (data) {
                    if (data != null) {
                        if (data.resultModel.Success) {
                            CloseWait();
                            orderProcess(PaperType, OspHeaderId, BatchType);
                        } else {
                            swal.fire(data.resultModel.Msg);
                        }
                    }
                },
                error: function () {
                    swal.fire("檢查來源柴切工單是否完工失敗");
                }
            })
        });
    } else {
        orderProcess(PaperType, OspHeaderId, BatchType);
    }
}

function closs_onclick() {
    var selectRowData = ProcessDataTables.rows('.selected').data();
    if (selectRowData.length > 1) {
        swal.fire("關帳只能選擇一項");
        return;
    }
    if (selectRowData == null) {
        swal.fire("請先選擇一項")
        return;
    }
    var BtnCloss = CloseBatch;
    var Status = selectRowData.pluck('Status')[0]
    var OspHeaderId = selectRowData.pluck('OspHeaderId')[0]
    if (Status == CompletedBatch) {
        changeStatus(OspHeaderId, BtnCloss);
    } else {
        swal.fire("加工狀態不正確，重新選擇");
        return;
    }
}

function edit_onclick() {
    var selectRowData = ProcessDataTables.rows('.selected').data();
    if (selectRowData.length > 1) {
        swal.fire("排單只能選擇一項");
        return;
    }
    if (selectRowData == null) {
        swal.fire("請先選擇一項");
        return;
    }
    var Status = selectRowData.pluck('Status')[0]
    var OspHeaderId = selectRowData.pluck('OspHeaderId')[0]
    var PaperType = selectRowData.pluck('PaperType')[0]
    var BatchType = selectRowData.pluck('BatchType')[0]
    if (Status == DwellBatch) {
        orderProcess(PaperType, OspHeaderId, BatchType);

    } else {
        swal.fire("加工狀態不正確，重新選擇");
        return;
    }
}

function cutReceipt_onclick() {
    var headerList = []

    var selectRowData = ProcessDataTables.rows('.selected').data();

    for (i = 0; i < selectRowData.length; i++) {
        switch (selectRowData.pluck('Status')[i]) {
            case WaitBatch:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "請先排單，再列印裁切單。");
                return;
            case CompletedBatch:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "已完工，無法列印");
                return;
            case CloseBatch:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "已關帳，無法列印。");
                return;
            case Canceled:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "已取消，無法列印。");
                return;
        }

        if (selectRowData.pluck('BatchType')[i] == "OSP") {
            headerList.push(selectRowData.pluck('OspHeaderId')[i]);
        }
    }

    var millisecondsToWait = 0;
    for (i = 0; i < headerList.length; i++) {
        var url = "/Home/OspCutReceiptReport/?OspHeaderId=" + headerList[i];
        setTimeout(windowOpen, millisecondsToWait, url);
        millisecondsToWait += 5000;
    }
}

function cutMaterial_onclick() {
    var headerList = []

    var selectRowData = ProcessDataTables.rows('.selected').data();

    for (i = 0; i < selectRowData.length; i++) {
        switch (selectRowData.pluck('Status')[i]) {
            case WaitBatch:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "請先排單，再列印領料單。");
                return;
            case CompletedBatch:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "已完工，無法列印");
                return;
            case CloseBatch:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "已關帳，無法列印。");
                return;
            case Canceled:
                swal.fire(selectRowData.pluck('BatchNo')[i] + "已取消，無法列印。");
                return;
        }
        headerList.push(selectRowData.pluck('OspHeaderId')[i]);
        
    }

    var millisecondsToWait = 0;
    for (i = 0; i < headerList.length; i++) {
        var url = "/Home/OspReport/?OspHeaderId=" + headerList[i];
        setTimeout(windowOpen, millisecondsToWait, url);
        millisecondsToWait += 5000;
    }

}

//排單
function orderProcess(PaperType, OspHeaderId, BatchType) {
    $.ajax({
        url: '/Process/_ProcessIndex',
        type: "POST",
        data: { PaperType: PaperType },
        success: function (result) {
            $('body').append(result);
            Open($('#ProcessModal'), OspHeaderId, BatchType);
        },
        error: function () {
            swal.fire("排單失敗");
        }

    });
}

function search() {
    $('#btnSearch').click(function () {
        rowData = null;
        var Status = $("#Status").val();
        var BatchNo = $("#BatchNo").val();
        var MachineNum = $("#MachineNum").val();
        var DueDateTo = $("#DueDateTo").val();
        var DueDateFrom = $("#DueDateFrom").val();
        var CuttingDateFrom = $("#CuttingDateFrom").val();
        var CuttingDateTo = $("#CuttingDateTo").val();
        var Subinventory = $("#Subinventory").val();
        var PlanStartDateFrom = $("#PlanStartDateFrom").val();
        var PlanStartDateTo = $("#PlanStartDateTo").val();
        
        if (BatchNo &&  BatchNo.length < 4) {
            swal.fire("工單號須輸入4碼以上");
            return;
        }

        ProcessLoadTable(Status, BatchNo, MachineNum, DueDateFrom, DueDateTo, CuttingDateFrom, CuttingDateTo, PlanStartDateFrom, PlanStartDateTo, Subinventory);


    });

}

//彈出dialog
function Open(modal_dialog, OspHeaderId, BatchType) {
    modal_dialog.modal({
        backdrop: "static",
        keyboard: true,
        show: true
    });

    //初始化
    $('#Dialog_CuttingDateFrom').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#Dialog_CuttingDateTo').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    modal_dialog.on('hidden.bs.modal', function (e) {
        $("div").remove(modal_dialog.selector);
    });

    modal_dialog.on('show.bs.modal', function (e) {
        $.validator.unobtrusive.parse('form');
    });

    //確認按鍵
    modal_dialog.on('click', '#btnDailogProcess', function (e) {
        var Dialog_CuttingDateFrom = $('#Dialog_CuttingDateFrom').val();
        var Dialog_CuttingDateTo = $('#Dialog_CuttingDateTo').val();
        var Dialog_MachineNum = $('#Dialog_MachineNum').val();
        var BtnStatus = DwellBatch;

        if (Dialog_CuttingDateFrom.length == 0) {
            swal.fire("裁切日期(起)不得空白");
            return
        }

        if (Dialog_CuttingDateTo.length == 0) {
            swal.fire("裁切日期(迄)不得空白");
            return
        }

        if (new Date(Dialog_CuttingDateFrom) > new Date(Dialog_CuttingDateTo)) {
            swal.fire("時間起不得超過迄");
            return
        }

        if (BatchType != 'REP' && Dialog_MachineNum == null) {
            swal.fire("機台不能空白");
            return
        }

        ShowWait(function () {
            $.ajax({
                url: '/Process/_BtnDailogChangStatusCutDate',
                type: "POST",
                dataType: 'json',
                data: {
                    OspHeaderId: OspHeaderId, Dialog_CuttingDateFrom: Dialog_CuttingDateFrom
                    , Dialog_CuttingDateTo: Dialog_CuttingDateTo, Dialog_MachineNum: Dialog_MachineNum, BtnStatus: BtnStatus
                },
                success: function (result) {
                    CloseWait();
                    $(modal_dialog.selector).modal('hide');
                    firstLoad();
                    rowData = null;
                },
                error: function () {
                    swal.fire("時間格式錯誤");
                }
            });
        });

    });



    modal_dialog.modal('show');

}

function changeStatus(OspHeaderId, BtnStatus) {
    ShowWait(function () {
        $.ajax({
            url: '/Process/SetClose',
            type: "POST",
            dataType: 'json',
            data: { OspHeaderId: OspHeaderId, BtnStatus: BtnStatus },
            success: function (result) {
                CloseWait();
                firstLoad();
            },
            error: function () {
                swal.fire("關帳失敗")
            }
        });
    });
   
}

function firstLoad() {
    var Status = $("#Status").val();
    var BatchNo = $("#BatchNo").val();
    var MachineNum = $("#MachineNum").val();
    var DueDateFrom = $("#DueDateFrom").val();
    var DueDateTo = $("#DueDateTo").val();
    var CuttingDateFrom = $("#CuttingDateFrom").val();
    var CuttingDateTo = $("#CuttingDateTo").val();
    var Subinventory = $("#Subinventory").val();
    var PlanStartDateFrom = $("#PlanStartDateFrom").val();
    var PlanStartDateTo = $("#PlanStartDateTo").val();

    if (BatchNo && BatchNo.length < 4) {
        swal.fire("工單號須輸入4碼以上");
        return;
    }

    ProcessLoadTable(Status, BatchNo, MachineNum, DueDateFrom, DueDateTo, CuttingDateFrom, CuttingDateTo, PlanStartDateFrom, PlanStartDateTo, Subinventory);
}


function GetStatusCode(StatusCode) {
    switch (StatusCode) {
        case WaitBatch:
            return '待排單';
        case DwellBatch:
            return '已排單';
        case PendingBatch:
            return '待核准';
        case CompletedBatch:
            return '已完工';
        case CloseBatch:
            return '關帳';
        case Modified:
            return '已修改';
        case '6':
            return '已取消';
        default:
            return '';
    }
}
