var rowData
var ProcessDataTables
$(document).ready(function () {
    BtnEvent();
    search();
    //初始化日期
    $('#DueDate').datepicker({
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


    firstLoad();


    ProcessDataTables.on('click', '#btnRecord', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var Process_Detail_Id = data.Process_Detail_Id;
        if (data == null) {
            return false;
        }
        if (Process_Detail_Id == 1) {
            window.location.href = '/Process/Schedule/' + Process_Detail_Id;
        }
        if (Process_Detail_Id == 2) {
            window.location.href = '/Process/Flat/' + Process_Detail_Id;
        }

        if (Process_Detail_Id == 3) {
            window.location.href = '/Process/PaperRoll/' + Process_Detail_Id;
        }

       
    })

    ProcessDataTables.on('click', '#btnEdit', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var Process_Detail_Id = data.Process_Detail_Id;
        if (data == null) {
            return false;
        }
        window.location.href = '/Process/Edit/' + Process_Detail_Id;
    })


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#ProcessDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });



})

function ProcessLoadTable(Status, OspBatchNo, MachineNum, DueDate, CuttingDateFrom, CuttingDateTo, Subinventory) {

    ProcessDataTables = $('#ProcessDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        scrollX : true,
        destroy: true,
        processing: true,
        serverSide: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/TableResult",
            "type": "POST",
            "datatype": "json",
            "data": {
                Status: Status, OspBatchNo: OspBatchNo,
                MachineNum: MachineNum, DueDate: DueDate, CuttingDateFrom: CuttingDateFrom,
                CuttingDateTo: CuttingDateTo, Subinventory: Subinventory
            }
        },
        select: {
            style: 'single',
            selector: 'td:first-child'
        },
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
            { data: "BatchType", "name": "工單號", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "MachineNum", "name": "機台", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "Status", "name": "狀態", "autoWidth": true, "render": function (data, type, row) {
                    if (data == "已排單") {
                        switch (row.Status.substr(0, 1)) {
                            case "OSP":
                                return '<a href=/Process/Schedule/' + row.OspDetailInId + '> ' + data + '</a>';
                                break;
                            case "TMP":
                                return '<a href=/Process/Flat/' + row.OspDetailInId + '> ' + data + '</a>';
                                break;
                            case "R":
                                return '<a href=/Process/PaperRoll/' + row.OspDetailInId + '> ' + data + '</a>';
                                break;
                        }
                    } else {
                        return data;
                    }

                    if (data == "已排單") {
                        switch (row.Status.substr(0, 1)) {
                            case "OSP":
                                return '<a href=/Process/Schedule/' + row.OspDetailInId + '> ' + data + '</a>';
                                break;
                            case "TMP":
                                return '<a href=/Process/Flat/' + row.OspDetailInId + '> ' + data + '</a>';
                                break;
                            case "R":
                                return '<a href=/Process/PaperRoll/' + row.OspDetailInId + '> ' + data + '</a>';
                                break;
                        }
                    }


                }, "className": "dt-body-center"
            },
            { data: "CustomerName", "name": "客戶名稱", "autoWidth": true, "className": "dt-body-center"},
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center"},
            { data: "BasicWeight", "name": "基重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center"},
            { data: "OrderWeight", "name": "令重", "autoWidth": true, "className": "dt-body-right"},
            { data: "GrainDirection", "name": "絲向", "autoWidth": true, "className": "dt-body-center"},
            { data: "ReamWt", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionUom", "name": "交易單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "PrimaryQuantity", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUom", "name": "主要單位", "autoWidth": true, "className": "dt-body-center"},
            { data: "PackingType", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center"},
            { data: "OspRemark", "name": "委外工單備註", "autoWidth": true, "className": "dt-body-center"},
            { data: "Note", "name": "生產備註", "autoWidth": true, "className": "dt-body-center"},
            { data: "SelectedInventoryItemNumber", "name": "組成成份料號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Product_Item", "name": "產品料號", "autoWidth": true, "className": "dt-body-center"},
            { data: "OrderNumber", "name": "訂單編號", "autoWidth": true, "className": "dt-body-center"},
            { data: "OrderLineNumber", "name": "明細行", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "", "autoWidth": true, "render": function (data, type, row) {
                    if (row.Status == "已完工") {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>' + '<button class="btn btn-primary btn-sm" id = "btnRecord">完工紀錄</button>';
                    }
                    if (row.Status == "待核准") {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>' + '<button class="btn btn-primary btn-sm" id = "btnRecord">完工紀錄</button>';
                    }
                    if (row.Status == "已排單") {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>';
                    } else {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>';
                    }
       
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
    $('#BtnStatus').click(function () {
        if (rowData == null) {
            swal.fire("請先選擇一項");
            return;
        }
        var Status = rowData.pluck('Status')[0]
        var OspDetailInId = rowData.pluck('OspDetailInId')[0]
        if (Status == "待排單") {
            $.ajax({
                url: '/Process/_ProcessIndex',
                type: "POST",
                success: function (result) {
                    $('body').append(result);
                    Open($('#ProcessModal'), OspDetailInId);
                },
                error: function () {
                    swal.fire("失敗");
                }
            });

        } else {
            swal.fire("加工狀態不正確，重新選擇");
            return;
        }


    });

    $('#BtnCloss').click(function () {
        if (rowData == null) {
            swal.fire("請先選擇一項")
            return;
        }
        var BtnCloss = $('#BtnCloss').text();
        var Status = rowData.pluck('Status')[0]
        var OspDetailInId = rowData.pluck('OspDetailInId')[0]
        if (Status == "已完工") {
            changeStatus(OspDetailInId, BtnCloss);
        } else {
            swal.fire("加工狀態不正確，重新選擇");
            return;
        }


    });

    $('#BtnEdit').click(function () {
        if (rowData == null) {
            swal.fire("請先選擇一項")
            return;
        }
        var Status = rowData.pluck('Status')[0]
        var OspDetailInId = rowData.pluck('OspDetailInId')[0]
        if (Status == "已排單") {
            $.ajax({
                url: '/Process/_ProcessIndex',
                type: "POST",
                success: function (result) {
                    $('body').append(result);
                    Open($('#ProcessModal'), OspDetailInId);
                },
                error: function () {
                    swal.fire("失敗");
                }
            });
        } else {
            swal.fire("加工狀態不正確，重新選擇");
            return;
        }

    });
}

function search() {
    $('#btnSearch').click(function () {
        rowData = null;
        var Status = $("#Status").val();
        var OspBatchNo = $("#OspBatchNo").val();
        var MachineNum = $("#MachineNum").val();
        var DueDate = $("#DueDate").val();
        var CuttingDateFrom = $("#CuttingDateFrom").val();
        var CuttingDateTo = $("#CuttingDateTo").val();
        var Subinventory = $("#Subinventory").val();

        ProcessLoadTable(Status, OspBatchNo, MachineNum, DueDate, CuttingDateFrom, CuttingDateTo, Subinventory);
        
        
    });

}

//彈出dialog
function Open(modal_dialog, OspDetailInId) {
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
        var BtnStatus = "已排單";

        if (Dialog_CuttingDateFrom.length == 0) {
            swal.fire("裁切日期(起)不得空白");
            return
        }

        if (Dialog_CuttingDateTo.length == 0) {
            swal.fire("裁切日期(迄)不得空白");
            return
        }

        if (Dialog_MachineNum.length == 0) {
            swal.fire("機台不得空白");
            return
        }

        if (new Date(Dialog_CuttingDateFrom) > new Date(Dialog_CuttingDateTo)) {
            swal.fire("時間起不得超過迄");
            return
        }

        $.ajax({
            url: '/Process/_BtnDailog',
            type: "POST",
            dataType: 'json',
            data: {
                OspDetailInId: OspDetailInId, Dialog_CuttingDateFrom: Dialog_CuttingDateFrom
                , Dialog_CuttingDateTo: Dialog_CuttingDateTo, Dialog_MachineNum: Dialog_MachineNum, BtnStatus: BtnStatus
            },
            success: function (result) {
                $(modal_dialog.selector).modal('hide');
                firstLoad();
                rowData = null;
            },
            error: function () {
                swal.fire("時間格式錯誤");
            }
        });



    });



    modal_dialog.modal('show');

}

function changeStatus(OspDetailInId, BtnStatus) {
    $.ajax({
        url: '/Process/_BtnDailog',
        type: "POST",
        dataType: 'json',
        data: { OspDetailInId: OspDetailInId, BtnStatus: BtnStatus },
        success: function (result) {
            firstLoad();
        },
        error: function () {
            swal.fire("失敗")
        }
    });


}

function firstLoad() {
    var OspBatchStatusDesc = $("#OspBatchStatusDesc").val();
    var OspBatchNo = $("#OspBatchNo").val();
    var MachineNum = $("#MachineNum").val();
    var DueDate = $("#DueDate").val();
    var CuttingDateFrom = $("#CuttingDateFrom").val();
    var CuttingDateTo = $("#CuttingDateTo").val();
    var Subinventory = $("#Subinventory").val();

    ProcessLoadTable(OspBatchStatusDesc, OspBatchNo, MachineNum, DueDate, CuttingDateFrom, CuttingDateTo, Subinventory);
}