var selected = [];
var editor;
var StockDT;

$(document).ready(function () {
    //$.fn.dataTable.Buttons.defaults.dom.button.className = 'btn'; //datatable button 預設的class btn btn-default 改為 btn
    //$.fn.dataTable.Editor.classes.form.button = 'btn'; //editor button 預設的class btn btn-default 改為 btn

    GetTop();

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
        //if ($('#ddlLocatorArea').is(":visible")) {
        //    return $("#ddlLocator").val();
        //} else {
        //    return null;
        //}
        if ($('#ddlLocator option').length === 1) {
            return null;
        } else {
            return $("#ddlLocator").val();
        }
    }

    $("#btnSearchStock").click(function () {
        SearchStock();
    });

    $("#btnAddRecord").click(function () {
        AddTransactionDetail();
    });

    $('#txtQty').keydown(function (e) {
        if (e.keyCode == 13) {
            AddTransactionDetail();
        }
    });

    $("#btnSaveTransaction").click(function () {
        SaveTransactionDetail();
    });


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
            "url": "/Obsolete/SearchStock",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.organizationId = getOrganizationId();
                d.subinventoryCode = getSubinventoryCode();
                d.locatorId = getLocatorId();
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
                $('#Unit').text(Unit);
            } else if (ITEM_CATEGORY == "捲筒") {
                var Unit = dt.rows(indexes).data().pluck('PRIMARY_UOM_CODE')[0];
                $('#Unit').text(Unit);
            } else {
                $('#Unit').text("");
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
            $('#Unit').text("");

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
            url: '/Obsolete/DetailEditor',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var TData = d.data;
                var StockObsoleteDTList = [];
                var size = Object.keys(TData).length;
                for (var i = 0; i < size; i++) {
                    var ID = Object.keys(TData)[i];
                    //var NOTE = Object.values(TData[ID])[0];
                    var NOTE = Object.keys(TData[ID]).map(function (e) {
                        return TData[ID][e]
                    })[0];

                    var StockObsoleteDT = {
                        'ID': ID,
                        'NOTE': NOTE
                    }
                    StockObsoleteDTList.push(StockObsoleteDT);
                }
                var data = {
                    'action': d.action,
                    'StockObsoleteDTList': StockObsoleteDTList
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
            "url": "/Obsolete/GetTransactionDetail",
            "type": "POST",
            "datatype": "json",
            "data": {}
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
            ],
        }
    });



    function AddTransactionDetail() {
        var stockId = $('#StockId').text();
        if (stockId == "") {
            swal.fire('請選擇庫存');
            event.preventDefault();
            return false;
        }

        var ObsoleteQty = $('#txtQty').val();
        if (ObsoleteQty == "") {
            swal.fire('請輸入報廢數量');
            event.preventDefault();
            return false;
        }

        ShowWait(function () {
            $.ajax({
                url: "/Obsolete/AddTransactionDetail",
                type: "post",
                data: {
                    stockId: stockId,
                    mQty: ObsoleteQty
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
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
        });

        
    }

    function DelTransactionDetail(selectedData) {


        var list = [];
        for (i = 0 ; i < selectedData.length; i++) {
            list.push(selectedData[i].ID);
        }


        $.ajax({
            url: "/Obsolete/DelTransactionDetail",
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
        var count = TransactionDetailDT.rows().count();
        if (count == 0) {
            swal.fire('請輸入異動明細');
            event.preventDefault();
            return;
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
                        url: "/Obsolete/SaveTransactionDetail",
                        type: "post",
                        data: {},
                        success: function (data) {
                            if (data.status) {
                                swal.fire(data.result);
                                TransactionDetailDT.ajax.reload();
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
});

function SearchStock() {
    if ($('#ddlSubinventory').val() == "請選擇") {
        swal.fire('請選擇倉庫');
        event.preventDefault();
        return;
    }
    if ($('#ddlLocator option').length > 1 && $('#ddlLocator').val() == "請選擇") {
        swal.fire('請選擇儲位');
        event.preventDefault();
        return;
    }
    //if ($('#ddlLocatorArea').is(":visible")) {
    //    if ($('#ddlLocator').val() == "請選擇") {
    //        swal.fire('請選擇儲位');
    //        event.preventDefault();
    //        return;
    //    }
    //}
    if ($('#txtItemNumber').val() == "") {
        swal.fire('請輸入料號');
        event.preventDefault();
        return;
    }


    StockDT.ajax.reload(null, false);
}

function SubinventoryChangeCallBack() {

}


function LocatorChangeCallBack() {

}

//function AutoCompleteItemNumberEnterCallBack() {
//    SearchStock();
//}

function AutoCompleteItemNumberSelectCallBack(ITEM_NO) {
    $("#btnSearchStock").focus();
    return false;
    //$("#txtItemNumber").val(ITEM_NO);
    //SearchStock();
}
