var EditorPaperRoll
var EditorFlat
var InventoryPaperRollTable
var InventoryFlatTable
$(document).ready(function () {
    OnClick();
    LoadPaperRollTable();
    LoadFlatTable();
    LoadPaperRollRecordTable();
    LoadFlatRecordTable();


    InventoryPaperRollTable.on('click', '#BtnInventory', function (e) {
        e.preventDefault();
        EditorPaperRoll.edit($(this).closest('tr'), {
            title: '盤點',
            buttons: [
                '確定',
                {
                    label: '取消',
                    fn: function () {
                        EditorPaperRoll.close();
                    }
                }
            ],
        });
    });

    InventoryPaperRollTable.on('click', '#BtnRecord', function (e) {
        var data = $('#InventoryPaperRollTable').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        }
        window.location.href = "/Inventory/PaperRollRecord/" + id;
    });

    InventoryFlatTable.on('click', '#BtnInventory', function (e) {
        e.preventDefault();
        EditorFlat.edit($(this).closest('tr'), {
            title: '盤點',
            buttons: [
                '確定',
                {
                    label: '取消',
                    fn: function () {
                        EditorFlat.close();
                    }
                }
            ],
        });
    });

    InventoryFlatTable.on('click', '#BtnRecord', function (e) {
        var data = $('#InventoryFlatTable').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        }
        window.location.href = "/Inventory/FlatRecord/" + id;
    });

});

function OnClick() {
    $('#BtnPaperRollSearch').click(function () {
        var Subinventory = $('#Subinventory').val();
        var Item_No = $('#Item_No').val();
        var PaperType = $('#PaperType').val();
        var LotNumber = $('#LotNumber').val();
        LoadPaperRollTable(Subinventory, Item_No, PaperType, LotNumber);


    });

    $('#BtnFlatSearch').click(function () {
        var Subinventory = $('#FlatSubinventory').val();
        var Item_No = $('#FlatItem_No').val();
        var PackingType = $('#PackingType').val();
        LoadFlatTable(Subinventory, Item_No, PackingType);
    });
}

function LoadPaperRollTable(Subinventory, Item_No, PaperType, LotNumber) {

    EditorPaperRoll = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Inventory/EditorPaperRoll',
            type: 'post',
            datatype: 'json',
            contentType: 'application/json',
            data: function (d) {
                var PaperRollList = [];
                $.each(d.data, function (key, value) {

                    var PaperRoll = {
                        'Id': d.data[key]['Id'],
                        'FirmStock': d.data[key]['FirmStock'],
                        'Reason': d.data[key]['Reason'],
                        'TransactionQuantity': d.data[key]['TransactionQuantity']
                    }
                    PaperRollList.push(PaperRoll);
                });

                var data = {
                    'Action': d.action,
                    'PaperRollList': PaperRollList
                }

                return JSON.stringify(data);
            },
            success: function () {
                
            }
        },
        table: "#InventoryPaperRollTable",
        formOptions: {
            main: {
                onBackground: 'none'
            }
        },
        idSrc: 'Id',
        fields: [
            {
                name: "Id"
            },
            {
                label: "條碼號",
                name:"Barcode"
            },
            {
                label: "料號",
                name:"Item_No"
            },
            {
                label: "在庫",
                name:"Stock"
            },
            {
                label: "實盤在庫",
                name: "FirmStock",
                type:'select',
                options: [
                    { label: '否', value: '否' },
                    { label: '是', value: '是' }
                ]
            },
            {
                label: "異動數量",
                name: "TransactionQuantity"
            },
            {
                label: "盤盈盤虧",
                name:"Panying"
            },
            {
                label: "異動原因",
                name:"Reason"
            }

        ]
    });

    EditorPaperRoll.disable(['Barcode', 'Item_No', 'Stock', 'Panying']);
    EditorPaperRoll.hide(['Id']);

    InventoryPaperRollTable = $('#InventoryPaperRollTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Inventory/LoadPaperRollTable",
            "type": "POST",
            "datatype": "json",
            "data": {
                Subinventory: Subinventory,
                Item_No: Item_No,
                PaperType: PaperType,
                LotNumber: LotNumber
            }
        },
        columnDefs: [{
            orderable: false, targets: [9, 10], width: "60px",
        }],
        columns: [
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "PaperType", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "BaseWeight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
            { data: "TheoreticalWeight", "name": "理論重(KG)", "autoWidth": true, "className": "dt-body-center" },
            { data: "LotNumber", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Stock", "name": "在庫", "autoWidth": true, "className": "dt-body-center" },
            {
                data:"", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return '<button class="btn btn-primary btn-sm" id= "BtnInventory">盤點</button>';
                }
            },
            {
                data: "", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return '<button class="btn btn-primary btn-sm" id= "BtnRecord">異動紀錄</button>';
                },
            },
        ],

    });
}

