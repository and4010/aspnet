var t;
$(document).ready(function () {
   
    $('#btnSearch').click(function () {
        loadTable();
        //t.ajax.reload();
    });

    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    $('#ProcessDate').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $("#ProcessDate").val($.datepicker.formatDate("yy-mm-dd", new Date()));
    //$('#QueryTable tbody').on('click', '.available-query', function () {
    //    var data = $('#QueryTable').DataTable().row($(this).parents('tr')).data();

    //    var processCode = data['ProcessCode'];
    //    var serverCode = data['ServerCode'];
    //    var batchId = data['BatchId'];

    //    window.open("/Soa/Detail/" + processCode + "/" + serverCode + "/" + batchId);
    //});
    loadTable();

    function loadTable() {
        var processCode = $("#ProcessCodeList option:selected").val();
        var processDate = $("#ProcessDate").val();
        var hasError = $("#ErrorOptionList option:selected").val();

        initTable(processCode, processDate, hasError);
    }
});


function initTable(processCode, processDate, hasError) {

     t = $('#QueryTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        processing: true,
        serverSide: true,
        autoWidth: false,
        destroy:true,
        dom:
            "<'row'<'col-sm-3 width-s'l><'col-sm-6'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[50, 100, 200, 500], [50, 100, 200, 500]],
        ajax: {
            "url": "/Soa/SoaQuery",
            "type": "POST",
            "datatype": "json",
            "data": {
                'processCode': processCode,
                'processDate': processDate,
                'hasError': hasError,
                '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
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
            //{ data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center", "searchable": false },
            {
                "data": null, "sortable": false, "searchable": false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
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
         //'columnDefs': [{
         //    "targets": [0],
         //    "orderable": false,
         //    "sortable": false
         //}]
        //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
        //    $("td:first", nRow).html(iDisplayIndex + 1);
        //    return nRow;
        //},
         //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
         //    var oSettings = oAllLinksTable.fnSettings();
         //    $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);
         //    return nRow;
         //},
    });

    //t.on('order.dt search.dt', function () {
    //    t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();
}

