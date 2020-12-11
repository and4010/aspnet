
var QueryTable;


$(document).ready(function () {

    //$('#BatchNo').combobox();
    //$('#MachineNum').combobox();

    $('#btnSearch').click(function () {
        QueryTable.ajax.reload();
    });

    $('#btnPrint').click(function () {

        ShowWait(ospYieldReport);
    });

    $('#dateFrom').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    $('#dateTo').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    //initQueryTable();
});

function ospYieldReport() {
    var batchNo = getBatchNo();
    if (batchNo && batchNo.length < 4) {
        swal.fire("工單號須輸入4碼以上");
        CloseWait();
        return;
    }

    $.ajax({
        async: false,
        url: "/Report/OspYieldReport",
        type: "post",
        data: {
            cuttingDateFrom: getCuttingDateFrom(),
            cuttingDateTo: getCuttingDateTo(),
            batchNo: batchNo,
            machineNum: getMachineNum(),
            itemNumber: getItemNumber(),
            barcode: getBarcode(),
            subinventory: getSubinventory()
        },
        success: function (model) {
            $("#ReportPartial").html(model).promise().done(function () {
                CloseWait();
            });
        },
        error: function () {
            CloseWait();
            swal.fire('更新報表失敗');
        }
    });
}

function getCuttingDateFrom() {
    return $("#dateFrom").val();
}

function getCuttingDateTo() {
    return $("#dateTo").val();
}

function getBatchNo() {
    return $("#BatchNo").val();
}

function getMachineNum() {
    return $("#MachineNum").val();
}

function getItemNumber() {
    return $("#ItemNumber").val();
}

function getBarcode() {
    return $("#Barcode").val();
}

function getSubinventory() {
    return $("#Subinventory").val();
}

function initQueryTable() {

    QueryTable = $('#QueryTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        //orderMulti: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax

        dom:
            "<'row'<'col-sm-3 width-s'l><'col-sm-6'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        //"lengthMenu": [[200, 500, 1000, 2000], [200, 500, 1000, 2000]],
        ajax: {
            "url": "/Report/YieldQuery",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.cuttingDateFrom = getCuttingDateFrom();
                d.cuttingDateTo = getCuttingDateTo();
                d.batchNo = getBatchNo();
                d.machineNum = getMachineNum();
                d.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
            }
        },
        buttons: [
            {
                extend: 'excel',
                text: '匯出Excel',
                filename: function () {
                    return moment().format("YYYYMMDDHHmmss");
                }
            },
        ],
        columns: [
            { data: "BatchNo", "name": "工單號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Status", "name": "狀態", "autoWidth": true, "className": "dt-body-center" },
            { data: "DiItemNumber", "name": "組成成分料號", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "DiQty", "name": "噸數", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    } else {
                        return data;
                    }
                }
            },
            { data: "DoItemNumber", "name": "產品料號", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "DoReQty", "name": "令數", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    } else {
                        return data;
                    }
                }
            },
            {
                data: "DoQty", "name": "噸數", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    } else {
                        return data;
                    }
                }
            },
            { data: "Yield", "name": "得率", "autoWidth": true, "className": "dt-body-right" }
        ]

    });
}