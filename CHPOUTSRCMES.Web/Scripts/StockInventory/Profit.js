﻿var selected = [];
var LossDetailDT;
var ProfitDetailDT;
var editor;

function ProfitInit() {
    GetProfitTop();
    ProfitOnClick();
    ProfitLoadLossDetailDT();
    ProfitLoadProfitDetailDT();
    ProfitOnkey();
};


function GetProfitTop() {
    $("#txtItemNumberArea").toggleClass('border-0')
    $.ajax({
        url: "/StockTransaction/GetTop",
        type: "GET",
        dataType: 'html',
        data: {},
        success: function (data) {
            $('#Top').empty();
            $('#Top').html(data);
            ProfitTopInit();
        },
        error: function () {
            swal.fire('更新倉庫搜尋頁面失敗');
        },
        complete: function (data) {


        }

    })
}


function ProfitTopInit() {

    $('#ddlSubinventory').change(function () {

        var SUBINVENTORY_CODE = $("#ddlSubinventory").val();
        $.ajax({
            url: "/StockTransaction/GetLocatorListForUserId",
            type: "post",
            data: {
                SUBINVENTORY_CODE: SUBINVENTORY_CODE
            },
            success: function (data) {
                $('#ddlLocator').empty();
                for (var i = 0; i < data.length; i++) {
                    $('#ddlLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }
                //GetItemNumberList();
                if (data.length == 1) {
                    $('#ddlLocatorArea').hide();
                } else {
                    $('#ddlLocatorArea').show();
                }

            },
            error: function () {
                swal.fire('更新儲位失敗');
            },
            complete: function (data) {


            }

        })


    })

    $('#ddlLocator').change(function () {

    })

    $("#txtItemNumber").autocomplete({
        autoFocus: true,
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/AutoCompleteItemNumber",
                type: "POST",
                dataType: "json",
                data: {
                    //SubinventoryCode: $("#ddlSubinventory").val(),
                    //Locator: $("#ddlLocator").val(),
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data.slice(0, 20), function (item) {
                        return { label: item.Description, value: item.Value };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            if (ui.item) {
                $('#txtItemNumber').val(ui.item.value);
                ProfitAutoCompleteItemNumberSelectCallBack(ui.item.value);

            }
        }
    });

    function ProfitAutoCompleteItemNumberSelectCallBack(ITEM_NO) {
        $('#btnSearchLoss').focus();
        //$("#txtItemNumber").val(ITEM_NO);
        //SearchLoss();
    }

    //$('#txtItemNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        $('#btnSearchStock').focus();
    //        //$(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
    //        //AutoCompleteItemNumberEnterCallBack();
    //    }
    //});
}

function ProfitLoadLossDetailDT() {
    LossDetailDT = $('#LossDetailDT').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        //pageLength: 2,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockInventory/GetLossDetailForProfit",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.SubinventoryCode = $('#ddlSubinventory').val();
                d.Locator = $('#ddlLocator').val();
                d.ItemNumber = $('#txtItemNumber').val();
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "STOCK_ID", name: "項次", autoWidth: true },
            { data: "SUBINVENTORY_CODE", name: "倉庫", autoWidth: true },
            { data: "SEGMENT3", name: "儲位", autoWidth: true },
            { data: "ITEM_NO", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "BARCODE", name: "條碼", autoWidth: true },
            { data: "PRIMARY_TRANSACTION_QTY", name: "主要異動數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_AVAILABLE_QTY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_UOM_CODE", name: "主要單位", autoWidth: true },
             {
                 data: "SECONDARY_TRANSACTION_QTY", name: "次要異動數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
                     if (data != null) {
                         if (data == 0) {
                             return '';
                         }
                         return data;
                     } else {
                         return '';
                     }
                 }
             },
            {
                data: "SECONDARY_AVAILABLE_QTY", name: "次要數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
                    if (data != null) {
                        if (data == 0) {
                            return '';
                        }
                        return data;
                    } else {
                        return '';
                    }
                }
            },
            { data: "SECONDARY_UOM_CODE", name: "次要單位", autoWidth: true },
            { data: "NOTE", name: "備註", autoWidth: true },
            { data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }
        ],

        order: [[13, 'desc']],
        select: {
            style: 'single'
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                }
            },
            buttons: [
                'selectNone'
            ],
        }
    });

    LossDetailDT.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var StockId = dt.rows(indexes).data().pluck('STOCK_ID')[0];
            $("#StockId").text(StockId);
            var Subinventory = dt.rows(indexes).data().pluck('SUBINVENTORY_CODE')[0];
            $("#Subinventory").text(Subinventory);
            var Locator = dt.rows(indexes).data().pluck('SEGMENT3')[0];
            $("#Locator").text(Locator);
            var ItemNumber = dt.rows(indexes).data().pluck('ITEM_NO')[0];
            $("#ItemNumber").text(ItemNumber);
            var Barcode = dt.rows(indexes).data().pluck('BARCODE')[0];
            $('#Barcode').text(Barcode);
            var ITEM_CATEGORY = dt.rows(indexes).data().pluck('ITEM_CATEGORY')[0];
            if (ITEM_CATEGORY == "平版") {
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                $('#TransactionUnit').text(Unit);
                $('#txtQty').attr('disabled', false);
            } else if (ITEM_CATEGORY == "捲筒") {
                $('#txtQty').attr('disabled', true);
                var qty = dt.rows(indexes).data().pluck('PRIMARY_TRANSACTION_QTY')[0] * -1;
                $('#txtQty').val(qty);
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                $('#TransactionUnit').text(Unit);

            } else {
                $('#TransactionUnit').text("");
            }

            var rowsData = StockDT.rows({ page: 'current' }).data();
            for (i = 0 ; i < rowsData.length; i++) {
                for (j = 0; j < selected.length; j++) {
                    if (selected[j] == rowsData[i].ID) {
                        selected.splice(j, 1);
                    }
                }
            }

            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            if (index === -1) {
                selected.push(ID);
            }

            $('#txtQty').focus().select();
        }
    });

    LossDetailDT.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#StockId").text("");
            $("#Subinventory").text("");
            $("#Locator").text("");
            $("#ItemNumber").text("");
            $("#Barcode").text("");
            $('#TransactionUnit').text("");

            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            selected.splice(index, 1);
        }
    });
}

