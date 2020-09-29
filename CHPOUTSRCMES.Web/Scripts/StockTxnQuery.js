$(document).ready(function () {
   
    $('#btnSearch').click(loadTable2);

    $('#SubinvenotoryCode1').on('change', function (e) {
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


    loadTable2();
});

function loadTable2() {
    var subinventory = $("#SubinvenotoryCode1 option:selected").val();
    var locatorId = $("#LocatorId option:selected").val();
    var itemCategory = $("#ItemCategory option:selected").val();
    var itemNo = $("#ItemNumber").val();
    var barcode = $("#Barcode").val();

    loadTable(subinventory, locatorId, itemCategory, itemNo, barcode);
}


function loadTable(subinventory, locatorId, itemCategory, itemNo, barcode) {

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
        "lengthMenu": [[50, 100, 200, 300], [50, 100, 200, 300]],
        ajax: {
            "url": "/Stock/StockTxnQuery",
            "type": "POST",
            "datatype": "json",
            "data": {
                'subinventory': subinventory,
                'locatorId': locatorId,
                'itemCategory': itemCategory,
                'itemNo': itemNo,
                'barcode': barcode, 
                '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
            }
        },
        buttons: [
            {
                extend: 'excel',
                text: '匯出Excel'
            }
        ],
        columns: [
            { data: "Barcode", "name": "條碼", "autoWidth": true, "className": "dt-body-center" },
            { data: "InventoryItemId", "name": "料號ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "ItemNumber", "name": "料號", "autoWidth": true, "className": "dt-body-center" },
            { data: "ItemCategory", "name": "捲筒/平版", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "OrganizationId", "name": "組織ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "OrganizationCode", "name": "組織代號", "autoWidth": true, "className": "dt-body-center" },
            { data: "OrganizationName", "name": "組織名稱", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "SubinventoryCode", "name": "倉庫代號", "autoWidth": true, "className": "dt-body-center" },
            { data: "SubinventoryName", "name": "倉庫名稱", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "LocatorId", "name": "儲位ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "LocatorSegment3", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "TrfOrganizationId", "name": "移轉組織ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "TrfOrganizationCode", "name": "移轉組織代號", "autoWidth": true, "className": "dt-body-center" },
            { data: "TrfOrganizationName", "name": "移轉組織名稱", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "TrfSubinventoryCode", "name": "移轉倉庫代號", "autoWidth": true, "className": "dt-body-center" },
            { data: "TrfSubinventoryName", "name": "移轉倉庫名稱", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "TrfLocatorId", "name": "移轉儲位ID", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "TrfLocatorSegment3", "name": "移轉儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "AvailableQty", "name": "剩餘量", "autoWidth": true, "className": "dt-body-center" },
            { data: "ChangedQty", "name": "變動量", "autoWidth": true, "className": "dt-body-center" },
            { data: "UomCode", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "ActionName", "name": "原因", "autoWidth": true, "className": "dt-body-center", "mRender": function (data, type, full) {
                    return full["Category"] + "-(" + data + ")";
                }
            },
            { data: "Category", "name": "作業", "autoWidth": true, "className": "dt-body-center", "visible": false },
            { data: "DocNumber", "name": "單號", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "CreateDate", "name": "時間", "autoWidth": true, "className": "dt-body-center", "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY/MM/DD HH:mm:ss');
                    } else {
                        return '';
                    }
                }
            }
            
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
