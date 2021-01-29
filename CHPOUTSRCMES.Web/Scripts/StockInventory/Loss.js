var StockDT;
var LossDetailDT;
var editor;
var Pay;   //0盤盈 1盤虧
var selected = [];

function getInventoryType() {
    return $("#ddlProfit option:selected").text();
}

function getTransactionTypeId() {
    var id = $("#ddlProfit").val();
    if (id == '請選擇') {
        id = '0';
    }
    return id;
}

function getLossOrganizationId() {
    var id = $("#ddlSubinventory").val();
    if (id == '請選擇') {
        id = '0';
    }
    return id;
}

function getLossSubinventoryCode() {
    return $("#ddlSubinventory option:selected").text();
}

function getLossLocatorId() {
    if ($('#ddlLocatorArea').is(":visible")) {
        return $("#ddlLocator").val();
    } else {
        return null;
    }
    //if ($('#ddlLocator option').length === 1) {
    //    return null;
    //} else {
    //    return $("#ddlLocator").val();
    //}
}


function LossInit() {
    LossTopInit();
    LossOnClick();
    LossLoadStockDT();
    LossLoadLossDetailDT();
    //event();
    LossOnkey();
};

//function GetLossTop() {
//    $("#txtItemNumberArea").toggleClass('border-0')
//    $.ajax({
//        url: "/StockTransaction/GetTop",
//        type: "GET",
//        dataType: 'html',
//        data: {},
//        success: function (data) {
//            $('#Top').empty();
//            $('#Top').html(data);
//            LossTopInit();
//        },
//        error: function () {
//            swal.fire('更新倉庫搜尋頁面失敗');
//        },
//        complete: function (data) {


//        }

//    })
//}


function LossTopInit() {

    $('#ddlSubinventory').change(function () {

        var SUBINVENTORY_CODE = $("#ddlSubinventory option:selected").text();
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
                    $('#ddlLocator').hide();
                } else {
                    $('#ddlLocatorArea').show();
                    $('#ddlLocator').show();
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
                    response($.map(data, function (item) {
                        return { label: item.Value + " " + item.Description, value: item.Value };
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
                LossAutoCompleteItemNumberSelectCallBack(ui.item.value);
            }
        }
    });

    function LossAutoCompleteItemNumberSelectCallBack(ITEM_NO) {
        $('#btnSearchStock').focus();
        //$("#txtItemNumber").val(ITEM_NO);
        //SearchStock();
    }

    //$('#txtItemNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        //$('#btnSearchStock').focus();
    //        //SearchStock();
    //        //$(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
    //        //AutoCompleteItemNumberEnterCallBack();
    //    }
    //});

}



function LossLoadStockDT() {
    StockDT = $('#StockDT').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        //pageLength: 2,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockInventory/SearchStock",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.organizationId = getLossOrganizationId();
                d.subinventoryCode = getLossSubinventoryCode();
                d.locatorId = getLossLocatorId();
                d.itemNumber = $("#txtItemNumber").val();
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            { data: "SUBINVENTORY_CODE", name: "倉庫", autoWidth: true },
            { data: "SEGMENT3", name: "儲位", autoWidth: true },
            { data: "ITEM_NO", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "BARCODE", name: "條碼", autoWidth: true },
            { data: "PRIMARY_AVAILABLE_QTY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_UOM_CODE", name: "主要單位", autoWidth: true },
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
            { data: "ID", name: "STOCK_ID", autoWidth: true, visible: false }
            //{ data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }
        ],

        order: [[1, 'desc']],
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

    StockDT.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var StockId = dt.rows(indexes).data().pluck('ID')[0];
            $("#StockId").text(StockId);
            var SUB_ID = dt.rows(indexes).data().pluck('SUB_ID')[0];
            $("#SUB_ID").text(SUB_ID);
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
                var Unit = dt.rows(indexes).data().pluck('SECONDARY_UOM_CODE')[0];
                $('#TransactionUnit').text(Unit);
                //$('#txtQty').attr('disabled', false);
            } else if (ITEM_CATEGORY == "捲筒") {
                $('#txtQty').val(dt.rows(indexes).data().pluck('PRIMARY_AVAILABLE_QTY')[0]);
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                //$('#txtQty').attr('disabled', true);
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

    StockDT.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#StockId").text("");
            $("#SUB_ID").text("");
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

function LossOnkey() {
    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#txtQty").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#btnAddRecord").click();
        }
    });
}


function LossOnClick() {
    $('#btnSearchStock').click(function () {
        SearchStock();
    });

    $('#btnAddRecord').click(function () {
        AddLossDetail();
    });

    $('#btnSaveTransaction').click(function () {
        SaveLossDetail();
    });

}







