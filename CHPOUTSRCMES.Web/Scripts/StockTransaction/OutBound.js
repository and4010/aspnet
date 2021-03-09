
function OutBoundInit() {

    var selected = [];
    var detailEditor;
    var pickEditor;

    var selectedNumberStatus = "0";
    var selectedTransferHeaderId = 0;

    $("#ddlShipmentNumber").combobox({
        select: function (event, ui) {
            SelectShipmentNumber();
        }
    });

    function getShipmentNumber() {
        return $('#ddlShipmentNumber option:selected').text();
    }

    function getTransferHeaderId() {
        return $('#ddlShipmentNumber').val();
    }

    function getOutOrganizationId() {
        return $("#ddlOutOrganization").val();
    }

    function getInOrganizationId() {
        return $("#ddlInOrganization").val();
    }


    function getOutSubinventoryCode() {
        return $("#ddlOutSubinventory option:selected").text();
    }
    function getInSubinventoryCode() {
        return $("#ddlInSubinventory option:selected").text();
    }

    function getOutLocatorId() {
        if ($('#ddlOutLocatorArea').is(":visible")) {
            return $("#ddlOutLocator").val();
        } else {
            return null;
        }
    }

    function getInLocatorId() {
        if ($('#ddlInLocatorArea').is(":visible")) {
            return $("#ddlInLocator").val();
        } else {
            return null;
        }
    }

    function getReamQty() {
        if ($('#txtSECONDARY_QUANTITY').is(":visible")) {
            return $('#txtSECONDARY_QUANTITY').val();
        } else {
            return 0;
        }


    }

    SubinventoryInit();

    $('.custom-combobox-input').css('width', '250px');

    $("#btnPrintPick").click(function () {
        var shipmentNumber = getShipmentNumber();
        if (shipmentNumber == "新增編號") {
            swal.fire('請選擇出貨編號');
            event.preventDefault();
            return;
        }
        window.open("/StockTransaction/OutboundPickingReport/?shipmentNumber=" + shipmentNumber);
    });

    $("#btnPickBarcode").click(function () {
        OutboundCreatePick();
    });


    $("#btnSaveTransfer").click(function () {
        OutBoundSaveTransfer();
    });


    $('#AutoCompleteShipmentNumber').keydown(function (e) {
        if (e.keyCode == 13) {
            if (getOutSubinventoryCode() == "請選擇") {
                swal.fire('請選擇發貨倉庫');
                event.preventDefault();
                return;
            }
            if ($('#ddlOutLocatorArea').is(":visible")) {
                if ($('#ddlOutLocator').val() == "請選擇") {
                    swal.fire('請選擇發貨儲位');
                    event.preventDefault();
                    return;
                }
            }
            if (getInSubinventoryCode() == "請選擇") {
                swal.fire('請選擇收貨倉庫');
                event.preventDefault();
                return;
            }
            if ($('#ddlInLocatorArea').is(":visible")) {
                if ($('#ddlInLocator').val() == "請選擇") {
                    swal.fire('請選擇收貨儲位');
                    event.preventDefault();
                    return;
                }
            }
            $(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
        }
    });


    $('#txtInputTransactionQty').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#txtRollReamQty').focus();
        }
    });

    $('#txtRollReamQty').keydown(function (e) {
        if (e.keyCode == 13) {
            CheckSaveStockTransferDT();
        }
    });

    $('#txtBARCODE').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($('#txtSECONDARY_QUANTITY').is(":visible")) {
                $('#txtSECONDARY_QUANTITY').focus();
                return;
            }
            OutboundCreatePick();
        }
    });

    $("#txtBARCODE").on(
        "input propertychange paste", function () {
            $("#txtSECONDARY_QUANTITY").val("");
        });

    $('#txtSECONDARY_QUANTITY').keydown(function (e) {
        if (e.keyCode == 13) {
            OutboundCreatePick();
        }
    });


    $("#btnSaveStockTransferDT").click(function () {
        CheckSaveStockTransferDT();
    });


    function SubinventoryInit() {

        $('#ddlOutOrganization').change(function () {

            ShowWait(function () {
                $.ajax({
                    url: "/StockTransaction/GetSubinventoryListForUserId",
                    type: "post",
                    data: {
                        ORGANIZATION_ID: getOutOrganizationId()
                    },
                    success: function (data) {
                        CloseWait();
                        $('#ddlOutSubinventory').html("");

                        for (var i = 0; i < data.length; i++) {
                            $('#ddlOutSubinventory').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                        }

                    },
                    error: function () {
                        swal.fire('更新發貨組織失敗');
                    },
                    complete: function () {

                    }

                });
            });
           

        })

        $('#ddlInOrganization').change(function () {

            ShowWait(function () {
                $.ajax({
                    url: "/StockTransaction/GetSubinventoryList",
                    type: "post",
                    data: {
                        ORGANIZATION_ID: getInOrganizationId()
                    },
                    success: function (data) {
                        CloseWait();
                        $('#ddlInSubinventory').html("");

                        for (var i = 0; i < data.length; i++) {
                            $('#ddlInSubinventory').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                        }

                    },
                    error: function () {
                        swal.fire('更新收貨組織失敗');
                    },
                    complete: function () {

                    }

                });
            });

        })

        $('#ddlOutSubinventory').change(function () {

            var SUBINVENTORY_CODE = getOutSubinventoryCode();

            ShowWait(function () {
                $.ajax({
                    url: "/StockTransaction/GetLocatorListForUserId",
                    type: "post",
                    data: {
                        SUBINVENTORY_CODE: SUBINVENTORY_CODE
                    },
                    success: function (data) {
                        $('#ddlOutLocator').empty();
                        for (var i = 0; i < data.length; i++) {
                            $('#ddlOutLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                        }
                        if (data.length == 1) {
                            $('#ddlOutLocatorArea').hide();
                            $('#ddlOutLocatorArea2').hide();

                        } else {
                            $('#ddlOutLocatorArea').show();
                            $('#ddlOutLocatorArea2').show();
                        }

                    },
                    error: function () {
                        swal.fire('更新發貨儲位失敗');
                    },
                    complete: function (data) {
                        GetShipmentNumberList("新增編號", "新增編號");
                        $('#AutoCompleteItemNumber').val("");
                        $('#PACKING_TYPE_LABEL').hide();
                        $('#PACKING_TYPE').html("");
                        $('#PACKING_TYPE').hide();
                        $('#UNIT').html("");
                    }

                })
            });
            


        })


        $('#ddlInSubinventory').change(function () {

            var SUBINVENTORY_CODE = getInSubinventoryCode();

            ShowWait(function () {
                $.ajax({
                    url: "/StockTransaction/GetLocatorList",
                    type: "post",
                    data: {
                        ORGANIZATION_ID: "*",
                        SUBINVENTORY_CODE: SUBINVENTORY_CODE,
                    },
                    success: function (data) {
                      
                        $('#ddlInLocator').empty();
                        for (var i = 0; i < data.length; i++) {
                            $('#ddlInLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                        }
                        if (data.length == 1) {
                            $('#ddlInLocatorArea').hide();
                            $('#ddlInLocatorArea2').hide();
                        } else {
                            $('#ddlInLocatorArea').show();
                            $('#ddlInLocatorArea2').show();
                        }

                    },
                    error: function () {
                        swal.fire('更新收貨儲位失敗');
                    },
                    complete: function (data) {
                        GetShipmentNumberList("新增編號", "新增編號");
                    }

                })
            });
            


        })


        $('#ddlOutLocator').change(function () {
           
        })

        $('#ddlInLocator').change(function () {
            
        })

    }


    detailEditor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockTransaction/OutboundDetailEditor',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var StockTransferDTData = d.data;
                var StockTransferDTList = [];
                var size = Object.keys(StockTransferDTData).length;
                for (var i = 0; i < size; i++) {
                    var ID = Object.keys(StockTransferDTData)[i];
                    var REMARK = Object.keys(StockTransferDTData[ID]).map(function (e) {
                        return StockTransferDTData[ID][e]
                    })[0];

                    var StockTransferDT = {
                        'ID': ID
                    }
                    StockTransferDTList.push(StockTransferDT);
                }
                var data = {
                    'action': d.action,
                    'StockTransferDTList': StockTransferDTList
                }


                return JSON.stringify(data);
            },
            success: function (data) {
                if (data.status) {

                    OutBoundDataTablesBody.ajax.reload();
                    OutBoundBarcodeDataTablesBody.ajax.reload();
                }
                else {
                    swal.fire(data.result);
                }
            }
        },
        table: "#OutBoundDataTablesBody",
        idSrc: 'ID',
        i18n: {
            remove: {
                button: "刪除",
                title: "確定要刪除??",
                submit: "確定",
                confirm: {
                    "_": "你確定要刪除這筆資料?",
                    "1": "你確定要刪除這些資料?"
                }
            },
        }
    });

    var OutBoundDataTablesBody = $('#OutBoundDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        dom:
            "<'row'<'col-sm-2 width-s'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockTransaction/GetOutboundStockTransferDT",
            "type": "Post",
            "datatype": "json",
            "data": function (d) {
                d.transferHeaderId = selectedTransferHeaderId;
                d.numberStatus = selectedNumberStatus;
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },
            { data: "ROLL_REAM_QTY", name: "捲數/板數", autoWidth: true },
            { data: "ROLL_REAM_UOM", name: "捲/板", autoWidth: true },
            { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right" },
            {
                data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
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
            { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },
            {
                data: "REQUESTED_QUANTITY2", name: "預計出庫輔數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
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
                data: "PICKED_QUANTITY2", name: "出庫已揀輔數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
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
            { data: "REQUESTED_QUANTITY_UOM2", name: "輔單位", autoWidth: true }
        ],

        "order": [[1, 'asc']],
        select: {
            style: 'single',
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                },
            },
            buttons: [

                'selectNone',
                {
                    extend: "remove",
                    text: '刪除',
                    name: 'remove',
                    className: 'btn-danger',
                    editor: pickEditor,
                    enabled: false,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        var rows = OutBoundDataTablesBody.rows({ selected: true }).indexes();

                        if (rows.length === 0) {
                            return;
                        }

                        detailEditor.remove(rows, {
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

            ]
        },

        "rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {
            if ($.inArray(data.ID, selected) !== -1) {
                var selectRow = ':eq(' + dataIndex + ')';
                OutBoundDataTablesBody.row(selectRow, { page: 'current' }).select();
            }
        },
        "preDrawCallback": function (settings) {
            $("#DetailSubId").text("");
            $("#DetailId").text("");
            $("#SelectedItemNumber2").text("");
            $("#PACKING_TYPE2").text("");
        }

    });

    OutBoundDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var DetailSubId = dt.rows(indexes).data().pluck('SUB_ID')[0];
            $("#DetailSubId").text(DetailSubId);
            var DetailId = dt.rows(indexes).data().pluck('ID')[0];
            $("#DetailId").text(DetailId);
            var SelectedItemNumber2 = dt.rows(indexes).data().pluck('ITEM_NUMBER')[0];
            $("#SelectedItemNumber2").text(SelectedItemNumber2);
            var PACKING_TYPE = dt.rows(indexes).data().pluck('PACKING_TYPE')[0];
            if (!PACKING_TYPE) {
                PACKING_TYPE = "";
            }
            $('#PACKING_TYPE2').text(PACKING_TYPE);

            var rowsData = OutBoundDataTablesBody.rows({ page: 'current' }).data();
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

            if (PACKING_TYPE == "令包") {
                $("#SECONDARY_QUANTITY").show();
                $("#txtSECONDARY_QUANTITY").show();
            }

            $('#txtBARCODE').focus().select();
        }
    });

    OutBoundDataTablesBody.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#DetailSubId").text("");
            $("#DetailId").text("");
            $("#SelectedItemNumber2").text("");
            $("#PACKING_TYPE2").text("");
            var PACKING_TYPE = dt.rows(indexes).data().pluck('PACKING_TYPE')[0];
            if (PACKING_TYPE == "令包") {
                $("#SECONDARY_QUANTITY").hide();
                $("#txtSECONDARY_QUANTITY").hide();
                $("#txtSECONDARY_QUANTITY").val("");
            }

            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            selected.splice(index, 1);
        }
    });


    pickEditor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockTransaction/OutboundPickEditor',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var StockTransferBarcodeDTData = d.data;
                var StockTransferBarcodeDTList = [];
                var size = Object.keys(StockTransferBarcodeDTData).length;
                for (var i = 0; i < size; i++) {
                    var ID = Object.keys(StockTransferBarcodeDTData)[i];
                    var REMARK = Object.keys(StockTransferBarcodeDTData[ID]).map(function (e) {
                        return StockTransferBarcodeDTData[ID][e]
                    })[0];

                    var StockTransferBarcodeDT = {
                        'ID': ID,
                        'REMARK': REMARK
                    }
                    StockTransferBarcodeDTList.push(StockTransferBarcodeDT);
                }
                var data = {
                    'action': d.action,
                    'StockTransferBarcodeDTList': StockTransferBarcodeDTList
                }


                return JSON.stringify(data);
            },
            success: function (data) {
                if (data.status) {
                    OutBoundDataTablesBody.ajax.reload(null, false);
                    OutBoundBarcodeDataTablesBody.ajax.reload();
                }
                else {
                    swal.fire(data.result);
                }
            }
        },
        table: "#OutBoundBarcodeDataTablesBody",
        idSrc: 'ID',
        fields: [
            {
                label: "備註:",
                name: "REMARK",
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

    var OutBoundBarcodeDataTablesBody = $('#OutBoundBarcodeDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        dom:
            "<'row'<'col-sm-2 width-s'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockTransaction/GetOutboundStockTransferBarcodeDT",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.transferHeaderId = selectedTransferHeaderId;
                d.numberStatus = selectedNumberStatus;
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            { data: "BARCODE", name: "條碼", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },
            { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
            {
                data: "SECONDARY_QUANTITY", name: "次要數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
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
            { data: "SECONDARY_UOM", name: "次要單位", autoWidth: true },
            { data: "REMARK", name: "備註", autoWidth: true, className: "dt-body-left" },
            { data: "ID", name: "ID", autoWidth: true, visible: false },
        ],

        order: [[1, 'desc']],
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
                    editor: pickEditor,
                    enabled: false,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        var rows = OutBoundBarcodeDataTablesBody.rows({ selected: true }).indexes();

                        if (rows.length === 0) {
                            return;
                        }

                        pickEditor.remove(rows, {
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
                        var data = OutBoundBarcodeDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }
                        var barcode = [];
                        var transferPickedIdList = [];
                        for (var i = 0; i < data.length; i++) {
                            transferPickedIdList.push(data[i].ID);
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
                                    PrintLable(OutBoundBarcodeDataTablesBody, "/StockTransaction/PrintOutboundLabel", "10");
                                }
                            });
                        } else {
                            PrintLable(OutBoundBarcodeDataTablesBody, "/StockTransaction/PrintOutboundLabel", "10");
                        }
                    },
                    className: "btn-primary",
                    enabled: false,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
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

                        pickEditor.edit(OutBoundBarcodeDataTablesBody.rows({ selected: true }).indexes())
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




    $("#SECONDARY_QUANTITY").hide();
    $("#txtSECONDARY_QUANTITY").val("");


    function SelectShipmentNumber(message) {
        var transferHeaderId = 0;
        if (getShipmentNumber() == "新增編號") {
            transferHeaderId = 0;
        } else {
            transferHeaderId = getTransferHeaderId();
        }

        $.ajax({
            url: "/StockTransaction/GetShipmentNumberData",
            type: "post",
            data: {
                transferHeaderId: transferHeaderId
            },
            success: function (data) {
                if (data.Success) {
                    CloseWait();
                    $('#txtBARCODE').val("");
                    $('#txtInputTransactionQty').val("");
                    $('#txtRollReamQty').val("");
                    $("#SECONDARY_QUANTITY").hide();
                    $('#txtSECONDARY_QUANTITY').val("");
                    inputOpen();
                    if (data.Msg == "新增編號") {
                        selectedTransferHeaderId = 0;
                        selectedNumberStatus = "0";
                        OutBoundDataTablesBody.ajax.reload();
                        OutBoundBarcodeDataTablesBody.ajax.reload();
                        return;
                    }
                    selectedTransferHeaderId = data.Data.TransferHeaderId;
                    selectedNumberStatus = data.Data.NumberStatus;

                    if ($('#ddlOutLocatorArea').is(":visible")) {
                        $("#ddlOutLocator").val(data.Data.LocatorId);
                    }

                    if ($('#ddlInLocatorArea').is(":visible")) {
                        $("#ddlInLocator").val(data.Data.TransferLocatorId);
                    }
                    OutBoundDataTablesBody.ajax.reload();
                    OutBoundBarcodeDataTablesBody.ajax.reload();
                    if (data.Data.NumberStatus == "1") {
                        //出貨編號已存檔
                        if (message) {
                            //show入庫存檔的成功訊息
                            swal.fire(message);
                        }
                        inputClose();
                    } else {

                    }

                } else {

                    swal.fire(data.Msg);
                }


            },
            error: function () {
                swal.fire('取得出貨編號資料失敗');
            },
            complete: function (data) {


            }

        });
        
    }


    function inputOpen() {
        OutBoundDataTablesBody.buttons().enable();
        OutBoundBarcodeDataTablesBody.buttons().enable();
        $('#AutoCompleteItemNumber').attr('disabled', false);
        $('#txtInputTransactionQty').attr('disabled', false);
        $('#txtRollReamQty').attr('disabled', false);
        $('#txtBARCODE').attr('disabled', false);
        $('#btnPrintPick').attr('disabled', false);
        $('#btnPickBarcode').attr('disabled', false);
        $('#btnSaveTransfer').attr('disabled', false);
        $('#btnSaveStockTransferDT').attr('disabled', false);
        $('#txtSECONDARY_QUANTITY').attr('disabled', false);
    }

    function inputClose() {
        OutBoundDataTablesBody.buttons().disable();
        OutBoundBarcodeDataTablesBody.buttons().disable();
        $('#AutoCompleteItemNumber').attr('disabled', true);
        $('#txtInputTransactionQty').attr('disabled', true);
        $('#txtRollReamQty').attr('disabled', true);
        $('#txtBARCODE').attr('disabled', true);
        $('#btnPrintPick').attr('disabled', true);
        $('#btnPickBarcode').attr('disabled', true);
        $('#btnSaveTransfer').attr('disabled', true);
        $('#btnSaveStockTransferDT').attr('disabled', true);
        $('#txtSECONDARY_QUANTITY').attr('disabled', true);
    }


    $("#AutoCompleteItemNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/AutoCompleteItemNumber",
                type: "POST",
                dataType: "json",
                data: {
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Value + " " + item.Description, value: item.Value };
                    }))
                }
            })
        },
        autoFocus: true,
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            if (ui.item) {
                $('#AutoCompleteItemNumber').val(ui.item.value);
                $('#txtInputTransactionQty').focus();
            }
        }
    });

    //離開料號欄位自動搜尋
    $('#AutoCompleteItemNumber').blur(function () {
        GetStockItemData($('#AutoCompleteItemNumber').val(), false);
    });



    function CheckSaveStockTransferDT() {

        if (getOutOrganizationId() == "請選擇") {
            swal.fire('請選擇發貨組織');
            event.preventDefault();
            return;
        }

        if (getInOrganizationId() == "請選擇") {
            swal.fire('請選擇收貨組織');
            event.preventDefault();
            return;
        }

        if (getOutSubinventoryCode() == "請選擇") {
            swal.fire('請選擇發貨倉庫');
            event.preventDefault();
            return;
        }
        if ($('#ddlOutLocatorArea').is(":visible")) {
            if ($('#ddlOutLocator').val() == "請選擇") {
                swal.fire('請選擇發貨儲位');
                event.preventDefault();
                return;
            }
        }
        if (getInSubinventoryCode() == "請選擇") {
            swal.fire('請選擇收貨倉庫');
            event.preventDefault();
            return;
        }
        if ($('#ddlInLocatorArea').is(":visible")) {
            if ($('#ddlInLocator').val() == "請選擇") {
                swal.fire('請選擇收貨儲位');
                event.preventDefault();
                return;
            }
        }

        if ($('#AutoCompleteItemNumber').val() == "") {
            swal.fire('請輸入料號');
            event.preventDefault();
            return;
        }
        if ($('#txtInputTransactionQty').val() == "") {
            swal.fire('請輸入數量');
            event.preventDefault();
            return;
        }
        if ($('#txtRollReamQty').val() == "") {
            swal.fire('請輸入棧板數、捲數');
            event.preventDefault();
            return;
        }

        var shipmentNumber = $('#ddlShipmentNumber').val();
        if (shipmentNumber == "新增編號") {
            setTimeout(function () {
                swal.fire({
                    title: "新增編號",
                    text: "確定新增編號嗎?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "確定",
                    cancelButtonText: "取消"
                }).then(function (result) {
                    if (result.value) {
                        OutboundCreateDetail();

                    }
                });
            }, 10)
        } else {
            OutboundCreateDetail();
        }

    }

    function OutboundCreateDetail() {
        ShowWait(function () {
            $.ajax({
                url: "/StockTransaction/OutboundCreateDetail",
                type: "post",
                data: {
                    shipmentNumber: getShipmentNumber(),
                    transferType: $("#ddlTransferType").val(),
                    itemNumber: $('#AutoCompleteItemNumber').val(),
                    outOrganizationId: getOutOrganizationId(),
                    outSubinventoryCode: getOutSubinventoryCode(),
                    outLocatorId: getOutLocatorId(),
                    inOrganizationId: getInOrganizationId(),
                    inSubinventoryCode: getInSubinventoryCode(),
                    inLocatorId: getInLocatorId(),
                    requestedQty: $('#txtInputTransactionQty').val(),
                    rollReamQty: $('#txtRollReamQty').val()

                },
                success: function (data) {
                    if (data.status) {
                        
                        GetShipmentNumberList(data.transferHeaderId, data.shipmentNumber);


                    } else {
                        swal.fire(data.result);
                    }

                },
                error: function () {
                    swal.fire('新增備貨單明細失敗');
                },
                complete: function (data) {

                }

            });
        });
        
    }


    function OutboundCreatePick() {

        var DetailId = $('#DetailId').text();
        if (!DetailId) {
            swal.fire('請選擇料號');
            event.preventDefault();
            return false;
        }

        if ($('#txtBARCODE').is(":visible")) {
            if ($('#txtBARCODE').val() == "") {
                swal.fire('請輸入條碼');
                event.preventDefault();
                return;
            }
        }

        if ($('#txtSECONDARY_QUANTITY').is(":visible")) {
            if ($('#txtSECONDARY_QUANTITY').val() == "") {
                swal.fire('請輸入令數');
                event.preventDefault();
                return;
            }
        }

        ShowWait(function () {
            $.ajax({
                url: "/StockTransaction/OutboundCreatePick",
                type: "post",
                data: {
                    transferHeaderId: getTransferHeaderId(),
                    transferDetailId: DetailId,
                    barcode: $('#txtBARCODE').val(),
                    reamQty: getReamQty()
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        OutBoundDataTablesBody.ajax.reload(null, false);
                        OutBoundBarcodeDataTablesBody.ajax.reload();
                    } else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('新增條碼失敗');
                },
                complete: function (data) {


                }

            });
        });
        
    }


    function OutBoundSaveTransfer() {
        if (getShipmentNumber() == "新增編號") {
            swal.fire('請選擇出貨編號');
            event.preventDefault();
            return;
        }

        swal.fire({
            title: "出庫存檔",
            text: "確定出庫存檔嗎?",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                ShowWait(function () {
                    $.ajax({
                        url: "/StockTransaction/OutBoundSaveTransfer",
                        type: "post",
                        data: {
                            transferHeaderId: getTransferHeaderId()
                        },
                        success: function (data) {
                            if (data.status) {
                                SelectShipmentNumber(data.result);
                            } else {
                                swal.fire(data.result);
                            }
                        },
                        error: function () {
                            swal.fire('出庫存檔失敗');
                        },
                        complete: function (data) {


                        }

                    });
                });

            }
        });


    }

    function GetStockItemData(ITEM_NO, focusNext) {
        $.ajax({
            url: "/StockTransaction/GetStockItemData",
            type: "post",
            data: {
                SUBINVENTORY_CODE: getOutSubinventoryCode(),
                ITEM_NO: ITEM_NO
            },
            success: function (data) {
                CloseWait();
                if (data.Success) {

                    if (data.Data.CatalogElemVal070 == "平版") {
                        $('#PACKING_TYPE_LABEL').show();
                        $('#PACKING_TYPE').show();
                        $('#PACKING_TYPE').html(data.Data.CatalogElemVal110);
                        $('#UNIT').html(data.Data.SecondaryUomCode);
                        if (focusNext) {
                            $('#txtInputTransactionQty').focus();
                        }
                    } else {
                        $('#PACKING_TYPE_LABEL').hide();
                        $('#PACKING_TYPE').hide();
                        $('#PACKING_TYPE').html("");
                        $('#UNIT').html(data.Data.PrimaryUomCode);
                        if (focusNext) {
                            $('#txtInputTransactionQty').focus();
                        }
                    }

                } else {
                    
                    $('#PACKING_TYPE_LABEL').hide();
                    $('#PACKING_TYPE').html("");
                    $('#PACKING_TYPE').hide();
                    $('#UNIT').html("");
                    
                }


            },
            error: function () {
                swal.fire('取得料號資料失敗');
            },
            complete: function (data) {


            }

        });
       
    }


    function GetShipmentNumberList(selectValue, selectText) {

        var OutOrganizationId = getOutOrganizationId();
        var InOrganizationId = getInOrganizationId();
        var InSubinventoryCode = getInSubinventoryCode();
        var OutSubinventoryCode = getOutSubinventoryCode();

        if (InSubinventoryCode == "請選擇" || OutSubinventoryCode == "請選擇" || OutOrganizationId == "請選擇" || InOrganizationId == "請選擇") {
            CloseWait();
            return;
        }

        ShowWait(function () {
            $.ajax({
                url: "/StockTransaction/GetOutboundShipmentNumberList",
                type: "post",
                data: {
                    outOrganizationId: OutOrganizationId,
                    outSubinventoryCode: OutSubinventoryCode,
                    inOrganizationId: InOrganizationId,
                    inSubinventoryCode: InSubinventoryCode
                },
                success: function (data) {
                    $('#ddlShipmentNumber').empty();
                    for (var i = 0; i < data.items.length; i++) {
                        $('#ddlShipmentNumber').append($('<option></option>').val(data.items[i].Value).html(data.items[i].Text));
                    }
                    $("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);
                    SelectShipmentNumber(); 
                },
                error: function () {
                    swal.fire('更新出貨編號失敗');
                },
                complete: function (data) {


                }

            })
        });

        

    }

}









