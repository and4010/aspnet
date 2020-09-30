
$(document).ready(function () {
    loadTable(
        $('#ProcessCode').val(),
        $('#ServerCode').val(),
        $('#BatchId').val()
    );

});


function loadTable(processCode, serverCode, batchId) {

    $('#QueryTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        processing: true,
        serverSide: true,
        autoWidth: false,
        destroy:true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[50, 100, 200, 500], [50, 100, 200, 500]],
        ajax: {
            "url": "/Soa/SoaDetailQuery",
            "type": "POST",
            "datatype": "json",
            "data": {
                'processCode': processCode,
                'serverCode': serverCode,
                'batchId': batchId,
                '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
            }
        },
        columns: [
            { data: "DocNumber", "name": "庫存ID", "autoWidth": true, "className": "dt-body-center" },
            { data: "InventoryItemId", "name": "倉庫", "autoWidth": true, "className": "dt-body-center"},
            { data: "ItemNumber", "name": "儲位ID", "autoWidth": true, "className": "dt-body-center" },
            { data: "StatusCode", "name": "儲位", "autoWidth": true, "className": "dt-body-center"},
            { data: "ErrorMsg", "name": "料號ID", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "PrimaryQuantity", "name": "重量(KG)", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    }

                    return data;
                }
            },
            { data: "PrimaryUom", "name": "主要單位", "autoWidth": true, "className": "dt-body-center"  },
            {
                data: "SecondaryQuantity", "name": "令數(RE)", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    }

                    return data;
                }
            },
            { data: "SecondaryUom", "name": "次要單位", "autoWidth": true, "className": "dt-body-center"  }
        ]

    });
}