function LossLoadLossDetailDT() {
    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockInventory/DetailEditor',
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
            },
            success: function (data) {
                if (data.status) {
                    LossDetailDT.ajax.reload();
                }
                else {
                    swal.fire(data.result);
                }
            }
        },
        table: "#LossDetailDT",
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
                name: "LOT_NUMBER"
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
            //create: {
            //    button: "新增",
            //    title: "新增",
            //    submit: "確定",
            //    action: 'btn-primary'
            //},
            //remove: {
            //    button: '刪除',
            //    title: "刪除",
            //    submit: "確定",
            //    confirm: {
            //        "_": "你確定要刪除這筆資料?",
            //        "1": "你確定要刪除這筆資料?"
            //    }
            //},
            //edit: {
            //    button: '編輯',
            //    title: "編輯",
            //    submit: "確定",
            //}
            edit: {
                button: "編輯備註",
                title: "編輯備註",
                submit: "確定",
                'className': 'btn-danger'
            },
            remove: {
                button: "刪除",
                title: "確定要刪除??",
                submit: "確定",
                confirm: {
                    "_": "你確定要刪除這筆資料?",
                    "1": "你確定要刪除這些資料?"
                }
            },
            multi: {
                "title": "多欄位異動",
                "info": "請注意，您一次選擇多個不同的備註，此次異動將會變成同樣的備註！",
                "restore": "取消更改",
                "noMulti": "This input can be edited individually, but not part of a group."
            }
        }
    });


    editor.hide(['ID', 'SUBINVENTORY_CODE', 'SEGMENT3', 'ITEM_NO', 'PRIMARY_AVAILABLE_QTY', 'LOT_NUMBER'])


    LossDetailDT = $('#LossDetailDT').DataTable({
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
            "url": "/StockInventory/GetTransactionDetail",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.transactionTypeId = getTransactionTypeId();
                d.fromHistoryData = false;
            }
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
                    //extend: "remove",
                    text: '刪除',
                    //name: 'remove',
                    className: 'btn-danger',
                    //editor: editor,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        var count = dt.rows({ selected: true }).count();

                        if (count == 0) {
                            return;
                        }

                        editor.remove(LossDetailDT.rows({ selected: true }).indexes())
                            .title('刪除')
                            .message(count === 1 ?
                                '你確定要刪除這筆資料?' :
                                '你確定要刪除這些資料?')
                            .buttons({
                                text: '刪除',
                                action: function () {
                                    this.submit();
                                },
                                className: 'btn-danger'
                            });

                        //editor.remove(rows, {
                        //    title: '刪除',
                        //    message: rows.length === 1 ?
                        //        '你確定要刪除這筆資料?' :
                        //        '你確定要刪除這些資料?',
                        //    buttons:
                        //    {
                        //        text: '刪除',
                        //        className: 'btn-danger',
                        //        action: function () {
                        //            this.submit();
                        //        }
                        //    }
                        //});
                    }
                },
                {
                    text: '編輯備註',
                    className: 'btn-danger',
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        var count = dt.rows({ selected: true }).count();

                        if (count == 0) {
                            return;
                        }

                        editor.edit(LossDetailDT.rows({ selected: true }).indexes())
                            .title('編輯備註')
                            .buttons({
                                text: '確定',
                                action: function () {
                                    this.submit();
                                },
                                className: 'btn-danger'
                            });
                    }
                }
            //{
            //    extend: "remove",
            //    text: '刪除',
            //    name: 'remove',
            //    className: 'btn-danger',
            //    editor: editor,
            //}
            //,
            //{
            //    extend: 'edit',
            //    text: '編輯備註',
            //    name: 'edit',
            //    className: 'btn-danger',
            //    editor: editor,
            //}
            ]
        },


        //buttons: [
        //    //{
        //    //    extend: "create",
        //    //    text: '新增條碼',
        //    //    name: 'create',
        //    //    className: 'btn-primary',
        //    //    editor: EditorBody,
        //    //}
        //    ,
        //    {
        //        extend: "selectAll",
        //    }
        //    ,
        //    {
        //        extend: "selectNone",
        //    }
        //    ,
        //    {
        //        extend: "remove",
        //        text: '刪除',
        //        name: 'remove',
        //        className: 'btn-danger',
        //        editor: editor,
        //    }
        //    ,
        //    {
        //        extend: 'edit',
        //        text: '編輯備註',
        //        name: 'edit',
        //        className: 'btn-danger',
        //        editor: editor,
        //    }

        //],
        columns: [
             { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
             { data: "SUB_ID", name: "項次", autoWidth: true },
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
            { data: "STOCK_ID", name: "STOCK_ID", autoWidth: true, visible: false }
             //{ data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }
        ],
        order: [[1, 'desc']]
    });



}

function SearchStock() {
    if ($('#ddlSubinventory').val() == "請選擇") {
        swal.fire('請選擇倉庫');
        event.preventDefault();
        return;
    }
    //if ($('#ddlLocator option').length > 1 && $('#ddlLocator').val() == "請選擇") {
    //    swal.fire('請選擇儲位');
    //    event.preventDefault();
    //    return;
    //}
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


    StockDT.ajax.reload(null, false);
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
    $('#TransactionUnit').text("");
}



function AddLossDetail() {
    var transactionTypeId = $('#ddlProfit').val();
    if (transactionTypeId == "請選擇") {
        swal.fire('請選擇盤點類別');
        event.preventDefault();
        return false;
    }

    var stockId = $('#StockId').text();
    if (stockId == "") {
        swal.fire('請選擇庫存');
        event.preventDefault();
        return false;
    }


    var mQty = $('#txtQty').val();
    if (mQty == "") {
        swal.fire('請輸入數量');
        event.preventDefault();
        return false;
    }


    ShowWait(function () {
        $.ajax({
            url: "/StockInventory/AddTransactionDetail",
            type: "post",
            data: {
                transactionTypeId: transactionTypeId,
                stockId: stockId,
                mQty: mQty
            },
            success: function (data) {
                if (data.status) {
                    CloseWait();
                    LossDetailDT.ajax.reload();
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
    });

    
}

function SaveLossDetail() {
    var transactionTypeId = $('#ddlProfit').val();
    if (transactionTypeId == "請選擇") {
        swal.fire('請選擇雜項異動類別');
        event.preventDefault();
        return false;
    }

    ShowWait(function () {
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
                    url: "/StockInventory/SaveTransactionDetail",
                    type: "post",
                    data: {
                        transactionTypeId: transactionTypeId
                    },
                    success: function (data) {
                        if (data.status) {
                            swal.fire(data.result);
                            LossDetailDT.ajax.reload();
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
    });

    


}