function ProfitLoadProfitDetailDT() {
    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockInventory/ProfitEditor',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: function (d) {
                var StockInventoryDTList = [];
                $.each(d.data, function (key, value) {
                    var StockInventoryDT = {
                        'ID': d.data[key]['ID'],
                        'NOTE': d.data[key]['NOTE'],
                        'PRIMARY_AVAILABLE_QTY': d.data[key]['PRIMARY_AVAILABLE_QTY'],
                        'PRIMARY_TRANSACTION_QTY': d.data[key]['PRIMARY_TRANSACTION_QTY'],
                        'SECONDARY_TRANSACTION_QTY': d.data[key]['SECONDARY_TRANSACTION_QTY'],
                        'LOT_NUMBER': d.data[key]['LOT_NUMBER'],
                        'ITEM_NO': d.data[key]['ITEM_NO'],
                        'SEGMENT3': d.data[key]['SEGMENT3'],
                        'SUBINVENTORY_CODE': d.data[key]['SUBINVENTORY_CODE']
                    }
                    StockInventoryDTList.push(StockInventoryDT);
                });

                var data = {
                    'action': d.action,
                    'StockInventoryDTList': StockInventoryDTList
                }
                return JSON.stringify(data);
            }
            //success: function () {
            //    LoadBody();
            //}
        },
        table: "#ProfitDetailDT",
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
                lable: "儲位ID",
                name: "LOCATOR_ID"
            },
            {
                label: "料號",
                name: "ITEM_NO"
            },
            {
                label: "捲號",
                name: "LOT_NUMBER"
            },
            {
                label: "異動量(KG)",
                name: "PRIMARY_TRANSACTION_QTY"
            },
             {
                 label: "異動量(RE)",
                 name: "SECONDARY_TRANSACTION_QTY"
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


    editor.hide(['ID', 'SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO', 'PRIMARY_TRANSACTION_QTY', 'LOT_NUMBER', 'LOCATOR_ID', 'SECONDARY_TRANSACTION_QTY']);

    var SubinventoryCode = $('#ddlSubinventory').val();
    var Locator = $('#ddlLocator').val();
    var ItemNumber = $('#txtItemNumber').val();

    ProfitDetailDT = $('#ProfitDetailDT').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        //destroy: true,
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockInventory/GetProfitDetail",
            "type": "POST",
            "datatype": "json",
            "data": {},
        },
        select: {
            style: 'multi',
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                }
            },
            buttons: [
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
                editor: editor,
            }
            //,
            //{
            //    extend: 'edit',
            //    text: '編輯備註',
            //    name: 'edit',
            //    className: 'btn-danger',
            //    editor: editor,
            //}
            ,
            {
                text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                //className: 'btn-default btn-sm',
                action: function (e) {
                    PrintLable(ProfitDetailDT, "/Home/GetLabels3", "5");
                },
                className: "btn-primary"
            }
            ,
            {
                extend: 'edit',
                text: '編輯備註',
                className: 'btn-danger',
                name: 'edit',
                action: function (e, dt, node, config) {
                    var count = dt.rows({ selected: true }).count();
                    if (count == 0) {
                        return;
                    }

                    editor.edit({
                        title: '編輯備註',
                        buttons: '確定',
                        action: function () {
                            this.submit();
                        },
                        className: 'btn-danger'
                    });
                    editor.hide(['ID', 'SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO', 'PRIMARY_TRANSACTION_QTY', 'LOT_NUMBER', 'LOCATOR_ID', 'SECONDARY_TRANSACTION_QTY']);

                }
            }
            ]
        },

        columns: [
             { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
             { data: "STOCK_ID", name: "項次", autoWidth: true },
             { data: "SUBINVENTORY_CODE", name: "倉庫", autoWidth: true },
             { data: "SEGMENT3", name: "儲位", autoWidth: true },
             { data: "ITEM_NO", name: "料號", autoWidth: true, className: "dt-body-left" },
             { data: "BARCODE", name: "條碼", autoWidth: true },
             { data: "PRIMARY_TRANSACTION_QTY", name: "主要異動數量", autoWidth: true, className: "dt-body-right" },
             { data: "PRIMARY_AVAILABLE_QTY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
             { data: "PRIMARY_UOM_CODE", name: "主要單位", autoWidth: true },
              {
                  data: "SECONDARY_TRANSACTION_QTY", name: "次要異動數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
                      if (data != null) {
                          if (data == 0) {
                              return '';
                          }
                          return data;
                      } else {
                          return '';
                      }
                  }
              },
             {
                 data: "SECONDARY_AVAILABLE_QTY", name: "次要數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
                     if (data != null) {
                         if (data == 0) {
                             return '';
                         }
                         return data;
                     } else {
                         return '';
                     }
                 }
             },
             { data: "SECONDARY_UOM_CODE", name: "次要單位", autoWidth: true },
             { data: "NOTE", name: "備註", autoWidth: true },
             { data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }
        ],

    });



}



