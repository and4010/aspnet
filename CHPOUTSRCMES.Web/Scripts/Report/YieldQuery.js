var QueryTable;


$(document).ready(function () {

    $('#BatchNo').combobox();
    $('#MachineNum').combobox();

    $('#btnSearch').click(function () {
        QueryTable.ajax.reload();
    });

    $('#btnPrint').click(function () {
        //var shipmentNumber = getShipmentNumber();
        //if (shipmentNumber == "新增編號") {
        //    swal.fire('請選擇出貨編號');
        //    event.preventDefault();
        //    return;
        //}
        window.open("/Report/OspYieldReport/?cuttingDateFrom=" + getCuttingDateFrom() + "&cuttingDateTo=" + getCuttingDateTo() + "&batchNo=" + getBatchNo() + "&machineNum=" + getMachineNum());
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

    initQueryTable();
});

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
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
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
                text: '匯出Excel'
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