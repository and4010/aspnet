﻿
$(document).ready(function () {
    loadTable(
        $('#SubinventoryCode').val(),
        $('#LocatorId').val(),
        $('#InventoryItemId').val(),
        $('#ItemCategory').val(),
    );

});


function loadTable(subinventory, locatorId, itemId, itemCategory) {

    $('#QueryTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        processing: true,
        serverSide: true,
        autoWidth: false,
        destroy:true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[500, 1000, 2000, 3000], [500, 1000, 2000, 3000]],
        ajax: {
            "url": "/Stock/StockDetailQuery",
            "type": "POST",
            "datatype": "json",
            "data": {
                'subinventory': subinventory,
                'locatorId': locatorId,
                'itemId': itemId,
                '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
            }
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone',
            {
                extend: 'excel',
                text: '匯出Excel'
            },
            {
                text: '列印標籤',
                action: printlabel
            }
        ],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "StockId", "name": "庫存ID", "autoWidth": true, "className": "dt-body-center", visible: false },
            { data: "SubinventoryCode", "name": "倉庫", "autoWidth": true, "className": "dt-body-center"},
            { data: "LocatorId", "name": "儲位ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "LocatorSegments", "name": "儲位", "autoWidth": true, "className": "dt-body-center"},
            { data: "InventoryItemId", "name": "料號ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "ItemNumber", "name": "料號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Barcode", "name": "條碼", "autoWidth": true, "className": "dt-body-center" },
            { data: "LotNumber", "name": "捲號", "autoWidth": true, "className": "dt-body-center", "visible": itemCategory == '捲筒' },
            { data: "PrimaryAvailableQty", "name": "主單位可用量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUomCode", "name": "主要單位", "autoWidth": true, "className": "dt-body-center", "visible": false  },
            { data: "SecondaryAvailableQty", "name": "次單位可用量", "autoWidth": true, "className": "dt-body-right" },
            { data: "SecondaryUomCode", "name": "次要單位", "autoWidth": true, "className": "dt-body-center", "visible": false  },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center", "visible": itemCategory == '捲筒' },
            { data: "BasicWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center", "visible": itemCategory == '捲筒' },
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center", "visible": itemCategory == '捲筒' },
            { data: "ReamWeight", "name": "令重", "autoWidth": true, "className": "dt-body-right", "visible": itemCategory == '平版' },
            { data: "PackingType", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center", "visible": itemCategory == '平版' }
        ]

    });
}


function printlabel() {


    var table = $('#QueryTable').DataTable();

    if (table.rows('.selected').data().length == 0) {
        $.swal.alert("請先選擇欲列印之標籤, 再按下列印標籤!!");
        return;
    }

    PrintLable(table, "/Stock/PrintLabel", "1");
}