function ProfitOnkey() {
    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#txtQty").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#btnAddRecord").click();
        }
    });
}

function ProfitOnClick() {

    $("#btnSearchLoss").click(function () {
        SearchLoss();
    });

    $('#btnAddRecord').click(function () {
        AddProfitDetail();
    });

    $('#btnSaveTransaction').click(function () {
        SaveProfitDetail();
    });


    $('#btnCreateBarcode').click(function (e) {
        e.preventDefault();
        var SEGMENT3 = $("#ddlLocator option:selected").text();
        var SUBINVENTORY_CODE = $('#ddlSubinventory').val();
        var LOCATOR_ID = $('#ddlLocator').val();
        var ITEM_NO = $('#txtItemNumber').val();

        if (SUBINVENTORY_CODE == "請選擇") {
            swal.fire("請先選擇倉庫");
            return;
        }

        if ($('#ddlLocatorArea').is(":visible")) {
            if (LOCATOR_ID == "請選擇") {
                swal.fire('請選擇儲位');
                event.preventDefault();
                return;
            }
        }

        if (ITEM_NO.length == 0) {
            swal.fire("請先輸入料號");
            return;
        }

        CreateBarcode(SUBINVENTORY_CODE, SEGMENT3, ITEM_NO, LOCATOR_ID);

        //editor.create({
        //    title: '新增條碼',
        //    buttons: '確定'
        //});
        //var ITEM_CATEGORY = GetITEM_CATEGORY(SubinventoryCode, ddlItemNo);
        //if (ITEM_CATEGORY == "平版")
        //{
        //    editor.show(['SUBINVENTORY_CODE', 'SEGMENT3', 'PRIMARY_AVAILABLE_QTY', 'ITEM_NO']);
        //    editor.disable(['SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO']);
        //    editor.field('SUBINVENTORY_CODE').val(SubinventoryCode);
        //    editor.field('SEGMENT3').val(ddlLocator);
        //    editor.field('ITEM_NO').val(ddlItemNo);
        //} else if (ITEM_CATEGORY == "捲筒") {
        //    editor.show(['SUBINVENTORY_CODE', 'SEGMENT3', 'LOT_NUMBER', 'PRIMARY_AVAILABLE_QTY', 'ITEM_NO']);
        //    editor.disable(['SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO']);
        //    editor.field('SUBINVENTORY_CODE').val(SubinventoryCode);
        //    editor.field('SEGMENT3').val(ddlLocator);
        //    editor.field('ITEM_NO').val(ddlItemNo);
        //}

    });

}

