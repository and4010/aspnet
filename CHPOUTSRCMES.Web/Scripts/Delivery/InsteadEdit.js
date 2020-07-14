$(document).ready(function () {

    var InsteadDataTablesBody = null;
    var InsteadBarcodeDataTablesBody = null;

    ImportInsteadEditDT();
    ImportInsteadBarcodeEditDT();


});




function ImportInsteadEditDT() {
    InsteadDataTablesBody = $('#InsteadDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        destroy: true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetInsteadEdit",
            "type": "GET",
            "datatype": "json",
            "data": {
                DELIVERY_NAME: $("#DELIVERY_NAME").text(),
                TRIP_NAME: $("#TRIP_NAME").text()
            },
        },
        columns: [
         { data: null, name: "項次", autoWidth: true },
         { data: "ORDER_NUMBER", name: "訂單編號", autoWidth: true },
         { data: "OSP_BATCH_NO", name: "工單號碼", autoWidth: true },
         { data: "ITEM_DESCRIPTION", name: "料號名稱", autoWidth: true },
         { data: "TMP_ITEM_DESCRIPTION", name: "代紙料號名稱", autoWidth: true },

          { data: "PAPER_TYPE", name: "紙別", autoWidth: true },
           { data: "BASIC_WEIGHT", name: "基重", autoWidth: true },
            { data: "SPECIFICATION", name: "規格", autoWidth: true },

         { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true },
         { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true },
         { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },

         //{ data: "REQUESTED_QUANTITY2", name: "預計出庫輔數量", autoWidth: true },
         //{ data: "PICKED_QUANTITY2", name: "出庫已揀輔數量", autoWidth: true },
         //{ data: "SRC_REQUESTED_QUANTITY_UOM2", name: "輔單位", autoWidth: true },

         //{ data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", autoWidth: true },
         //{ data: "SRC_PICKED_QUANTITY", name: "訂單已揀數量", autoWidth: true },
         //{ data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", autoWidth: true },


        ],

        "order": [[0, 'asc']]

    });

    InsteadDataTablesBody.on('order.dt search.dt', function () {
        InsteadDataTablesBody.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}


function ImportInsteadBarcodeEditDT() {
    InsteadBarcodeDataTablesBody = $('#InsteadBarcodeDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        destroy: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetInsteadEditBarcode",
            "type": "GET",
            "datatype": "json",
            "data": {
                DELIVERY_NAME: $("#DELIVERY_NAME").text()
            },
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "80px" },
         { data: null, name: "項次", autoWidth: true },
         { data: "ITEM_DESCRIPTION", name: "料號名稱", autoWidth: true },
          { data: "BARCODE", name: "條碼號", autoWidth: true },
       
         { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true },
         { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
         //{ data: "SECONDARY_QUANTITY", name: "次要數量", autoWidth: true },
         //{ data: "SECONDARY_UOM", name: "次要數量", autoWidth: true },
         { data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false },
        ],

        order: [[6, 'desc']],
        select: {
            style: 'multi',
            blurable: true,
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone',
            {
                text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                //className: 'btn-default btn-sm',
                action: function (e) {
                    PrintLable(PaperRolldataTablesBody, "/Purchase/GetLabel");
                }
            }
        ],

    });

    InsteadBarcodeDataTablesBody.on('order.dt search.dt', function () {
        InsteadBarcodeDataTablesBody.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}