function LoadFlatTable(Subinventory, Item_No, PackingType) {

    EditorFlat = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Inventory/EditorFlat',
            type: 'post',
            datatype: 'json',
            contentType: 'application/json',
            data: function (d) {
                var FlatList = [];
                $.each(d.data, function (key, value) {

                    var Flat = {
                        'Id': d.data[key]['Id'],
                        'StockReam_Qty': d.data[key]['StockReam_Qty'],
                        'Reason': d.data[key]['Reason']
                    }
                    FlatList.push(Flat);
                });

                var data = {
                    'Action': d.action,
                    'FlatList': FlatList
                }

                return JSON.stringify(data);
            },
            success: function () {

            }

        },
        table: "#InventoryFlatTable",
        formOptions: {
            main: {
                onBackground: 'none'
            }
        },
        idSrc: 'Id',
        fields: [
            {
                name:"Id"
            },
            {
                label: "條碼號",
                name: "Barcode"
            },
            {
                label: "料號",
                name: "Item_No"
            },
            {
                label: "令數",
                name: "Ream_Qty"
            },
            {
                label: "實盤令數",
                name: "StockReam_Qty",
            },
            {
                label: "盤盈盤虧",
                name: "Panying"
            },
            {
                label: "異動原因",
                name: "Reason"
            }

        ]

    });

    EditorFlat.disable(['Barcode', 'Item_No', 'Ream_Qty', 'Panying']);
    EditorFlat.hide(['Id']);

    InventoryFlatTable = $('#InventoryFlatTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Inventory/LoadFlatTable",
            "type": "POST",
            "datatype": "json",
            "data": {
                Subinventory: Subinventory,
                Item_No: Item_No,
                PackingType: PackingType
            }
        },
        columnDefs: [{
            orderable: false, targets: [6, 7], width: "60px",
        }],
        columns: [
            { data: "Id", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "ReamWeight", "name": "令重", "autoWidth": true, "className": "dt-body-center" },
            { data: "PackingType", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center" },
            { data: "Ream_Qty", "name": "令數", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "","autoWidth": true, "className": "dt-body-center", render: function () {
                    return '<button class="btn btn-primary btn-sm" id= "BtnInventory">盤點</button>';
                }
            },
            {
                data: "", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return '<button class="btn btn-primary btn-sm" id= "BtnRecord">異動紀錄</button>';
                }
            },
        ],

    });
}


function LoadPaperRollRecordTable() {
    var Id = $('#Id').val();
    $('#PaperRollInventoryRecordTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Inventory/LoadRecordPaperRollTable",
            "type": "POST",
            "datatype": "json",
            "data": {
                Id: Id
            }
        },
        columns: [
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Panying", "name": "動作", "autoWidth": true, "className": "dt-body-center" },
            { data: "TransactionQuantity", "name": "異動數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "Reason", "name": "異動原因", "autoWidth": true, "className": "dt-body-center" },
            { data: "Created_by", "name": "異動人", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "Last_Updated_Date", "name": "異動時間", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD HH:mm:ss');
                    } else {
                        return '';
                    }

                }
            },
        ],

    });



}


function LoadFlatRecordTable() {
    var Id = $('#Id').val();
    $('#FlatInventoryRecordTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Inventory/LoadRecordFlatTable",
            "type": "POST",
            "datatype": "json",
            "data": {
                Id: Id,
            }
        },
        columns: [
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Panying", "name": "動作", "autoWidth": true, "className": "dt-body-center" },
            { data: "StockReam_Qty", "name": "異動數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "Reason", "name": "異動原因", "autoWidth": true, "className": "dt-body-center" },
            { data: "Created_by", "name": "異動人", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "Last_Updated_Date", "name": "異動時間", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD HH:mm:ss');
                    } else {
                        return '';
                    }



                }
            },
        ],

    });



}