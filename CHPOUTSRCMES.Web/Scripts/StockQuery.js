$(document).ready(function () {
   
    $('#btnSearch').click(function () {

        var subinventory = $("#SubinvenotoryCode option:selected").val(); 
        var locatorId = $("#LocatorId option:selected").val();
        var itemCategory = $("#ItemCategory option:selected").val();
        var itemNo = $("#ItemNumber").val();

        loadTable(subinventory, locatorId, itemCategory, itemNo);

    });

    $('#SubinvenotoryCode').on('change', function (e) {
        var valueSelected = this.value;
        loadLocator(valueSelected, $('#LocatorId'));
    });

    $(".ItemNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Stock/GetItemNumbers",
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
    $('#QueryTable tbody').on('click', '.available-query', function () {
        var data = $('#QueryTable').DataTable().row($(this).parents('tr')).data();

        var subinventoryCode = data['SubinventoryCode'];
        var locatorId = data['LocatorId'];
        var itemId = data['InventoryItemId'];

        window.open("/Stock/Detail/" + subinventoryCode + "/" + locatorId + "/" + itemId);
    });
});


function loadTable(subinventory, locatorId, itemCategory, itemNo) {

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
                'itemNo': itemNo,
                '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
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
            { data: "LocatorId", "name": "儲位ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "LocatorSegments", "name": "儲位", "autoWidth": true, "className": "dt-body-center"},
            { data: "InventoryItemId", "name": "料號ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "ItemNumber", "name": "料號", "autoWidth": true, "className": "dt-body-center" },
            { data: "ItemCategory", "name": "捲筒/平版", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "PrimaryAvailableQty", "name": "主單位可用量", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    }

                    return '<a href="' + '/Stock/Detail/' + full["SubinventoryCode"] + '/' + full["LocatorId"] + '/' + full["InventoryItemId"] + '" class="available-query">'
                        + data + '</a>';
                }
            },
            { data: "PrimaryUomCode", "name": "主要單位", "autoWidth": true, "className": "dt-body-center", "visible": false   },
            {
                data: "SecondaryAvailableQty", "name": "次單位可用量", "autoWidth": true, "className": "dt-body-right", "mRender": function (data, type, full) {

                    if (data == null || data == 0) {
                        return "";
                    }

                    return '<a href="' + '/Stock/Detail/' + full["SubinventoryCode"] + '/' + full["LocatorId"] + '/' + full["InventoryItemId"] + '" class="available-query">'
                        + data + '</a>';
                }
            },
            { data: "SecondaryUomCode", "name": "次要單位", "autoWidth": true, "className": "dt-body-center", "visible": false  },
            { data: "PrimarySumQty", "name": "主單位合計量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUomCode", "name": "主要單位", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "SecondarySumQty", "name": "次單位合計量", "autoWidth": true, "className": "dt-body-right" },
            { data: "SecondaryUomCode", "name": "次要單位", "autoWidth": true, "className": "dt-body-center", "visible": false}
        ]

    });
}

function loadLocator(subinventory, option) {
    var data = {
        'subinventory': subinventory,
        '__RequestVerificationToken' : $('input[name=__RequestVerificationToken]').val()
    };
    
    $.ajax({
        url: '/Stock/GetLocators',
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (data) {
            option.empty();
            if (data != null) {
                $.each(data, function (i, item) {
                    option.append($('<option></option>').val(item.Value).text(item.Text));
                });
            }
        },
        error: function () {
            alert('無法取得儲位清單');
        }
    });
}
