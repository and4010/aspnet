function InBoundInit() {
    var selected = [];
    var editor;
    var selectedNumber;
    var mergeBacrodeEditor;

    $('#ddlOutSubinventory').change(function () {

        var SUBINVENTORY_CODE = $("#ddlOutSubinventory").val();
        $.ajax({
            url: "/StockTransaction/GetLocatorList",
            type: "post",
            data: {
                SUBINVENTORY_CODE: SUBINVENTORY_CODE
            },
            success: function (data) {
                $('#ddlOutLocator').empty();
                for (var i = 0; i < data.length; i++) {
                    $('#ddlOutLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }
                //GetItemNumberList();
                if (data.length == 1) {
                    $('#ddlOutLocatorArea').hide();

                } else {
                    $('#ddlOutLocatorArea').show();
                }

                //$('ddlOutLocator').html("");



                //var optionCount = ddl[0].length;
                //if (optionCount == 2) {
                //    //選單數量為2時，選擇第2個                
                //    ddl.combobox('autocomplete', ddl[0][1].value, ddl[0][1].text);
                //} else {
                //    ddl.combobox('autocomplete', ddl[0][0].value, ddl[0][0].text);
                //}

            },
            error: function () {
                swal.fire('更新發貨儲位失敗');
            },
            complete: function (data) {
                checkTransactionType();
                $('#AutoCompleteItemNumber').val("");
                $('#PACKING_TYPE_LABEL').hide();
                $('#PACKING_TYPE').html("");
                $('#PACKING_TYPE').hide();
                $('#UNIT').html("");
            }

        })


    })


    $('#ddlInSubinventory').change(function () {

        var SUBINVENTORY_CODE = $("#ddlInSubinventory").val();
        $.ajax({
            url: "/StockTransaction/GetLocatorList",
            type: "post",
            data: {
                SUBINVENTORY_CODE: SUBINVENTORY_CODE
            },
            success: function (data) {
                $('#ddlInLocator').empty();
                for (var i = 0; i < data.length; i++) {
                    $('#ddlInLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }
                if (data.length == 1) {
                    $('#ddlInLocatorArea').hide();
                } else {
                    $('#ddlInLocatorArea').show();
                }

            },
            error: function () {
                swal.fire('更新收貨儲位失敗');
            },
            complete: function (data) {
                checkTransactionType();

            }

        })


    })


    $('#ddlOutLocator').change(function () {
        checkTransactionType();
        $('#AutoCompleteItemNumber').val("");
        $('#PACKING_TYPE_LABEL').hide();
        $('#PACKING_TYPE').html("");
        $('#PACKING_TYPE').hide();
        $('#UNIT').html("");
        //GetItemNumberList();
    })

    $('#ddlInLocator').change(function () {
        checkTransactionType();
    })

    $('#txtInputTransactionQty').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($('#REAM_INPUT_AREA').is(":visible")) {
                $('#txtRollReamWT').focus();
            } else if ($('#ROLL_INPUT_AREA').is(":visible")) {
                $('#txtLotNumber').focus();
            }
        }
    });

    checkTransactionType();

    $("#btnExampleDownload").click(function () {
        window.open('/Home/DownloadFile', '_blank');
    });

    $("#btnImportFile").click(function () {
        if ($('#ddlOutSubinventory').val() == "請選擇") {
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
        if ($('#ddlInSubinventory').val() == "請選擇") {
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
        if ($('#AutoCompleteShipmentNumber').val().trim() == "") {
            swal.fire("請輸入編號");
            event.preventDefault();
            return;
        }

        $.ajax({
            url: '/StockTransaction/_ImportBodyRoll/',
            type: "POST",
            success: function (result) {
                $('body').append(result);
                Open($('#PaperRollModal'));
            },
            error: function () {
                swal.fire("失敗");
            }
        });
    });
    $("#btnImportFlatFile").click(function () {
        if ($('#ddlOutSubinventory').val() == "請選擇") {
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
        if ($('#ddlInSubinventory').val() == "請選擇") {
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

        if ($('#AutoCompleteShipmentNumber').val().trim() == "") {
            swal.fire("請輸入編號");
            event.preventDefault();
            return;
        }

        $.ajax({
            url: '/StockTransaction/_ImportBodyFlat/',
            type: "POST",
            success: function (result) {
                $('body').append(result);
                Open($('#FlatModal'));
            },
            error: function () {
                swal.fire("失敗");
            }
        });
    });


    $("#btnPrintPick").click(function () {
       
    });

    $("#txtBARCODE").keydown(function (e) {
        if (e.keyCode == 13) {
            BarcodeInbound();
        }
    });

    $("#btnSaveBarcode").click(function () {
        BarcodeInbound();
    });


    $("#btnSaveTransfer").click(function () {
        InBoundSaveTransfer();
    });

    $("#btnSaveStockTransferDT").click(function () {
        CheckCreateInboundBarcode();
    });

    $('#AutoCompleteItemNumber').keydown(function (e) {
        if (e.keyCode == 13) {
            $(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
            $('#txtInputTransactionQty').focus();
        }
    });

   
    

    $('#AutoCompleteShipmentNumber').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($('#ddlOutSubinventory').val() == "請選擇") {
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
            if ($('#ddlInSubinventory').val() == "請選擇") {
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
            //$('#AutoCompleteItemNumber').focus().select();
        }
    });

    $('#txtRollReamWT').keydown(function (e) {
        if (e.keyCode == 13) {
            CheckCreateInboundBarcode();
        }
    });
     
    $('#txtLotNumber').keydown(function (e) {
        if (e.keyCode == 13) {
            CheckCreateInboundBarcode();
        }
    });

    


    $("#AutoCompleteShipmentNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/InboundAutoCompleteShipmentNumber",
                type: "POST",
                dataType: "json",
                data: {
                    TransactionType: GetTransactionType(),
                    OutSubinventoryCode: $("#ddlOutSubinventory").val(),
                    OutLocator: $("#ddlOutLocator").val(),
                    InSubinventoryCode: $("#ddlInSubinventory").val(),
                    InLocator: $("#ddlInLocator").val(),
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Description, value: item.Value };
                    }))
                }
            })
        },
        //autoFocus: true,
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            if (ui.item) {
                //selectedNumber = ui.item.value;
                GetNumberStatus();
            }
        }
    });

    function GetNumberStatus() {
        $.ajax({
            url: "/StockTransaction/GetNumberStatus",
            type: "POST",
            dataType: "json",
            data: {
                TransactionType: GetTransactionType(),
                OUT_SUBINVENTORY_CODE: $("#ddlOutSubinventory").val(),
                OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
                IN_SUBINVENTORY_CODE: $("#ddlInSubinventory").val(),
                IN_LOCATOR_ID: $("#ddlInLocator").val(),
                Number: $('#AutoCompleteShipmentNumber').val()
            },
            success: function (data) {
                if (data.status) {

                    $('#NumberStatus').html(data.result);
                    PickInputAreaHide();
                    InputOpen();
                    $('#txtBARCODE').val("");
                    InBoundBarcodeDataTablesBody.ajax.reload();

                    if (data.result == "MES未出庫") {
                        swal.fire("編號" + $('#AutoCompleteShipmentNumber').val() + "尚未出庫存檔");
                        $('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                        //selectedNumber = "";
                    } else if (data.result == "MES已出庫") {
                        $('#txtBARCODE').focus();
                    } else if (data.result == "非MES出庫手動新增") {
                        $('#PickInputArea').show();
                        $('#AutoCompleteItemNumber').focus();
                    } else if (data.result == "非MES出庫檔案匯入") {
                        $('#txtBARCODE').focus();
                    } else if (data.result == "非MES已入庫") {
                        InputClose();
                    } else if (data.result == "MES已入庫") {
                        InputClose();
                    } else {
                        //為新增編號 此編號還未儲存
                        $('#PickInputArea').show();
                        $('#AutoCompleteItemNumber').focus();
                    }
                } else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('取得編號狀態失敗');
            }
        })
    }

    function PickInputAreaHide() {
        $('#PickInputArea').hide();

        $('#AutoCompleteItemNumber').autocomplete('close').val('');
        $('#PACKING_TYPE').html("");

        QtyInputAreaHide();
    }

    function QtyInputAreaHide() {
        $('#QtyInputArea').hide();

        $('#txtInputTransactionQty').val("");
        $('#UNIT').html("");

        $('#REAM_INPUT_AREA').hide();
        $('#txtRollReamWT').val("");

        $('#ROLL_INPUT_AREA').hide();
        $('#txtLotNumber').val("");
    }

    function InputOpen() {
        InBoundBarcodeDataTablesBody.buttons().enable();
        $('#AutoCompleteItemNumber').attr('disabled', false);
        $('#txtInputTransactionQty').attr('disabled', false);
        $('#txtRollReamWT').attr('disabled', false);
        $('#txtLotNumber').attr('disabled', false);
        $('#txtBARCODE').attr('disabled', false);
        $('#btnImportFile').attr('disabled', false);
        $('#btnImportFlatFile').attr('disabled', false);
        $('#btnExampleDownload').attr('disabled', false);
        $('#btnPrintPick').attr('disabled', false);
        $('#btnMultiLable').attr('disabled', false);
        $('#btnSaveBarcode').attr('disabled', false);
        $('#btnSaveTransfer').attr('disabled', false);
        $('#btnSaveStockTransferDT').attr('disabled', false);
    }

    function InputClose() {
        InBoundBarcodeDataTablesBody.buttons().disable();
        $('#AutoCompleteItemNumber').attr('disabled', true);
        $('#txtInputTransactionQty').attr('disabled', true);
        $('#txtRollReamWT').attr('disabled', true);
        $('#txtLotNumber').attr('disabled', true);
        $('#txtBARCODE').attr('disabled', true);
        $('#btnImportFile').attr('disabled', true);
        $('#btnImportFlatFile').attr('disabled', true);
        $('#btnExampleDownload').attr('disabled', true);
        $('#btnPrintPick').attr('disabled', true);
        $('#btnMultiLable').attr('disabled', true);
        $('#btnSaveBarcode').attr('disabled', true);
        $('#btnSaveTransfer').attr('disabled', true);
        $('#btnSaveStockTransferDT').attr('disabled', true);
    }


    $("#AutoCompleteItemNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/InboundAutoCompleteItemNumber",
                type: "POST",
                dataType: "json",
                data: {
                    InSubinventoryCode: $("#ddlInSubinventory").val(),
                    Locator: $("#ddlOutLocator").val(),
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Description, value: item.Value };
                    }))
                }
            })
        },
        //autoFocus: true,
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            if (ui.item) {
                GetStockItemData(ui.item.value);
            }
        }
    });

    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockTransaction/UpdateRemark',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var StockTransferBarcodeDTData = d.data;
                var StockTransferBarcodeDTList = [];
                var size = Object.keys(StockTransferBarcodeDTData).length;
                for (var i = 0; i < size; i++) {
                    var ID = Object.keys(StockTransferBarcodeDTData)[i];
                    var REMARK = Object.values(StockTransferBarcodeDTData[ID])[0];

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
        },
        table: "#InBoundBarcodeDataTablesBody",
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

            multi: {
                "title": "多欄位異動",
                "info": "請注意，您一次選擇多個不同的備註，此次異動將會變成同樣的備註！",
                "restore": "取消更改",
                "noMulti": "This input can be edited individually, but not part of a group."
            }
        }


    });


    mergeBacrodeEditor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockTransaction/MergeBarcode',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var StockTransferBarcodeDTData = d.data;
                var StockTransferBarcodeDTList = [];
                var size = Object.keys(StockTransferBarcodeDTData).length;
                for (var i = 0; i < size; i++) {
                    var ID = Object.keys(StockTransferBarcodeDTData)[i];
                    var BARCODE = Object.values(StockTransferBarcodeDTData[ID])[0];

                    var StockTransferBarcodeDT = {
                        'ID': ID,
                        'BARCODE': BARCODE
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
                    InBoundBarcodeDataTablesBody.ajax.reload();
                    swal.fire(data.result);
                }else{
                    swal.fire(data.result);
                }
            }
        },
        table: "#InBoundBarcodeDataTablesBody",
        idSrc: 'ID',
        fields: [
            {
                label: "被合併的條碼:",
                name: "MergeBarcode",
                type: 'text',
                def: ""

            }
        ],
        i18n: {
            edit: {
                button: "併板",
                title: "條碼合併",
                submit: "確定",
                'className': 'btn-danger'
            },

            multi: {
                "title": "多欄位異動",
                "info": "請注意，您一次選擇多個不同的條碼，此次異動將會合併成一個條碼！",
                "restore": "取消更改",
                "noMulti": "This input can be edited individually, but not part of a group."
            }
        }


    });

    var InBoundBarcodeDataTablesBody = $('#InBoundBarcodeDataTablesBody').DataTable({
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
            "url": "/StockTransaction/GetInboundStockTransferBarcodeDT",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.TransactionType = GetTransactionType();
                d.OUT_SUBINVENTORY_CODE = $("#ddlOutSubinventory").val();
                d.OUT_LOCATOR_ID = $("#ddlOutLocator").val();
                d.IN_SUBINVENTORY_CODE = $("#ddlInSubinventory").val();
                d.IN_LOCATOR_ID = $("#ddlInLocator").val();
                d.Number = $('#AutoCompleteShipmentNumber').val();
                d.NumberStatus = $("#NumberStatus").text();
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "StockTransferDT_ID", name: "項次", autoWidth: true },
            { data: "BARCODE", name: "條碼", autoWidth: true },
            { data: "Status", name: "入庫狀態", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "LOT_NUMBER", name: "捲號", autoWidth: true, className: "dt-body-left" },
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
            { data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }
        ],

        order: [[2, 'desc']],
        select: {
            style: 'multi',
            //blurable: true,
            //selector: 'td:first-child'
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                },
                //button: {
                //    tag: 'button',
                //    className: 'btn externalBtn'
                //}
            },
            buttons: [
                'selectAll',
                'selectNone',
                {
                    text: '刪除',
                    className: 'btn-danger',
                    action: function () {
                        var selectedData = InBoundBarcodeDataTablesBody.rows('.selected').data();
                        if (selectedData.length == 0) {
                            swal.fire("請選擇要刪除的條碼");
                            return;
                        }

                        swal.fire({
                            title: "條碼資料刪除",
                            text: "確定刪除嗎?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "確定",
                            cancelButtonText: "取消"
                        }).then(function (result) {
                            if (result.value) {
                                DeleteBarcode(selectedData);
                            }
                        });

                    }
                },
                {
                    text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                    //className: 'btn-default btn-sm',
                    action: function (e) {
                        PrintLable(InBoundBarcodeDataTablesBody, "/Home/GetLabel2", "2");
                    },
                    className: "btn-primary"
                },
                {
                    text: '編輯備註',
                    className: 'btn-danger',
                    action: function (e, dt, node, config) {
                        var count = dt.rows({ selected: true }).count();

                        if (count == 0) {
                            return;
                        }

                        //for (i = 0 ; i < count ; i++) {

                        //    if (dt.rows({ selected: true }).data().pluck('DELIVERY_STATUS')[i] == '已出貨') {
                        //        swal.fire('已出貨，無法再修改核准日');
                        //        return;
                        //    }
                        //    if (dt.rows({ selected: true }).data().pluck('DELIVERY_STATUS')[i] == '取消') {
                        //        swal.fire('已取消，無法再修改核准日');
                        //        return;
                        //    }
                        //}

                        editor.edit(InBoundBarcodeDataTablesBody.rows({ selected: true }).indexes())
                            .title('編輯備註')
                            .buttons({
                                text: '確定',
                                action: function () {
                                    this.submit();
                                },
                                className: 'btn-danger'
                            });
                    }
                },
                {
                    text: '併板',
                    className: 'btn-danger',
                    action: function (e, dt, node, config) {
                        var count = dt.rows({ selected: true }).count();

                        if (count == 0) {
                            return;
                        }

                        mergeBacrodeEditor.edit(InBoundBarcodeDataTablesBody.rows({ selected: true }).indexes())
                            .title('合併條碼')
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
    



    function checkTransactionType() {

        var InSubinventoryCode = $("#ddlInSubinventory").val();
        var OutSubinventoryCode = $("#ddlOutSubinventory").val();
        $.ajax({
            url: "/StockTransaction/CheckTransactionType",
            type: "post",
            data: {
                OutSubinventoryCode: OutSubinventoryCode,
                InSubinventoryCode: InSubinventoryCode
            },
            success: function (data) {
                if (data.status) {
                    
                    PickInputAreaHide();
                    $('#txtBARCODE').val("");
                    $('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                    //selectedNumber = "";
                    InBoundBarcodeDataTablesBody.ajax.reload();
                    
                    $('#NumberStatus').html('');
                    InputOpen();

                    if (data.result == "倉庫間移轉") {
                      
                        $('#TransactionType').html("移轉編號");
                        
                    } else {
                        
                        $('#TransactionType').html("出貨編號");
                      
                    }
                } else {
                    $('#TransactionType').html("編號");
                   
                }

            },
            error: function () {
                swal.fire('檢查移轉類別失敗');
            },
            complete: function (data) {


            }

        })



    }

    function GetTransactionType() {
        return $('#TransactionType').text();

        //if ($('#SHIPMENT_NUMBER_AREA').is(":visible")) {
        //    return "出貨編號";
        //} else if ($('#SUBINVENTORY_TRANSFER_NUMBER_AREA').is(":visible")) {
        //    return "移轉編號"
        //} else {
        //    return "";
        //}
    }

    function GetTransactionNumber() {
        //return selectedNumber;
        return $('#AutoCompleteShipmentNumber').val();

        //if ($('#SHIPMENT_NUMBER_AREA').is(":visible")) {
        //    return $('#txtShipmentNumber').val();
        //} else if ($('#SUBINVENTORY_TRANSFER_NUMBER_AREA').is(":visible")) {
        //    return $('#txtSubinventoryTransferNumber').val();
        //} else {
        //    return "";
        //}
    }

    function GetStockItemData(ITEM_NO) {

        $.ajax({
            url: "/StockTransaction/GetStockItemData",
            type: "post",
            data: {
                SUBINVENTORY_CODE: $('#ddlInSubinventory').val(),
                ITEM_NO: ITEM_NO
            },
            success: function (data) {
                if (data.STATUS) {
                    $('#QtyInputArea').show();

                    if (data.ITEM_CATEGORY == "平版") {
                        $('#PACKING_TYPE_LABEL').show();
                        $('#PACKING_TYPE').show();
                        $('#PACKING_TYPE').html(data.PACKING_TYPE);
                        $('#UNIT').html(data.UNIT);
                        $('#txtInputTransactionQty').focus();
                        $('#ROLL_INPUT_AREA').hide();
                        $('#REAM_INPUT_AREA').show();
                    } else {
                        $('#PACKING_TYPE_LABEL').hide();
                        $('#PACKING_TYPE').hide();
                        $('#PACKING_TYPE').html("");
                        $('#UNIT').html(data.UNIT);
                        $('#txtInputTransactionQty').focus();
                        $('#ROLL_INPUT_AREA').show();
                        $('#REAM_INPUT_AREA').hide();
                    }

                } else {
                    $('#QtyInputArea').hide();
                    $('#PACKING_TYPE_LABEL').hide();
                    $('#PACKING_TYPE').html("");
                    $('#PACKING_TYPE').hide();
                    $('#UNIT').html("");
                    $('#ROLL_INPUT_AREA').hide();
                    $('#REAM_INPUT_AREA').hide();
                }


            },
            error: function () {
                swal.fire('取得包裝方式失敗');
            },
            complete: function (data) {


            }

        });
    }

    function BarcodeInbound() {
       
        $.ajax({
            url: "/StockTransaction/BarcodeInbound",
            type: "post",
            data: {
                TransactionType: GetTransactionType(),
                Number: $('#AutoCompleteShipmentNumber').val(),
                BARCODE: $('#txtBARCODE').val()
            },
            success: function (data) {
                if (data.status) {
                    InBoundBarcodeDataTablesBody.ajax.reload();
                    $('#txtBARCODE').focus().select();
                } else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('條碼入庫失敗');
            },
            complete: function (data) {


            }

        });
    }

    function InBoundSaveTransfer() {
        var TransactionType = GetTransactionType();
        var Number = $('#AutoCompleteShipmentNumber').val();
        if (Number == "") {
            //swal.fire('請輸入編號');
            //event.preventDefault();
            //return false;
            if (TransactionType == "出貨編號") {
                swal.fire('請輸入出貨編號');
                event.preventDefault();
                return false;
            }
            if (TransactionType == "移轉編號") {
                swal.fire('請輸入移轉編號');
                event.preventDefault();
                return false;
            }
        }

        swal.fire({
            title: "入庫存檔",
            text: "確定入庫存檔嗎?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: "/StockTransaction/InBoundSaveTransfer",
                    type: "post",
                    data: {
                        TransactionType: TransactionType,
                        Number: Number
                    },
                    success: function (data) {
                        if (data.status) {
                           
                            swal.fire(data.result);
                            GetNumberStatus();
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

            }
        });


    }

    function GetRollReamWT(){
        if ($('#REAM_INPUT_AREA').is(":visible")) {
            return $('#txtRollReamWT').val();
        }else{
            return $('#txtInputTransactionQty').val();
        }
    }

    function GetLotNumber(){
        if ($('#ROLL_INPUT_AREA').is(":visible")) {
            return $('#txtLotNumber').val();
        }else{
            return "";
        }
    }


    function CheckCreateInboundBarcode() {

        if ($('#ddlOutSubinventory').val() == "請選擇") {
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
        if ($('#ddlInSubinventory').val() == "請選擇") {
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

        if ($('#ddlOutLocatorArea').is(":visible") && $('#ddlInLocatorArea').is(":visible")) {
            if ($('#ddlOutLocatorArea').val() == $('#ddlInLocatorArea').val()) {
                swal.fire('同倉庫儲位要不同');
                event.preventDefault();
                return;
            }
        }

        if ($('#ddlOutLocatorArea').is(":hidden ") && $('#ddlInLocatorArea').is(":hidden ")) {
            if ($('#ddlOutSubinventory').val() == $('#ddlInSubinventory').val()) {
                swal.fire('不可同倉庫無儲位移轉');
                event.preventDefault();
                return;
            }
        }

        if ($('#AutoCompleteShipmentNumber').val() == "") {
            swal.fire('請輸入編號');
            event.preventDefault();
            return;
        }

        if ($('#AutoCompleteItemNumber').is(":visible")) {
            if ($('#AutoCompleteItemNumber').val() == "") {
                swal.fire('請輸入料號');
                event.preventDefault();
                return;
            }
        }

        if ($('#txtInputTransactionQty').is(":visible")) {
            if ($('#txtInputTransactionQty').val() == "") {
                swal.fire('請輸入數量');
                event.preventDefault();
                return;
            }
        }

        if ($('#txtRollReamWT').is(":visible")) {
            if ($('#txtRollReamWT').val() == "") {
                swal.fire('請輸入每棧令數');
                event.preventDefault();
                return;
            }
        }
        
        if ($('#txtLotNumber').is(":visible")) {
            if ($('#txtLotNumber').val() == "") {
                swal.fire('請輸入捲數');
                event.preventDefault();
                return;
            }
        }
        

        $.ajax({
            url: "/StockTransaction/CheckNumber",
            type: "post",
            data: {
                TransactionType: GetTransactionType(),
                OUT_SUBINVENTORY_CODE: $('#ddlOutSubinventory').val(),
                OUT_LOCATOR_ID: $('#ddlOutLocator').val(),
                IN_SUBINVENTORY_CODE: $('#ddlInSubinventory').val(),
                IN_LOCATOR_ID: $('#ddlInLocator').val(),
                Number: $('#AutoCompleteShipmentNumber').val()
            },
            success: function (data) {
                if (data.status) {
                    if (data.result == "是否新增編號?") {
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
                                CreateInboundBarcode();

                            }
                        });
                    } else {
                        CreateInboundBarcode();

                    }


                } else {
                    swal.fire(data.result);
                }

            },
            error: function () {
                swal.fire('新增入庫單明細失敗');
            },
            complete: function (data) {

            }

        });
    }


    function CreateInboundBarcode() {
        //event.preventDefault();

        $.ajax({
            url: "/StockTransaction/CreateInboundBarcode",
            type: "post",
            data: {
                TransactionType: GetTransactionType(),
                OUT_SUBINVENTORY_CODE: $("#ddlOutSubinventory").val(),
                OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
                IN_SUBINVENTORY_CODE: $("#ddlInSubinventory").val(),
                IN_LOCATOR_ID: $("#ddlInLocator").val(),
                Number: $('#AutoCompleteShipmentNumber').val(),
                ITEM_NUMBER: $('#AutoCompleteItemNumber').val(),
                REQUESTED_QTY: $('#txtInputTransactionQty').val(),
                ROLL_REAM_WT: GetRollReamWT(),
                LOT_NUMBER: GetLotNumber()
            },
            success: function (data) {
                if (data.status) {
                    InBoundBarcodeDataTablesBody.ajax.reload();
                    ////$('#NumberStatus').html(data.result);
                    //InBoundBarcodeDataTablesBody.buttons().disable();
                    //$('#AutoCompleteItemNumber').attr('disabled', true);
                    //$('#txtInputTransactionQty').attr('disabled', true);
                    //$('#txtRollReamWT').attr('disabled', true);
                    //$('#txtLotNumber').attr('disabled', true);
                    //$('#txtBARCODE').attr('disabled', true);
                    //$('#btnImportFile').attr('disabled', true);
                    //$('#btnExampleDownload').attr('disabled', true);
                    //$('#btnPrintPick').attr('disabled', true);
                    //$('#btnMultiLable').attr('disabled', true);
                    //$('#btnSaveBarcode').attr('disabled', true);
                    //$('#btnSaveTransfer').attr('disabled', true);
                    //$('#btnSaveStockTransferDT').attr('disabled', true);
                    //swal.fire(data.result);
                } else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('產生明細失敗');
            },
            complete: function (data) {


            }

        });

    }

    function DeleteBarcode(selectedData) {


        var list = [];
        for (i = 0 ; i < selectedData.length; i++) {
            list.push(selectedData[i].ID);
        }


        $.ajax({
            url: "/StockTransaction/InboundDeleteBarcode",
            type: "post",
            data: {
                IDs: list
            },
            success: function (data) {
                if (data.status) {
                    InBoundBarcodeDataTablesBody.ajax.reload();
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


    //彈出dialog
    function Open(modal_dialog) {
        modal_dialog.modal({
            backdrop: "static",
            keyboard: true,
            show: true
        });

        modal_dialog.on('hidden.bs.modal', function (e) {
            $("div").remove(modal_dialog.selector);
        });

        modal_dialog.on('show.bs.modal', function (e) {
            $.validator.unobtrusive.parse('form');
        });


        modal_dialog.on('click', '#btnImport', function (e) {
            if (modal_dialog.selector == '#PaperRollModal') {
                ImportPaperRoll();
            } else if (modal_dialog.selector == '#FlatModal') {
                ImportFlat();
            }

        });

        modal_dialog.on('click', '#btnDailogPaperRoll', function (e) {
            
            var table = $('#ImportPaperRollTable').DataTable();

            var dd = table.rows().data().toArray();
            var StockTransferBarcodeDTList = [];
            $.each(dd, function (index, value) {
                var StockTransferBarcodeDT = {
                    'IN_SUBINVENTORY_CODE': value.Subinventory,
                    'IN_LOCATOR_ID': value.Locator,
                    'ITEM_NUMBER': value.ITEM_NUMBER,
                    'PRIMARY_QUANTITY': value.PRIMARY_QUANTITY,
                    'LOT_NUMBER': value.LOT_NUMBER
                }
                StockTransferBarcodeDTList.push(StockTransferBarcodeDT);
            });
            var data = {
                TransactionType: GetTransactionType(),
                OUT_SUBINVENTORY_CODE: $("#ddlOutSubinventory").val(),
                OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
                IN_SUBINVENTORY_CODE: $("#ddlInSubinventory").val(),
                IN_LOCATOR_ID: $("#ddlInLocator").val(),
                Number: $('#AutoCompleteShipmentNumber').val(),
                'StockTransferBarcodeDTList': StockTransferBarcodeDTList
            }
           
            var json = JSON.stringify(data);

            $.ajax({
                "url": "/StockTransaction/ImportRollInboundBarcode",
                "type": "POST",
                "datatype": "json",
                contentType: 'application/json',
                "data": json,
                success: function (data) {
                    if (data.status) {
                        GetNumberStatus();

                        //swal.fire(data.result);
                    } else {
                        swal.fire(data.result);
                    }


                },
                error: function (data) {
                    swal.fire(data);
                },
                complete: function (data) {
                    $('#PaperRollModal').modal('hide');

                }
            });

        });

        modal_dialog.on('click', '#BtnCancel', function () {
            $.ajax({
                url: '//'
            });
        });

        //modal_dialog.on('click', '#btnDailogPaperRoll', function () {
        //    $('#PaperRollModal').modal('hide');
        //    CreateInboundBarcode();
        //    $.ajax({


        //    });
        //});

        modal_dialog.on('click', '#btnDailogFlat', function (e) {
            
            var table = $('#ImportFlatTable').DataTable();

            var dd = table.rows().data().toArray();
            var StockTransferDTList = [];
            $.each(dd, function (index, value) {
                var StockTransferDT = {
                    'IN_SUBINVENTORY_CODE': value.Subinventory,
                    'IN_LOCATOR_ID': value.Locator,
                    'ITEM_NUMBER': value.ITEM_NUMBER,
                    'REQUESTED_QUANTITY': value.REQUESTED_QUANTITY,
                    'REQUESTED_QUANTITY2': value.REQUESTED_QUANTITY2,
                    'ROLL_REAM_WT': value.ROLL_REAM_WT,
                    'ROLL_REAM_QTY': value.ROLL_REAM_QTY
                }
                StockTransferDTList.push(StockTransferDT);
            });
            var data = {
                TransactionType: GetTransactionType(),
                OUT_SUBINVENTORY_CODE: $("#ddlOutSubinventory").val(),
                OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
                IN_SUBINVENTORY_CODE: $("#ddlInSubinventory").val(),
                IN_LOCATOR_ID: $("#ddlInLocator").val(),
                Number: $('#AutoCompleteShipmentNumber').val(),
                'StockTransferDTList': StockTransferDTList
            }

            var json = JSON.stringify(data);

            $.ajax({
                "url": "/StockTransaction/ImportFlatInboundBarcode",
                "type": "POST",
                "datatype": "json",
                contentType: 'application/json',
                "data": json,
                success: function (data) {
                    if (data.status) {
                        GetNumberStatus();

                        //swal.fire(data.result);
                    } else {
                        swal.fire(data.result);
                    }


                },
                error: function (data) {
                    swal.fire(data);
                },
                complete: function (data) {
                    $('#FlatModal').modal('hide');

                }
            });
        });


        modal_dialog.modal('show');

    }


    function ImportPaperRoll() {
        var fileInput = $('#file').get(0).files;
        var ddlInSubinventory = $('#ddlInSubinventory').val();
        var ddlInLocator = $('#ddlInLocator option:selected').text();
        var Number = $('#AutoCompleteShipmentNumber').val();
        var OUT_LOCATOR_ID = $("#ddlOutLocator").val();
        var IN_SUBINVENTORY_CODE = $("#ddlInSubinventory").val();
        var IN_LOCATOR_ID = $("#ddlInLocator").val();
        var TransactionType = $('#TransactionType').text();
        var OUT_SUBINVENTORY_CODE = $("#ddlOutSubinventory").val();
        var OUT_LOCATOR_ID = $("#ddlOutLocator").val();

        var formData = new FormData();
        if (fileInput.length > 0) {
            formData.append("file", fileInput[0]);
            formData.append("ddlInSubinventory", ddlInSubinventory);
            formData.append("ddlInLocator", ddlInLocator);
            formData.append("Number", Number);
            formData.append("OUT_LOCATOR_ID", OUT_LOCATOR_ID);
            formData.append("IN_SUBINVENTORY_CODE", IN_SUBINVENTORY_CODE);
            formData.append("IN_LOCATOR_ID", IN_LOCATOR_ID);
            formData.append("TransactionType", TransactionType);
            formData.append("OUT_SUBINVENTORY_CODE", OUT_SUBINVENTORY_CODE);
            formData.append("OUT_LOCATOR_ID", OUT_LOCATOR_ID);
        }


        $.ajax({
            "url": "/StockTransaction/UploadFileRoll",
            "type": "POST",
            "datatype": "json",
            "data": formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.result.Success) {
                    DialogPaperTable(data);
                } else {
                    swal.fire(data.result.Msg);
                }


            },
            error: function (data) {
                swal.fire(data);
            }
        });

    }

    function DialogPaperTable(data) {
        $('#ImportPaperRollTable').DataTable({
            "language": {
                "url": "/bower_components/datatables/language/zh-TW.json"
            },
            dom:
                "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            data: data.data,
            destroy: true,
            autoWidth: false,
            success: function () {

            },
            error: function () {
                $.swal.fire("失敗");
            },
            columns: [
                { data: "ID", "name": "項次", "autoWidth": true, "className": "dt-body-center" },
                { data: "Subinventory", "name": "倉庫", "autoWidth": true, "className": "dt-body-center" },
                { data: "Locator", "name": "儲位", "autoWidth": true, "className": "dt-body-center" },
                { data: "ITEM_NUMBER", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
                { data: "PAPERTYPE", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
                { data: "Base_Weight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
                { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center" },
                { data: "PRIMARY_QUANTITY", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
                { data: "PRIMARY_UOM", "name": "單位", "autoWidth": true, "className": "dt-body-center" },
                { data: "LOT_NUMBER", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },

            ]
        });
    }


    function ImportFlat() {
        var fileInput = $('#file').get(0).files;
        var ddlInSubinventory = $('#ddlInSubinventory').val();
        var ddlInLocator = $('#ddlInLocator option:selected').text();
        var Number = $('#AutoCompleteShipmentNumber').val();
        var OUT_LOCATOR_ID = $("#ddlOutLocator").val();
        var IN_SUBINVENTORY_CODE = $("#ddlInSubinventory").val();
        var IN_LOCATOR_ID = $("#ddlInLocator").val();
        var TransactionType = $('#TransactionType').text();
        var OUT_SUBINVENTORY_CODE = $("#ddlOutSubinventory").val();
        var OUT_LOCATOR_ID = $("#ddlOutLocator").val();
        var formData = new FormData();
        if (fileInput.length > 0) {
            formData.append("file", fileInput[0]);
            formData.append("ddlInSubinventory", ddlInSubinventory);
            formData.append("ddlInLocator", ddlInLocator);
            formData.append("Number", Number);
            formData.append("OUT_LOCATOR_ID", OUT_LOCATOR_ID);
            formData.append("IN_SUBINVENTORY_CODE", IN_SUBINVENTORY_CODE);
            formData.append("IN_LOCATOR_ID", IN_LOCATOR_ID);
            formData.append("TransactionType", TransactionType);
            formData.append("OUT_SUBINVENTORY_CODE", OUT_SUBINVENTORY_CODE);
            formData.append("OUT_LOCATOR_ID", OUT_LOCATOR_ID);
        }


        $.ajax({
            "url": "/StockTransaction/UploadFileFlat",
            "type": "POST",
            "datatype": "json",
            "data": formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.result.Success) {
                    DialogFaltTable(data);
                } else {
                    swal.fire(data.result.Msg);
                }


            },
            error: function (data) {
                swal.fire(data);
            }
        });
    }


    function DialogFaltTable(data) {
        $('#ImportFlatTable').DataTable({
            "language": {
                "url": "/bower_components/datatables/language/zh-TW.json"
            },
            dom:
                "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            data: data.data,
            destroy: true,
            autoWidth: false,
            processing: true,
            error: function () {
                $.swal.fire("失敗");
            },
            columns: [
                { data: "ID", "name": "項次", "autoWidth": true },
                { data: "IN_SUBINVENTORY_CODE", "name": "倉別", "autoWidth": true },
                { data: "IN_LOCATOR_ID", "name": "儲位", "autoWidth": true },
                { data: "ITEM_NUMBER", "name": "料號", "autoWidth": true },
                { data: "PACKING_TYPE", "name": "包裝方式", "autoWidth": true },
                { data: "ROLL_REAM_QTY", "name": "棧板數", "autoWidth": true },
                { data: "ROLL_REAM_WT", "name": "每件令數", "autoWidth": true },
                { data: "REQUESTED_QUANTITY2", "name": "總令數", "autoWidth": true },
                { data: "REQUESTED_QUANTITY", "name": "數量(頓)", "autoWidth": true },
            ]
        });
    }
}



