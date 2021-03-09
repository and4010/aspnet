var selected = [];

$(document).ready(function () {




    var PaperRollDataTablesBody = $('#PaperRollDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        "serverSide": true,
        processing: true,
        orderMulti: true,
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetRollEdit",
            "type": "POST",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text(),
                DELIVERY_STATUS_NAME: $("#DELIVERY_STATUS").text()
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            { data: "ORDER_NUMBER", name: "訂單", autoWidth: true },
            { data: "ORDER_SHIP_NUMBER", name: "訂單行號", autoWidth: true },
            { data: "OSP_BATCH_NO", name: "工單號碼", autoWidth: true },
            { data: "TMP_ITEM_NUMBER", name: "代紙料號", autoWidth: true, className: "dt-body-left" },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "PAPER_TYPE", name: "紙別", autoWidth: true },
            { data: "BASIC_WEIGHT", name: "基重", autoWidth: true },
            { data: "SPECIFICATION", name: "規格", autoWidth: true },
            { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right" },
            { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right" },
            { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },
            { data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", autoWidth: true, className: "dt-body-right" },
            { data: "SRC_PICKED_QUANTITY", name: "訂單已揀數量", autoWidth: true, className: "dt-body-right" },
            { data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", autoWidth: true },
        ],

        "order": [[1, 'asc']],
        select: {
            style: 'single',
        },
        buttons: [

            'selectNone'
        ],
        "rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {
            if ($.inArray(data.ID, selected) !== -1) {
                var selectRow = ':eq(' + dataIndex + ')';
                PaperRollDataTablesBody.row(selectRow, { page: 'current' }).select();
            }
        },
        "preDrawCallback": function (settings) {
            $("#SUB_ID").text("");
            $("#DLV_DETAIL_ID").text("");
            $("#ORDER_NUMBER").text("");
            $("#ORDER_SHIP_NUMBER").text("");
            $("#INVENTORY_ITEM_ID").text("");
            $("#ITEM_NUMBER").text("");
        }

    });


    PaperRollDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var SUB_ID = dt.rows(indexes).data().pluck('SUB_ID')[0];
            $("#SUB_ID").text(SUB_ID);
            var DLV_DETAIL_ID = dt.rows(indexes).data().pluck('ID')[0];
            $("#DLV_DETAIL_ID").text(DLV_DETAIL_ID);
            var ORDER_NUMBER = dt.rows(indexes).data().pluck('ORDER_NUMBER')[0];
            $("#ORDER_NUMBER").text(ORDER_NUMBER);
            var ORDER_SHIP_NUMBER = dt.rows(indexes).data().pluck('ORDER_SHIP_NUMBER')[0];
            $("#ORDER_SHIP_NUMBER").text(ORDER_SHIP_NUMBER);
            var TMP_ITEM_ID = dt.rows(indexes).data().pluck('TMP_ITEM_ID')[0];
            var OSP_BATCH_ID = dt.rows(indexes).data().pluck('OSP_BATCH_ID')[0];
            var INVENTORY_ITEM_ID = dt.rows(indexes).data().pluck('INVENTORY_ITEM_ID')[0];
            if (TMP_ITEM_ID && OSP_BATCH_ID) {
                $("#INVENTORY_ITEM_ID").text(TMP_ITEM_ID);
            } else {
                $("#INVENTORY_ITEM_ID").text(INVENTORY_ITEM_ID);
            }
            var TMP_ITEM_NUMBER = dt.rows(indexes).data().pluck('TMP_ITEM_NUMBER')[0];
            var ITEM_NUMBER = dt.rows(indexes).data().pluck('ITEM_NUMBER')[0];
            if (TMP_ITEM_NUMBER) {
                $("#ITEM_NUMBER").text(TMP_ITEM_NUMBER);
                $("#PICK_STATUS").text("REP");
            } else {
                $("#ITEM_NUMBER").text(ITEM_NUMBER);
                $("#PICK_STATUS").text("");
            }
            $("#ITEM_NUMBER").text(ITEM_NUMBER);
            var SRC_REQUESTED_QUANTITY_UOM = dt.rows(indexes).data().pluck('SRC_REQUESTED_QUANTITY_UOM')[0];
            $('#SRC_REQUESTED_QUANTITY_UOM').text(SRC_REQUESTED_QUANTITY_UOM);

            var rowsData = PaperRollDataTablesBody.rows({ page: 'current' }).data();
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

            $('#txtBARCODE').focus().select();
        }
    });

    PaperRollDataTablesBody.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#SUB_ID").text("");
            $("#DLV_DETAIL_ID").text("");
            $("#ORDER_NUMBER").text("");
            $("#ORDER_SHIP_NUMBER").text("");
            $("#INVENTORY_ITEM_ID").text("");
            $("#ITEM_NUMBER").text("");
            $("#PICK_STATUS").text("");
            $('#SRC_REQUESTED_QUANTITY_UOM').text("");
            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            selected.splice(index, 1);
        }
    });

    var editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Delivery/PickDTEditor',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: function (d) {
                var DlvPickedIdList = [];
                $.each(d.data, function (key, value) {
                    DlvPickedIdList.push(d.data[key]['PICKED_ID']);
                });

                var data = {
                    'action': d.action,
                    'DlvPickedIdList': DlvPickedIdList
                }
                return JSON.stringify(data);
            },
            success: function (data) {
                if (data.status) {

                    PaperRollDataTablesBody.ajax.reload(null, false);
                    PaperRollBarcodeDataTablesBody.ajax.reload();
                    UpdateDeliveryDetailViewHeader();

                }
                else {
                    swal.fire(data.result);
                }
            }
        },
        table: "#PaperRollBarcodeDataTablesBody",
        formOptions: {
            main: {
                onBackground: 'none'
            }
        },
        idSrc: 'PICKED_ID',
        
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


    var PaperRollBarcodeDataTablesBody = $('#PaperRollBarcodeDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        orderMulti: true,
        serverSide: true,
        processing: true,
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetRollEditBarcode",
            "type": "Post",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text(),
                DELIVERY_STATUS_NAME: $("#DELIVERY_STATUS").text()
            },
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "BARCODE", name: "條碼號", autoWidth: true },
            { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
            { data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false },
            { data: "PICKED_ID", name: "PICKED_ID", autoWidth: true, visible: false }
        ],

        "order": [[1, 'desc']],
        select: {
            style: 'multi',
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                },
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
                    enabled: false,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        var rows = PaperRollBarcodeDataTablesBody.rows({ selected: true }).indexes();

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
                {
                    text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                    action: function (e) {
                        var data = PaperRollBarcodeDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }
                        var barcode = [];
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].PALLET_STATUS == '1') { //是否為拆板
                                barcode.push(data[i].BARCODE);
                            }
                        }
                        if (barcode.length > 0) {
                            swal.fire({
                                title: "注意",
                                html: "以下為拆板後的條碼，請更換庫存棧板上的舊條碼。<br>" + barcode.join('<br>'),
                                type: "warning",
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "確定",
                            }).then(function (result) {
                                if (result.value) {
                                    PrintLable(PaperRollBarcodeDataTablesBody, "/Delivery/PrintLabel", "7");
                                }
                            });
                        } else {
                            PrintLable(PaperRollBarcodeDataTablesBody, "/Delivery/PrintLabel", "7");
                        }
                    },
                    className: "btn-primary",
                    enabled: false,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                }
            ],
        }


    });

    $('#txtBARCODE').keydown(function (e) {
        if (e.keyCode == 13) {
            InputBarcode();
        }
    })



    $("#btnSaveBarcode").click(function () {
        InputBarcode();
    });


    $("#btnCheckDeliveryName").click(function () {
        deliveryNameCheck(true);

    });

    $('#txtDeliveryName').keydown(function (e) {
        if (e.keyCode == 13) {
            deliveryNameCheck(true);
        }
    })

    function InputBarcode() {
        var DLV_DETAIL_ID = $('#DLV_DETAIL_ID').text();
        if (!DLV_DETAIL_ID) {
            swal.fire('請選擇料號');
            event.preventDefault();
            return false;
        }

        var BARCODE = $('#txtBARCODE').val();
        if (!BARCODE) {
            swal.fire('請輸入條碼');
            event.preventDefault();
            return false;
        }


        $('#txtBARCODE').attr('disabled', true);

        ShowWait(function () {
            $.ajax({
                url: "/Delivery/InputRollEditBarcode",
                type: "post",
                data: {
                    BARCODE: BARCODE,
                    DlvHeaderId: $("#DlvHeaderId").text(),
                    DLV_DETAIL_ID: $("#DLV_DETAIL_ID").text(),
                    DELIVERY_NAME: $("#DELIVERY_NAME").text()
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        PaperRollDataTablesBody.ajax.reload(null, false);
                        PaperRollBarcodeDataTablesBody.ajax.reload();
                        UpdateDeliveryDetailViewHeader();

                    }
                    else {

                        swal.fire({
                            title: data.result,
                            onAfterClose: function () {
                                $('#txtBARCODE').focus().select();
                            }
                        });
                    }
                },
                error: function () {
                    swal.fire('條碼輸入失敗');

                },
                complete: function (data) {
                    $('#txtBARCODE').attr('disabled', false);
                }

            });
        });

    }

    function DeleteBarcode(selectedData) {


        var list = [];
        for (i = 0; i < selectedData.length; i++) {
            list.push(selectedData[i].PICKED_ID);
        }


        $.ajax({
            url: "/Delivery/DeleteRollEditBarcode",
            type: "post",
            data: {
                'PICKED_ID': list,
                DlvHeaderId: $("#DlvHeaderId").text(),
            },
            success: function (data) {
                if (data.status) {
                    if (data.result == "條碼刪除成功") {
                        PaperRollDataTablesBody.ajax.reload(null, false);
                        PaperRollBarcodeDataTablesBody.ajax.reload();
                        UpdateDeliveryDetailViewHeader();
                    }
                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('條碼刪除失敗');
            },
            complete: function (data) {


            }

        });
    }

    function deliveryNameCheck(showDialog) {
        var DELIVERY_NAME = $("#DELIVERY_NAME").text();

        if (DELIVERY_NAME == $("#txtDeliveryName").val()) {
            inputOpen();
        } else {
            if (showDialog) {
                swal.fire("交運單號輸入錯誤");
                event.preventDefault();
            }
        }
    }

    function inputOpen() {
        PaperRollDataTablesBody.buttons().enable();
        PaperRollBarcodeDataTablesBody.buttons().enable();
        $('#txtBARCODE').attr('disabled', false);
        $('#btnSaveBarcode').attr('disabled', false);

    }

    function inpuClose() {
        $('#txtBARCODE').attr('disabled', true);
        $('#btnSaveBarcode').attr('disabled', true);
    }
    inpuClose();

    deliveryNameCheck(false);
});



