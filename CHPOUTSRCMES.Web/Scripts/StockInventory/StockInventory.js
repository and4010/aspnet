var HeaderTable;
var BodyTable;
var EditorBody;
var Pay;   //0盤盈 1盤虧
var selected = [];

function SendInventoryInit() {
    GetTop();
    onClick();
    LoadHeaderTable();
    LoadBody();
    event();
    onkey();

    HeaderTable.on('select', function (e, dt, type, indexes) {
        if (type = 'row') {
            var ID = dt.rows(indexes).data().pluck('ID')[0];
            $("#ID").text(ID);
            var Subinventory = dt.rows(indexes).data().pluck('SUBINVENTORY_CODE')[0];
            $("#Subinventory").text(Subinventory);
            var Locator = dt.rows(indexes).data().pluck('SEGMENT3')[0];
            $("#Locator").text(Locator);
            var ItemNo = dt.rows(indexes).data().pluck('ITEM_NO')[0];
            $("#ItemNo").text(ItemNo);
            var Barcode = dt.rows(indexes).data().pluck('BARCODE')[0];
            $('#Barcode').text(Barcode);

            var ITEM_CATEGORY = dt.rows(indexes).data().pluck('ITEM_CATEGORY')[0];
            if (ITEM_CATEGORY == "平版") {
                var Unit = dt.rows(indexes).data().pluck('SECONDARY_UOM_CODE')[0];
                $('#Unit').text(Unit);
            } else if (ITEM_CATEGORY == "捲筒") {
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                $('#Unit').text(Unit);
            } else {
                $('#Unit').text("");
            }


            var rowsData = HeaderTable.rows({ page: 'current' }).data();
            for (i = 0; i < rowsData.length; i++) {
                for (j = 0; j < selected.length; j++) {
                    if (selected[j] == rowsData[i].ID) {
                        selected.splice(j, 1);
                    }
                }
            }

            var index = $.inArray(ID, selected);
            if (index === -1) {
                selected.push(ID);
            }
        }

    });

    HeaderTable.on('deselect', function (e, dt, type, indexes) {
        if (type = 'row') {
            clearText();
            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            selected.splice(index, 1);
        }

    });


};

function SubinventoryChangeCallBack() {

}


function LocatorChangeCallBack() {

}


function event() {

    var ddlType = $("#ddlProfit").val();
    if (ddlType == "紙捲") {
        $('#Unit').text("KG");
    }
    if (ddlType == "平張") {
        $('#Unit').text("RE");
    }


    var ddlProfit = $("#ddlProfit").val();
    if (ddlProfit == "盤盈") {
        $('#QtyText').text("增加數量");
        Pay = 0;
    }
    if (ddlProfit == "盤虧") {
        $('#QtyText').text("減少數量");
        Pay = 1;
        $('#BtnInsert').hide();
    }

}

function onkey() {
    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#Qty").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnSave").click();
        }
    });
}


function onClick() {
    $('#Search').click(function () {
        var SubinventoryCode = $('#ddlSubinventory').val();
        var ddlLocator = $('#ddlLocator').val();
        var ddlItemNo = $('#txtItemNumber').val();

        LoadHeaderTable(SubinventoryCode, ddlLocator, ddlItemNo, Pay); 
        LoadBody();
    });


    $('#BtnSave').click(function () {
        var qty = $("#Qty").val();
        if (selected.length == 0) {
            swal.fire("請先選擇一個項目");
            return;
        }
        if (qty.length == 0) {
            swal.fire("請輸入數量");
            return;
        }
        Save();
    });


    $('#BtnInsert').click(function (e) {
        e.preventDefault();
        EditorBody.hide(['ID'])
        var SubinventoryCode = $('#ddlSubinventory').val();
        var ddlLocator = $('#ddlLocator').val();
        var ddlItemNo = $('#txtItemNumber').val();

        if (SubinventoryCode == "請選擇") {
            swal.fire("請先選擇倉庫");
            return;
        }

        if ($('#ddlLocatorArea').is(":visible")) {
            if (ddlLocator == "請選擇") {
                swal.fire('請選擇儲位');
                event.preventDefault();
                return;
            }
        }

        if (ddlItemNo.length == 0) {
            swal.fire("請先輸入料號");
            return;
        }

        EditorBody.create({
            title: '新增條碼',
            buttons: '確定'
        });
        EditorBody.show(['LOT_NUMBER', 'PRIMARY_AVAILABLE_QTY', 'ITEM_NO']);
    
    });

}

function Save() {
    var Qty = $("#Qty").val();
    var data = HeaderTable.column(1).data();
    var Id = data[0];
    $.ajax({
        url: "/StockInventory/SaveQtyBodyDataTables",
        type: "POST",
        datatype: "json",
        data: { Id: Id, Qty: Qty, Pay: Pay },
        success: function (data) {
            if (data.resultModel.Success) {
                LoadBody();
                clearText();
            } else {
                swal.fire(data.resultModel.Msg);
            }
          
        }

    });
}