function CreateBarcode(SUBINVENTORY_CODE, SEGMENT3, ITEM_NO, LOCATOR_ID) {
    var ITEM_CATEGORY;
    if (LOCATOR_ID == "請選擇") {
        LOCATOR_ID = 0;
    }
    if (SEGMENT3 == "請選擇") {
        SEGMENT3 = "";
    }
    $.ajax({
        url: "/StockTransaction/GetStockItemData",
        type: "post",
        data: {
            //SUBINVENTORY_CODE: SUBINVENTORY_CODE,
            ITEM_NO: ITEM_NO
        },
        success: function (data) {
            if (data.Success) {
                ITEM_CATEGORY = data.Data.CatalogElemVal070;
            } else {
                swal.fire(data.Msg);
                ITEM_CATEGORY = "";
            }
        },
        error: function () {
            swal.fire('取得料號資料失敗');
        },
        complete: function (data) {
            if (ITEM_CATEGORY == "") {
                return;
            }

            editor.create({
                title: '新增條碼',
                buttons: '確定'
            });
            editor.hide(['ID']);
            if (ITEM_CATEGORY == "平版") {
                editor.show(['SUBINVENTORY_CODE', 'SEGMENT3', 'SECONDARY_TRANSACTION_QTY', 'ITEM_NO']);
                editor.disable(['PRIMARY_TRANSACTION_QTY', 'SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO']);
                editor.field('SUBINVENTORY_CODE').val(SUBINVENTORY_CODE);
                editor.field('SEGMENT3').val(SEGMENT3);
                editor.field('LOCATOR_ID').val(LOCATOR_ID);
                editor.field('ITEM_NO').val(ITEM_NO);
            } else if (ITEM_CATEGORY == "捲筒") {
                editor.show(['SUBINVENTORY_CODE', 'SEGMENT3', 'LOT_NUMBER', 'PRIMARY_TRANSACTION_QTY', 'ITEM_NO']);
                editor.enable(['PRIMARY_TRANSACTION_QTY']);
                editor.hide(['SECONDARY_TRANSACTION_QTY']);
                editor.disable(['SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO', 'SECONDARY_TRANSACTION_QTY']);
                editor.field('SUBINVENTORY_CODE').val(SUBINVENTORY_CODE);
                editor.field('LOCATOR_ID').val(LOCATOR_ID);
                editor.field('SEGMENT3').val(SEGMENT3);
                editor.field('ITEM_NO').val(ITEM_NO);
            }
        }

    });
}

function SearchLoss() {
    if ($('#ddlSubinventory').val() == "請選擇") {
        swal.fire('請選擇倉庫');
        event.preventDefault();
        return;
    }
    if ($('#ddlLocatorArea').is(":visible")) {
        if ($('#ddlLocator').val() == "請選擇") {
            swal.fire('請選擇儲位');
            event.preventDefault();
            return;
        }
    }
    if ($('#txtItemNumber').val() == "") {
        swal.fire('請輸入料號');
        event.preventDefault();
        return;
    }

    LossDetailDT.ajax.reload(null, false);
}





function AddProfitDetail() {
    var ID = $('#StockId').text();
    if (ID == "") {
        swal.fire('請選擇料號');
        event.preventDefault();
        return false;
    }


    var Qty = $('#txtQty').val();
    if (Qty == "") {
        swal.fire('請輸入數量');
        event.preventDefault();
        return false;
    }



    $.ajax({
        url: "/StockInventory/AddProfitDetail",
        type: "post",
        data: {
            ID: ID,
            Qty: Qty
        },
        success: function (data) {
            if (data.status) {
                ProfitDetailDT.ajax.reload();
            } else {
                swal.fire(data.result);
            }
        },
        error: function () {
            swal.fire('新增異動明細失敗');
        },
        complete: function (data) {


        }

    });
}

function SaveProfitDetail() {
    var count = ProfitDetailDT.rows().count();
    if (count == 0) {
        swal.fire('請輸入異動明細');
        event.preventDefault();
        return;
    }


    swal.fire({
        title: "異動存檔",
        text: "確定存檔嗎?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "確定",
        cancelButtonText: "取消"
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: "/StockInventory/SaveProfitDetail",
                type: "post",
                data: {},
                success: function (data) {
                    if (data.status) {
                        swal.fire(data.result);
                        ProfitDetailDT.ajax.reload();
                    } else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('異動存檔失敗');
                },
                complete: function (data) {


                }

            });
        }
    });


}