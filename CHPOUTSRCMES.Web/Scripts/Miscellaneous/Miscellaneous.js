var selected = [];
var editor;

$(document).ready(function () {

    GetTop();

    function isNotANumber(inputData) {
        if (!inputData) {
            return false;
        }
        if (isNaN(inputData)) {
            return false;
        } else {
            return true;
        }
    }


    function getMiscellaneous() {
        return $("#ddlMiscellaneous option:selected").text();
    }

    function getTransactionTypeId() {
        var id = $("#ddlMiscellaneous").val();
        if (id == '請選擇') {
            id = '0';
        }
        return id;
    }

    function getOrganizationId() {
        var id = $("#ddlSubinventory").val();
        if (id == '請選擇') {
            id = '0';
        }
        return id;
    }

    function getSubinventoryCode() {
        return $("#ddlSubinventory option:selected").text();
    }

    function getLocatorId() {
        if ($('#ddlLocatorArea').is(":visible")) {
            return $("#ddlLocator").val();
        } else {
            return null;
        }
    }

    function getSearchQty() {
        var qty = $("#txtSearchQty").val();
        if (!qty) {
            qty = 0;
        }
        return qty;
    }

    function getPercentageError() {
        var percentageError = $("#txtPercentageError").val();
        if (!percentageError) {
            percentageError = 0;
        }
        return percentageError;
    }

    $('#txtSearchQty').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#txtPercentageError').focus().select();
        }
    });

    $('#txtPercentageError').keydown(function (e) {
        if (e.keyCode == 13) {
            $("#btnSearchStock").focus();
            return false;
            //SearchStock();
        }
    });

    $("#btnSearchStock").click(function () {
        SearchStock();
    });

    $("#btnAddRecord").click(function () {
        AddTransactionDetail();
    });

    $('#txtQty').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#txtNote').focus().select();
        }
    });

    $('#txtNote').keydown(function (e) {
        if (e.keyCode == 13) {
            AddTransactionDetail();
        }
    })


    $("#btnSaveTransaction").click(function () {
        SaveTransactionDetail();
    });


    $('#ddlMiscellaneous').change(function () {
        if (getMiscellaneous() == "雜發") {
            $("#qtyType").html("減少數量");
        } else if (getMiscellaneous() == "雜收") {
            $("#qtyType").html("增加數量");
        } else {
            $("#qtyType").html("數量");
        }
        TransactionDetailDT.ajax.reload();
        //var Miscellaneous = $("#ddlMiscellaneous").val();
        //if (Miscellaneous == "請選擇") {
        //    $('#Content').empty();
        //    return;
        //}

        //$.ajax({
        //    url: "/Miscellaneous/GetContent",
        //    type: "GET",
        //    dataType: 'html',
        //    data: {
        //        Miscellaneous: Miscellaneous
        //    },
        //    success: function (data) {
        //        $('#Content').empty();
        //        $('#Content').html(data);

        //        if (Miscellaneous == "雜發") {  
        //            SendInit();
        //        } else {
        //            ReceiveInit();
        //        }
        //    },
        //    error: function () {
        //        swal.fire('更新內容失敗');
        //    },
        //    complete: function (data) {


        //    }

        //})


    })


    var StockDT = $('#StockDT').DataTable({
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
            "url": "/Miscellaneous/SearchStock",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.organizationId = getOrganizationId();
                d.subinventoryCode = getSubinventoryCode();
                d.locatorId = getLocatorId();
                d.itemNumber = $("#txtItemNumber").val();
                d.primaryQty = getSearchQty();
                d.percentageError = getPercentageError();
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
            var ORGANIZATION_ID = dt.rows(indexes).data().pluck('ORGANIZATION_ID')[0];
            $("#ORGANIZATION_ID").text(ORGANIZATION_ID);
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
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                $('#TransactionUnit').text(Unit);
            } else if (ITEM_CATEGORY == "捲筒") {
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                $('#TransactionUnit').text(Unit);
            } else {
                $('#TransactionUnit').text("");
            }

            var rowsData = StockDT.rows({ page: 'current' }).data();
            for (i = 0; i < rowsData.length; i++) {
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
            $("#ORGANIZATION_ID").text("");
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

    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Miscellaneous/DetailEditor',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var TData = d.data;
                var StockMiscellaneousDTList = [];
                var size = Object.keys(TData).length;
                for (var i = 0; i < size; i++) {
                    var ID = Object.keys(TData)[i];
                    //var NOTE = Object.values(TData[ID])[0];
                    var NOTE = Object.keys(TData[ID]).map(function (e) {
                        return TData[ID][e]
                    })[0];

                    var StockMiscellaneousDT = {
                        'ID': ID,
                        'NOTE': NOTE
                    }
                    StockMiscellaneousDTList.push(StockMiscellaneousDT);
                }
                var data = {
                    'action': d.action,
                    'StockMiscellaneousDTList': StockMiscellaneousDTList
                }
                return JSON.stringify(data);
            },
            success: function (data) {
                if (data.status) {
                    TransactionDetailDT.ajax.reload();
                }
                else {
                    swal.fire(data.result);
                }
            }
        },
        table: "#TransactionDetailDT",
        idSrc: 'ID',
        fields: [
            {
                label: "備註:",
                name: "NOTE",
                type: 'text'
            }
        ],
        i18n: {
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

    var TransactionDetailDT = $('#TransactionDetailDT').DataTable({
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
            "url": "/Miscellaneous/GetTransactionDetail",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.transactionTypeId = getTransactionTypeId();
            }
        },
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

        order: [[1, 'desc']],
        select: {
            style: 'multi'
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                }
            },
            buttons: [
                'selectAll',
                'selectNone',
                {
                    extend: "remove",
                    text: '刪除',
                    name: 'remove',
                    className: 'btn-danger',
                    editor: editor,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        var rows = TransactionDetailDT.rows({ selected: true }).indexes();

                        if (rows.length === 0) {
                            return;
                        }

                        editor.remove(rows, {
                            title: '刪除',
                            message: rows.length === 1 ?
                                '你確定要刪除這筆資料?' :
                                '你確定要刪除這些資料?',
                            buttons:
                            {
                                text: '刪除',
                                className: 'btn-danger',
                                action: function () {
                                    this.submit();
                                }
                            }
                        })
                    }
                },
                //{
                //    text: '刪除',
                //    className: 'btn-danger',
                //    action: function () {
                //        var selectedData = TransactionDetailDT.rows('.selected').data();
                //        if (selectedData.length == 0) {
                //            swal.fire("請選擇要刪除的項目");
                //            return;
                //        }

                //        swal.fire({
                //            title: "異動明細刪除",
                //            text: "確定刪除嗎?",
                //            type: "warning",
                //            showCancelButton: true,
                //            confirmButtonColor: "#DD6B55",
                //            confirmButtonText: "確定",
                //            cancelButtonText: "取消"
                //        }).then(function (result) {
                //            if (result.value) {
                //                DelTransactionDetail(selectedData);
                //            }
                //        });

                //    }
                //},
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

                        editor.edit(TransactionDetailDT.rows({ selected: true }).indexes())
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
                //    text: '編輯備註',
                //    className: 'btn-danger',
                //    action: function (e, dt, node, config) {
                //        var count = dt.rows({ selected: true }).count();

                //        if (count == 0) {
                //            return;
                //        }

                //        editor.edit(TransactionDetailDT.rows({ selected: true }).indexes())
                //        .title('編輯備註')
                //        .buttons({
                //            text: '確定',
                //            action: function () {
                //                this.submit();
                //            },
                //            className: 'btn-danger'
                //        });
                //    }
                //}
            ],
        }
    });

    function SearchStock() {
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
        if ($('#txtSearchQty').val() == "") {
            swal.fire('請輸入數量');
            event.preventDefault();
            return;
        } else {
            if (!isNotANumber($('#txtSearchQty').val())) {
                swal.fire('數量須為數字');
                event.preventDefault();
                return;
            }
        }
        if ($('#txtPercentageError').val() == "") {
            swal.fire('請輸入誤差百分比');
            event.preventDefault();
            return;
        } else {
            if (!isNotANumber($('#txtPercentageError').val())) {
                swal.fire('誤差百分比須為數字');
                event.preventDefault();
                return;
            }
        }


        StockDT.ajax.reload(null, false);
    }

    function AddTransactionDetail() {
        var stockId = $('#StockId').text();
        if (stockId == "") {
            swal.fire('請選擇庫存');
            event.preventDefault();
            return false;
        }

        var PrimaryQty = $('#txtQty').val();
        if (PrimaryQty == "") {
            swal.fire('請輸入增減數量');
            event.preventDefault();
            return false;
        }

        var transactionTypeId = $('#ddlMiscellaneous').val();
        if (transactionTypeId == "請選擇") {
            swal.fire('請選擇雜項異動類別');
            event.preventDefault();
            return false;
        }

        var Note = $('#txtNote').val();
        if (Note == "") {
            swal.fire('請輸入備註');
            event.preventDefault();
            return false;
        }


        $.ajax({
            url: "/Miscellaneous/AddTransactionDetail",
            type: "post",
            data: {
                transactionTypeId: transactionTypeId,
                stockId: stockId,
                mPrimaryQty: PrimaryQty,
                note: Note
            },
            success: function (data) {
                if (data.status) {
                    TransactionDetailDT.ajax.reload();
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

    function DelTransactionDetail(selectedData) {


        var list = [];
        for (i = 0; i < selectedData.length; i++) {
            list.push(selectedData[i].ID);
        }


        $.ajax({
            url: "/Miscellaneous/DelTransactionDetail",
            type: "post",
            data: {
                IDs: list
            },
            success: function (data) {
                if (data.status) {
                    TransactionDetailDT.ajax.reload();
                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('刪除異動明細失敗');
            },
            complete: function (data) {


            }

        });
    }

    function SaveTransactionDetail() {
        var transactionTypeId = $('#ddlMiscellaneous').val();
        if (transactionTypeId == "請選擇") {
            swal.fire('請選擇雜項異動類別');
            event.preventDefault();
            return false;
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
                    url: "/Miscellaneous/SaveTransactionDetail",
                    type: "post",
                    data: {
                        transactionTypeId: transactionTypeId
                    },
                    success: function (data) {
                        if (data.status) {
                            TransactionDetailDT.ajax.reload();
                            swal.fire(data.result);
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
});



function SubinventoryChangeCallBack() {

}


function LocatorChangeCallBack() {

}

//function AutoCompleteItemNumberEnterCallBack() {
//    $('#txtSearchQty').focus().select();
//}

function AutoCompleteItemNumberSelectCallBack(ITEM_NO) {
    //$("#txtItemNumber").val(ITEM_NO);

    GetStockItemData(ITEM_NO, true);
}

function LostFocusItemNumberCallBack(ITEM_NO) {

    GetStockItemData(ITEM_NO, false);

}

function GetStockItemData(ITEM_NO, focusNext) {
    $.ajax({
        url: "/StockTransaction/GetStockItemData",
        type: "post",
        data: {
            SUBINVENTORY_CODE: $("#ddlSubinventory option:selected").text(),
            ITEM_NO: ITEM_NO
        },
        success: function (data) {
            if (data.Success) {
                $('#SearchUnit').html(data.Data.PrimaryUomCode);
                if (focusNext) {
                    $('#txtSearchQty').focus().select();
                }
            } else {
                $('#SearchUnit').html("");
            }
        },
        error: function () {
            swal.fire('取得料號資訊失敗');
        },
        complete: function (data) {

        }

    });
}