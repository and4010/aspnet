
$(document).ready(function () {
    loadTable(
        $('#ProcessCode').val(),
        $('#ServerCode').val(),
        $('#BatchId').val()
    );

    $('#btnResend').click(function (e) {
        e.preventDefault();
        swal.fire({
            title: '重送資料?',
            text: "您是否確定要將資料重新送回ERP?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '傳送!'
        }).then(function (result) {
            if (result.value) {

                $.ajax({
                    url: '/Soa/LogOff',
                    type: "POST",
                    datatype: "json",
                    data: {
                        '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val(),
                        'ProcessCode': '',
                        'ServerCode': '',
                        'BatchId' : ''
                    },
                    success: function (response) {
                        if (response.status) {
                            location.reload();
                        }
                    },
                    error: function () {
                        Swal.fire("發現未預期錯誤!!");
                    }

                });
            }
        });



    })

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
        "lengthMenu": [[100, 150, 200, 500], [100, 150, 200, 500]],
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

