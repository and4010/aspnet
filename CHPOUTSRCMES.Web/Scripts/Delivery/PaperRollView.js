$(document).ready(function () {

    var PaperRollDataTablesBody = $('#PaperRollDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },

        //destroy: true,
        autoWidth: false,
        //rowId: 'PaperRollEditDT_ID',
        //select: true,
        serverSide: true,
        processing: true,
        //"paging": true,
        //"pagingType": "full_numbers",
        //"lengthMenu": [[1, 10, 5, 2], [1, 10, 5, 2]],
        //stateSave: true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetRollEdit",
            "type": "POST",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text(),
                DELIVERY_STATUS_NAME: $("#DELIVERY_STATUS").text()
            }
        },
        columns: [
         //{ data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
         { data: "ORDER_NUMBER", name: "訂單", autoWidth: true },
         { data: "ORDER_SHIP_NUMBER", name: "訂單行號", autoWidth: true },
         { data: "OSP_BATCH_NO", name: "工單號碼", autoWidth: true },
         { data: "TMP_ITEM_NUMBER", name: "代紙料號", autoWidth: true, className: "dt-body-left" },
            { data: "ITEM_NUMBER", name: "料號名稱", autoWidth: true, className: "dt-body-left" },
         { data: "PAPER_TYPE", name: "紙別", autoWidth: true },
         { data: "BASIC_WEIGHT", name: "基重", autoWidth: true },
         { data: "SPECIFICATION", name: "規格", autoWidth: true },
         { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right" },
         { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },
         { data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_PICKED_QUANTITY", name: "訂單已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true },

        ],

        "order": [[0, 'asc']],
        
    });

    var PaperRollBarcodeDataTablesBody = $('#PaperRollBarcodeDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        //destroy: true,
        autoWidth: false,
        //"pageLength": 1,
        serverSide: true,
        processing: true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetRollEditBarcode",
            "type": "Post",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text(),
                DELIVERY_STATUS_NAME: $("#DELIVERY_STATUS").text()
            },
        },
        columns: [
         //{ data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
         { data: "BARCODE", name: "條碼號", autoWidth: true },
         { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
         { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true, className: "dt-body-left" },
         //{ data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false },
        ],

        "order": [[0, 'desc']],
       

    });


  

});


