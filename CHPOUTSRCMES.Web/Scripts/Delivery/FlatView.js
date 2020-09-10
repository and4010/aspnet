$(document).ready(function () {

    var FlatDataTablesBody = $('#FlatDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetFlatEdit",
            "type": "Post",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text(),
                DELIVERY_STATUS_NAME: $("#DELIVERY_STATUS").text()
            },
        },
        columns: [
         { data: "SUB_ID", name: "項次", autoWidth: true },
         { data: "ORDER_NUMBER", name: "訂單", autoWidth: true },
         { data: "ORDER_SHIP_NUMBER", name: "訂單行號", autoWidth: true },
         { data: "OSP_BATCH_NO", name: "工單號碼", autoWidth: true },
         { data: "TMP_ITEM_NUMBER", name: "代紙料號", autoWidth: true, className: "dt-body-left" },
         { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
         { data: "REAM_WEIGHT", name: "令重", autoWidth: true },
         { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },

         { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right" },
         { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },

         { data: "REQUESTED_QUANTITY2", name: "預計出庫輔數量", autoWidth: true, className: "dt-body-right" },
         { data: "PICKED_QUANTITY2", name: "出庫已揀輔數量", autoWidth: true, className: "dt-body-right" },
         { data: "REQUESTED_QUANTITY_UOM2", name: "輔單位", autoWidth: true },

         { data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_PICKED_QUANTITY", name: "訂單已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true },

        ],

        "order": [[0, 'asc']],
       
        

    });

    

    


    var FlatBarcodeDataTablesBody = $('#FlatBarcodeDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        //pageLength: 2,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetFlatEditBarcode",
            "type": "POST",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text(),
                DELIVERY_STATUS_NAME: $("#DELIVERY_STATUS").text()
            },
        },
        columns: [
            { data: "SUB_ID", name: "項次", autoWidth: true },
          { data: "BARCODE", name: "條碼號", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
          { data: "REAM_WEIGHT", name: "令重", autoWidth: true },
         { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },
         { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
         { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
         { data: "SECONDARY_QUANTITY", name: "次要數量", autoWidth: true, className: "dt-body-right" },
         { data: "SECONDARY_UOM", name: "次要單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true, className: "dt-body-left" },
         //{ data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false },
         
        ],

        order: [[0, 'desc']],
        

    });



    

});



