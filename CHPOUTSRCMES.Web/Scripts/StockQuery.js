$(document).ready(function () {
   
    $('#btnSearch').click(function () {

        var subinventory = $("#SubinvenotoryCode").val();
        var locatorId = $("#LocatorId").val();
        var itemCategory = $("#ItemCategory").val();
        var itemNo = $("#ItemNumber").val();

        LoadTable(subinventory, locatorId, itemCategory, itemNo);

    });

});


function LoadTable(subinventory, locatorId, itemCategory, itemNo) {

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
            "url": "/Stock/StockQuery",
            "type": "POST",
            "datatype": "json",
            "data": {
                'subinventory': subinventory,
                'locatorId': locatorId,
                'itemCategory': itemCategory,
                'itemNo': item
            }
        },
        buttons: [
            {
                extend: 'excel',
                text: '匯出Excel'
            },
        ],
        columns: [
            { data: "SubinventoryCode", "name": "倉庫", "autoWidth": true, "className": "dt-body-center"},
            { data: "LocatorId", "name": "儲位ID", "autoWidth": true, "className": "dt-body-center"},
            { data: "LocatorSegments", "name": "儲位", "autoWidth": true, "className": "dt-body-center"},
            { data: "InventoryItemId", "name": "料號ID", "autoWidth": true, "className": "dt-body-center"},
            { data: "ItemNumber", "name": "料號", "autoWidth": true, "className": "dt-body-center"},
            { data: "PrimaryAvailableQty", "name": "主單位可用量", "autoWidth": true, "className": "dt-body-right"},
            { data: "PrimarySumQty", "name": "主單位合計量", "autoWidth": true, "className": "dt-body-right"},
            { data: "PrimaryUomCode", "name": "主要單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "SecondaryAvailableQty", "name": "次單位可用量", "autoWidth": true, "className": "dt-body-right" },
            { data: "SecondarySumQty", "name": "次單位合計量", "autoWidth": true, "className": "dt-body-right"},
            { data: "SecondaryUomCode", "name": "次要單位", "autoWidth": true, "className": "dt-body-center"}
        ]

    });
}

