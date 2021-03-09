
var QueryTable;


$(document).ready(function () {

    $('#btnSearch').click(function () {
        QueryTable.ajax.reload();
    });

    $('#btnPrint').click(function () {
        ospYieldReport();
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

    $(".ItemNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Report/GetItemNumbers",
                type: "POST",
                dataType: "json",
                data: {
                    'Prefix': request.term,
                    'itemNo': $("#ItemNumber").val(),
                    '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Value + " " + item.Description, value: item.Value };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });

});

function ospYieldReport() {
    var batchNo = getBatchNo();
    if (batchNo && batchNo.length < 4) {
        swal.fire("工單號須輸入4碼以上");
        return;
    }

    ShowWait(
        function () {
            

            $.ajax({
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
                    swal.fire('更新報表失敗');
                }
            })
        }
    );
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
