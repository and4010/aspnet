var queryTable;

$(document).ready(function () {

    $('#btnSearch').click(function () {
        queryTable.ajax.reload();
    });

    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    $('#ProcessDate').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    //SOA傳輸記錄表格
    queryTable = $('#QueryTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        processing: true,
        serverSide: true,
        autoWidth: false,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        dom:
            "<'row'<'col-sm-3 width-s'l><'col-sm-6'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[50, 100, 200, 500], [50, 100, 200, 500]],
        ajax: {
            "url": "/Soa/SoaQuery",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.processCode = $("#ProcessCodeList option:selected").val();
                d.processDate = $("#ProcessDate").val();
                d.hasError = $("#ErrorOptionList option:selected").val();
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
        "order": [[2, 'desc']],
        columns: [
            {
                "data": null, "sortable": false, "searchable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { data: "ProcessCode", "name": "傳輸代號", "autoWidth": true, "className": "dt-body-center" },
            { data: "ProcessName", "name": "傳輸類型", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "ProcessDate", "name": "傳輸日期", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY/MM/DD HH:mm:ss');
                    } else {
                        return '';
                    }
                }
            },
            {
                data: "BatchId", "name": "傳輸ID", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {
                    if (data == null || data == 0) {
                        return "";
                    }

                    return '<a href="/Soa/Detail/' + full["ProcessCode"] + '/' + full["ServerCode"] + '/' + full["BatchId"] + '" class="available-query">'
                        + data + '</a>';
                }
            },
            { data: "RowNum", "name": "傳輸筆數", "autoWidth": true, "className": "dt-body-right" },
            { data: "StatusCode", "name": "狀態", "autoWidth": true, "className": "dt-body-center" },
            { data: "ErrorMsg", "name": "錯誤訊息", "autoWidth": true, "className": "dt-body-right" },
            { data: "SoaPullingFlag", "name": "SOA傳輸狀態", "autoWidth": true, "className": "dt-body-center" },
            { data: "SoaErrorMsg", "name": "SOA錯誤訊息", "autoWidth": true, "className": "dt-body-right" },
            { data: "SoaProcessCode", "name": "SOA狀態", "autoWidth": true, "className": "dt-body-center" }
        ],
    });
});

//於$(document).ready執行後才會執行，不可放在$(document).ready前。
//當按回上一頁來到此頁時，Firefox不會執行$(document).ready和window.onload，IE和Chrome都會執行。
window.onload = function () {
    //日期欄位初始化
    var processDate = $("#ProcessDate").val();
    if (processDate) {
        $("#ProcessDate").val(processDate);
    } else {
        //沒有日期時使用本機日期
        $("#ProcessDate").val($.datepicker.formatDate("yy-mm-dd", new Date()));
    }
    queryTable.ajax.reload();
};