function LoadHeaderTable(SubinventoryCode, Locator, ItemNumber, Pay) {

    HeaderTable = $('#HeaderDataTables').DataTable({
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
            "url": "/StockInventory/LoadHeaderDataTables",
            "type": "POST",
            "datatype": "json",
            "data": {
                SubinventoryCode: SubinventoryCode,
                Locator: Locator,
                ItemNumber: ItemNumber,
                Pay: Pay
            },
        },
        select: {
            style: 'single',
        },
        columnDefs: [{
            orderable: false, targets: [0], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "ID", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "SUBINVENTORY_CODE", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "SEGMENT3", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "ITEM_NO", "name": "料號", "autoWidth": true, "className": "dt-body-center" },
            { data: "BARCODE", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "PRIMARY_AVAILABLE_QTY", "name": "數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PRIMARY_UOM_CODE", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "SECONDARY_AVAILABLE_QTY", "name": "數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "SECONDARY_UOM_CODE", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
        ],

    });

}



function LoadBody() {
    EditorBody = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockInventory/EditorBody',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: function (d) {
                var StockInventoryModel;
                $.each(d.data, function (key, value) {
                    StockInventoryModel = {
                        'ID': d.data[key]['ID'],
                        'NOTE': d.data[key]['NOTE'],
                        'PRIMARY_AVAILABLE_QTY': d.data[key]['PRIMARY_AVAILABLE_QTY'],
                        'LOT_NUMBER': d.data[key]['LOT_NUMBER'],
                        'ITEM_NO': d.data[key]['ITEM_NO'],
                        'SEGMENT3': $('#ddlLocator').val(),
                        'SUBINVENTORY_CODE': $('#ddlSubinventory').val(),
                    };
                });
                var data = {
                    'Pay' : Pay,
                    'Action': d.action,
                    'StockInventoryModel': StockInventoryModel
                };
                return JSON.stringify(data);
            },
            success: function () {
                LoadBody();
            }
        },
        table: "#BodyDataTables",
        formOptions: {
            main: {
                onBackground: 'none'
            }
        },
        idSrc: 'ID',
        fields: [
            {
                name: "ID"
            },
            {
                label: "倉庫",
                name: "SUBINVENTORY_CODE"
            },
            {
                label: "儲位",
                name: "SEGMENT3"
            },
            {
                label: "料號",
                name: "ITEM_NO"
            },
            {
                label: "捲號",
                name:"LOT_NUMBER"
            },
            {
                label: "數量",
                name: "PRIMARY_AVAILABLE_QTY"
            },
            {
                label: "備註",
                name: "NOTE"
            }

        ],
        i18n: {
            create: {
                button: "新增",
                title: "新增",
                submit: "確定",
                action: 'btn-primary'
            },
            remove: {
                button: '刪除',
                title: "刪除",
                submit: "確定",
                confirm: {
                    "_": "你確定要刪除這筆資料?",
                    "1": "你確定要刪除這筆資料?"
                }
            },
            edit: {
                button: '編輯',
                title: "編輯",
                submit: "確定",
            }
        }
    });


    EditorBody.hide(['ID', 'SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO', 'PRIMARY_AVAILABLE_QTY','LOT_NUMBER'])

    var SubinventoryCode = $('#ddlSubinventory').val();
    var Locator = $('#ddlLocator').val();
    var ItemNumber = $('#txtItemNumber').val();

    BodyTable = $('#BodyDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockInventory/LoadBodyDataTables",
            "type": "POST",
            "datatype": "json",
            "data": {
                SubinventoryCode: SubinventoryCode,
                Locator: Locator,
                ItemNumber: ItemNumber,
                Pay:Pay,
            },
        },
        select: {
            style: 'multi',
        },
        buttons: [
            //{
            //    extend: "create",
            //    text: '新增條碼',
            //    name: 'create',
            //    className: 'btn-primary',
            //    editor: EditorBody,
            //}
            ,
            {
                extend: "selectAll",
            }
            ,
            {
                extend: "selectNone",
            }
            ,
            {
                extend: "remove",
                text: '刪除',
                name: 'remove',
                className: 'btn-danger',
                editor: EditorBody,
            }
            ,
            {
                extend: 'edit',
                text: '編輯備註',
                name: 'edit',
                className: 'btn-danger',
                editor: EditorBody,
            }

        ],
        columnDefs: [{
            orderable: false, targets: [0], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "ID", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
            { data: "SUBINVENTORY_CODE", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
            { data: "SEGMENT3", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
            { data: "ITEM_NO", "name": "料號", "autoWidth": true, "className": "dt-body-center" },
            { data: "BARCODE", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "PRIMARY_AVAILABLE_QTY", "name": "數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PRIMARY_UOM_CODE", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "SECONDARY_AVAILABLE_QTY", "name": "數量", "autoWidth": true, "className": "dt-body-right" },
            { data: "SECONDARY_UOM_CODE", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "NOTE", "name": "備註", "autoWidth": true, "className": "dt-body-center" },
        ],

    });

}


function dispalyText() {
    $('#ID').val();
    $('#Subinventory').val();
    $('#Locator').val();
    $('#ItemNo').val();
    $('#Barcode').val();
}


function clearText() {
    $('#ID').text("");
    $('#Subinventory').text("");
    $('#Locator').text("");
    $('#ItemNo').text("");
    $('#Barcode').text("");
    $('#Unit').text("");
}