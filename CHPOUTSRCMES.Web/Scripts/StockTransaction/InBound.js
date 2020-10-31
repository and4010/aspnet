function InBoundInit() {
    var selected = [];
    var editor;
    var selectedNumber;
    var mergeBacrodeEditor;
    var selectedNumberStatus = "0";
    var selectedTransferHeaderId = 0;
    

    function getShipmentNumber() {
        if ($("#chkCustomShipmentNumber").prop("checked")) {
            return $('#txtShipmentNumber').val();
        } else {
            return $('#ddlShipmentNumber option:selected').text();
        }
        
    }

    function getTransferHeaderId() {
        //return $('#ddlShipmentNumber').val();
        return $('#TransferHeaderId').text();
    }

    function getOutOrganizationId() {
        //return $("#ddlOutSubinventory").val();
        return $("#ddlOutOrganization").val();
    }

    function getInOrganizationId() {
        //return $("#ddlInSubinventory").val();
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

    $("#chkCustomShipmentNumber").click(function () {
        if ($("#chkCustomShipmentNumber").prop("checked")) {
            $(".custom-combobox").hide(100);
            $("#txtShipmentNumber").show();
        } else {
            $(".custom-combobox").show(100);
            $("#txtShipmentNumber").hide();
        }
    });


    $('#ddlOutOrganization').change(function () {

        $.ajax({
            url: "/StockTransaction/GetSubinventoryList",
            type: "post",
            data: {
                ORGANIZATION_ID: getOutOrganizationId()
            },
            success: function (data) {
                $('#ddlOutSubinventory').html("");

                for (var i = 0; i < data.length; i++) {
                    $('#ddlOutSubinventory').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }

                if (getOutOrganizationId() != getInOrganizationId()) {
                    $('#chkCustomShipmentNumber').show();
                    $('#lblCustomShipmentNumber').show();
                } else {
                    $('#chkCustomShipmentNumber').hide();
                    $('#lblCustomShipmentNumber').hide();
                    $("#chkCustomShipmentNumber").prop("checked", false);
                    $(".custom-combobox").show(100);
                    $("#txtShipmentNumber").hide();
                }

            },
            error: function () {
                swal.fire('更新發貨組織失敗');
            },
            complete: function () {
                
            }

        });

    })

    $('#ddlInOrganization').change(function () {

        $.ajax({
            url: "/StockTransaction/GetSubinventoryListForUserId",
            type: "post",
            data: {
                ORGANIZATION_ID: getInOrganizationId()
            },
            success: function (data) {
                $('#ddlInSubinventory').html("");

                for (var i = 0; i < data.length; i++) {
                    $('#ddlInSubinventory').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }

                if (getOutOrganizationId() != getInOrganizationId()) {
                    $('#chkCustomShipmentNumber').show();
                    $('#lblCustomShipmentNumber').show();
                } else {
                    $('#chkCustomShipmentNumber').hide();
                    $('#lblCustomShipmentNumber').hide();
                    $("#chkCustomShipmentNumber").prop("checked", false);
                    $(".custom-combobox").show(100);
                    $("#txtShipmentNumber").hide();
                }

            },
            error: function () {
                swal.fire('更新收貨組織失敗');
            },
            complete: function () {
                
            }

        });

    })

    $('#ddlOutSubinventory').change(function () {

        var SUBINVENTORY_CODE = getOutSubinventoryCode();
        $.ajax({
            url: "/StockTransaction/GetLocatorList",
            type: "post",
            data: {
                ORGANIZATION_ID: "*",
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
                //checkTransactionType();
                GetShipmentNumberList("新增編號", "新增編號");
                $('#AutoCompleteItemNumber').val("");
                $('#PACKING_TYPE_LABEL').hide();
                $('#PACKING_TYPE').html("");
                $('#PACKING_TYPE').hide();
                $('#UNIT').html("");
            }

        })


    })



    $('#ddlInSubinventory').change(function () {

        var SUBINVENTORY_CODE = getInSubinventoryCode();
        $.ajax({
            url: "/StockTransaction/GetLocatorListForUserId",
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
                //checkTransactionType();
                GetShipmentNumberList("新增編號", "新增編號");
            }

        })


    })


    $('#ddlOutLocator').change(function () {
        //GetShipmentNumberList("新增編號", "新增編號");

        //checkTransactionType();
        //$('#AutoCompleteItemNumber').val("");
        //$('#PACKING_TYPE_LABEL').hide();
        //$('#PACKING_TYPE').html("");
        //$('#PACKING_TYPE').hide();
        //$('#UNIT').html("");
        //GetItemNumberList();
    })

    $('#ddlInLocator').change(function () {
        //GetShipmentNumberList("新增編號", "新增編號");

        //checkTransactionType();
    })

    $("#ddlShipmentNumber").combobox({
        select: function (event, ui) {
            SelectShipmentNumber();
            //if (getShipmentNumber() == "新增編號") {
            //    $("#scrollbox").collapse('show');
            //    $('#AutoCompleteItemNumber').focus();
            //} else {
            //    GetShipmentNumberData();
            //}
            //GetNumberStatus();

            //GetStockItemData(this.value);
        }
    });

    $('.custom-combobox-input').css('width', '200px');
    $('#btnImportFile').css('margin-left', '28px');

    $('#txtInputTransactionQty').keydown(function (e) {
        if (e.keyCode == 13) {
            if ($('#REAM_INPUT_AREA').is(":visible")) {
                $('#txtRollReamWT').focus();
            } else if ($('#ROLL_INPUT_AREA').is(":visible")) {
                $('#txtLotNumber').focus();
            }
        }
    });

    //checkTransactionType();

    $("#btnExampleDownload").click(function () {
        window.open('/StockTransaction/DownloadExcelSampleFile', '_blank');
    });

    $("#btnExampleDownload2").click(function () {
        window.open('/StockTransaction/DownloadExcelSampleFile2', '_blank');
    });

    $("#btnImportFile").click(function () {
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

        //if ($('#ddlOutLocatorArea').is(":visible") && $('#ddlInLocatorArea').is(":visible")) {
        //    if (getOutLocatorId() == getInLocatorId()) {
        //        swal.fire('同倉庫儲位要不同');
        //        event.preventDefault();
        //        return;
        //    }
        //}

        //if ($('#AutoCompleteShipmentNumber').val().trim() == "") {
        //    swal.fire("請輸入編號");
        //    event.preventDefault();
        //    return;
        //}

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

        //if ($('#ddlOutLocatorArea').is(":visible") && $('#ddlInLocatorArea').is(":visible")) {
        //    if (getOutLocatorId() == getInLocatorId()) {
        //        swal.fire('同倉庫儲位要不同');
        //        event.preventDefault();
        //        return;
        //    }
        //}

        //if ($('#AutoCompleteShipmentNumber').val().trim() == "") {
        //    swal.fire("請輸入編號");
        //    event.preventDefault();
        //    return;
        //}

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


    $("#btnPrintRollPick").click(function () {
        var shipmentNumber = getShipmentNumber();
        if (shipmentNumber == "新增編號") {
            swal.fire('請選擇出貨編號');
            event.preventDefault();
            return;
        }
        window.open("/StockTransaction/InboundRollPickingReport/?shipmentNumber=" + shipmentNumber);
    });

    $("#btnPrintFlatPick").click(function () {
        var shipmentNumber = getShipmentNumber();
        if (shipmentNumber == "新增編號") {
            swal.fire('請選擇出貨編號');
            event.preventDefault();
            return;
        }
        window.open("/StockTransaction/InboundFlatPickingReport/?shipmentNumber=" + shipmentNumber);
    });

    //$(".custom-combobox").keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        SelectShipmentNumber();
    //    }
    //});

    //$('#txtShipmentNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        SelectShipmentNumber();
    //    }
    //});

    //離開自訂編號欄位自動選擇自訂編號
    $('#txtShipmentNumber').blur(function () {
        SelectShipmentNumber();
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

    if ($('#btnSaveTransfer2').is(":visible")) {
        $("#btnSaveTransfer2").click(function () {
            InBoundSaveTransferNoCheckStockStatus();
        });
    }

    $("#btnSaveStockTransferDT").click(function () {
        CheckCreateInboundBarcode();
    });

    //$('#AutoCompleteItemNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        $(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
    //        $('#txtInputTransactionQty').focus();
    //    }
    //});




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




    //$("#AutoCompleteShipmentNumber").autocomplete({
    //    source: function (request, response) {
    //        $.ajax({
    //            url: "/StockTransaction/InboundAutoCompleteShipmentNumber",
    //            type: "POST",
    //            dataType: "json",
    //            data: {
    //                TransactionType: GetTransactionType(),
    //                OutSubinventoryCode: getOutSubinventoryCode() ,
    //                OutLocator: $("#ddlOutLocator").val(),
    //                InSubinventoryCode: getInSubinventoryCode(),
    //                InLocator: $("#ddlInLocator").val(),
    //                Prefix: request.term
    //            },
    //            success: function (data) {
    //                response($.map(data, function (item) {
    //                    return { label: item.Description, value: item.Value };
    //                }))
    //            }
    //        })
    //    },
    //    //autoFocus: true,
    //    messages: {
    //        noResults: "", results: ""
    //    },
    //    select: function (event, ui) {
    //        if (ui.item) {
    //            //selectedNumber = ui.item.value;
    //            GetNumberStatus();
    //        }
    //    }
    //});

    function SelectShipmentNumber() {
        //var transferHeaderId = 0;
        //if (getShipmentNumber() == "新增編號") {
        //    transferHeaderId = 0;
        //} else {
        //    transferHeaderId = getTransferHeaderId();
        //}

        $.ajax({
            //url: "/StockTransaction/GetShipmentNumberData",
            url: "/StockTransaction/GetShipmentNumberDataForShipmentNumber",
            type: "post",
            data: {
                //transferHeaderId: transferHeaderId
                shipmentNumber : getShipmentNumber()
            },
            success: function (data) {
                if (data.Success) {
                    if (data.Msg == "新增編號") {
                        InputOpen();
                        $('#txtBARCODE').val("");
                        selectedTransferHeaderId = 0;
                        selectedNumberStatus = "0";
                        InBoundBarcodeDataTablesBody.ajax.reload();
                        $("#scrollbox").collapse('show');
                        $('#AutoCompleteItemNumber').focus();
                        return;
                    }



                     
                        //出貨編號已存檔
                        if (data.Data.IsMes == "1" && data.Data.TransferType == "O" && data.Data.NumberStatus == "1") {
                            //對方是MES出庫
                            swal.fire({
                                title: "出庫轉入庫",
                                text: "是否匯入MES出庫資料?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "確定",
                                cancelButtonText: "取消"
                            }).then(function (result) {
                                if (result.value) {
                                    selectedTransferHeaderId = data.Data.TransferHeaderId;
                                    selectedNumberStatus = data.Data.NumberStatus;

                                    if ($('#ddlOutLocatorArea').is(":visible")) {
                                        $("#ddlOutLocator").val(data.Data.LocatorId);
                                    }

                                    if ($('#ddlInLocatorArea').is(":visible")) {
                                        $("#ddlInLocator").val(data.Data.TransferLocatorId);
                                    }


                                    $('#txtBARCODE').val("");
                                    OutBoundToInbound();
                                } else {
                                    InputOpen();
                                    $("#ddlShipmentNumber").combobox('autocomplete', "新增編號", "新增編號");
                                    selectedTransferHeaderId = 0;
                                    selectedNumberStatus = "0";
                                    InBoundBarcodeDataTablesBody.ajax.reload();
                                    $("#scrollbox").collapse('show');
                                    $('#AutoCompleteItemNumber').focus();
                                }
                            });

                        } else {
                            //非MES入庫 已存檔 
                            selectedTransferHeaderId = data.Data.TransferHeaderId;
                            selectedNumberStatus = data.Data.NumberStatus;

                            if ($('#ddlOutLocatorArea').is(":visible")) {
                                $("#ddlOutLocator").val(data.Data.LocatorId);
                            }

                            if ($('#ddlInLocatorArea').is(":visible")) {
                                $("#ddlInLocator").val(data.Data.TransferLocatorId);
                            }


                            $('#txtBARCODE').val("");
                            InBoundBarcodeDataTablesBody.ajax.reload();
                            if (data.Data.NumberStatus == "1") {
                                InputClose();
                            } else {
                                InputOpen();
                            }
                            
                            
                        }


                    

                } else {
                    InputOpen();
                    $('#txtBARCODE').val("");
                    selectedTransferHeaderId = 0;
                    selectedNumberStatus = "0";
                    InBoundBarcodeDataTablesBody.ajax.reload();
                    $("#scrollbox").collapse('show');
                    $('#AutoCompleteItemNumber').focus();
                    //swal.fire(data.Msg);
                }


            },
            error: function () {
                swal.fire('取得出貨編號資料失敗');
            },
            complete: function (data) {


            }

        });
    }

    function GetNumberStatus() {
        $.ajax({
            url: "/StockTransaction/GetNumberStatus",
            type: "POST",
            dataType: "json",
            data: {
                TransactionType: GetTransactionType(),
                OUT_SUBINVENTORY_CODE: getOutSubinventoryCode(),
                OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
                IN_SUBINVENTORY_CODE: getInSubinventoryCode(),
                IN_LOCATOR_ID: $("#ddlInLocator").val(),
                Number: $('#ddlShipmentNumber').val()
                //Number: $('#AutoCompleteShipmentNumber').val()
            },
            success: function (data) {
                if (data.status) {

                    $('#NumberStatus').html(data.result);
                    //PickInputAreaHide();
                    InputOpen();
                    $('#txtBARCODE').val("");
                    InBoundBarcodeDataTablesBody.ajax.reload();

                    if (data.result == "MES未出庫") {
                        swal.fire("編號" + $('#ddlShipmentNumber').val() + "尚未出庫存檔");
                        //swal.fire("編號" + $('#AutoCompleteShipmentNumber').val() + "尚未出庫存檔");
                        //$('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                        //selectedNumber = "";
                    } else if (data.result == "MES已出庫") {
                        $('#txtBARCODE').focus();
                    } else if (data.result == "非MES入庫手動新增") {
                        //$('#PickInputArea').show();
                        $('#AutoCompleteItemNumber').focus();
                    } else if (data.result == "非MES入庫檔案匯入") {
                        $('#txtBARCODE').focus();
                    } else if (data.result == "非MES已入庫") {
                        InputClose();
                    } else if (data.result == "MES已入庫") {
                        InputClose();
                    } else {
                        //為新增編號 此編號還未儲存
                        //$('#PickInputArea').show();
                        $("#scrollbox").collapse('show');
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

    function OutBoundToInbound() {
        $.ajax({
            url: "/StockTransaction/OutBoundToInbound",
            type: "POST",
            data: {
                transferHeaderId: selectedTransferHeaderId
            },
            success: function (data) {
                if (data.Success) {
                    GetShipmentNumberList(data.Data.TransferHeaderId, data.Data.ShipmentNumber);
                    //$("#ddlShipmentNumber").combobox('autocomplete', data.Data.TransferHeaderId, data.Data.ShipmentNumber);
                    //selectedTransferHeaderId = data.Data.TransferHeaderId;
                    //selectedNumberStatus = data.Data.NumberStatus;

                    //if ($('#ddlOutLocatorArea').is(":visible")) {
                    //    $("#ddlOutLocator").val(data.Data.LocatorId);
                    //}

                    //if ($('#ddlInLocatorArea').is(":visible")) {
                    //    $("#ddlInLocator").val(data.Data.TransferLocatorId);
                    //}
                    //$('#txtBARCODE').val("");
                    //InBoundBarcodeDataTablesBody.ajax.reload();
                    //InputOpen();

                } else {
                    swal.fire(data.Msg);
                }
            },
            error: function () {
                swal.fire('匯入MES出庫資料失敗');
            }
        })
    }


    function PickInputAreaHide() {
        $('#PickInputArea').hide();

        $('#AutoCompleteItemNumber').autocomplete('close').val('');
        $('#PACKING_TYPE').html("");

        //QtyInputAreaHide();
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
        $('#btnExampleDownload2').attr('disabled', false);
        $('#btnPrintRollPick').attr('disabled', false);
        $('#btnPrintFlatPick').attr('disabled', false);
        $('#btnMultiLable').attr('disabled', false);
        $('#btnSaveBarcode').attr('disabled', false);
        $('#btnSaveTransfer').attr('disabled', false);
        $('#btnSaveStockTransferDT').attr('disabled', false);
        $('#btnSaveTransfer2').attr('disabled', false);
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
        $('#btnExampleDownload2').attr('disabled', true);
        $('#btnPrintRollPick').attr('disabled', true);
        $('#btnPrintFlatPick').attr('disabled', true);
        $('#btnMultiLable').attr('disabled', true);
        $('#btnSaveBarcode').attr('disabled', true);
        $('#btnSaveTransfer').attr('disabled', true);
        $('#btnSaveStockTransferDT').attr('disabled', true);
        $('#btnSaveTransfer2').attr('disabled', true);
    }


    $("#AutoCompleteItemNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/AutoCompleteItemNumber",
                type: "POST",
                dataType: "json",
                data: {
                    //InSubinventoryCode: getInSubinventoryCode(),
                    //Locator: $("#ddlOutLocator").val(),
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
                //GetStockItemData(ui.item.value);
            }
        }
    });

    //$("#AutoCompleteItemNumber").bind("input propertychange", function () {
    //$('#AutoCompleteItemNumber').bind('paste', function (e) {
    //    setTimeout(function () {
    //        //$("#AutoCompleteItemNumber").autocomplete("search", $("#AutoCompleteItemNumber").val());
    //        //$("#AutoCompleteItemNumber").data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
    //        $('#AutoCompleteItemNumber').autocomplete("search", $('#AutoCompleteItemNumber').val());
    //    }, 0);
    //});

    //離開料號欄位自動搜尋
    $('#AutoCompleteItemNumber').blur(function () {
        GetStockItemData($('#AutoCompleteItemNumber').val(), false);
    });

    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/StockTransaction/InboundPickEditor',
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

                    InBoundBarcodeDataTablesBody.ajax.reload();
                }
                else {
                    swal.fire(data.result);
                }
            }
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
                    //var BARCODE = Object.values(StockTransferBarcodeDTData[ID])[0];
                    var BARCODE = Object.keys(StockTransferBarcodeDTData[ID]).map(function (e) {
                        return StockTransferBarcodeDTData[ID][e]
                    })[0];

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
                } else {
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
                d.transferHeaderId = selectedTransferHeaderId;
                d.numberStatus = selectedNumberStatus;
                //d.TransactionType = GetTransactionType();
                //d.OUT_SUBINVENTORY_CODE = getOutSubinventoryCode() ;
                //d.OUT_LOCATOR_ID = $("#ddlOutLocator").val();
                //d.IN_SUBINVENTORY_CODE = getInSubinventoryCode();
                //d.IN_LOCATOR_ID = $("#ddlInLocator").val();
                //d.Number = $('#ddlShipmentNumber').val();
                ////d.Number = $('#AutoCompleteShipmentNumber').val();
                //d.NumberStatus = $("#NumberStatus").text();
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
            //{ data: "TransferDetailId", name: "項次", autoWidth: true },
            { data: "BARCODE", name: "條碼", autoWidth: true },
            {
                data: "Status", name: "入庫狀態", autoWidth: true, "mRender": function (data, type, full) {
                    if (data != null) {
                        if (data == '0') {
                            return '待列印';
                        } else if (data == '1') {
                            return '待入庫';
                        } else if (data == '2') {
                            return '已入庫';
                        } else {
                            return '';
                        }
                    } else {
                        return '';
                    }
                }
            },
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
            { data: "ID", name: "ID", autoWidth: true, visible: false },
            //{ data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }
        ],

        order: [[1, 'desc']],
        select: {
            style: 'multi',
            //blurable: true,
            //selector: 'td:first-child'
        },
        //"rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {
        //    if ($.inArray(data.ID, selected) !== -1) {
        //        var selectRow = ':eq(' + dataIndex + ')';
        //        InBoundBarcodeDataTablesBody.row(selectRow, { page: 'current' }).select();
        //    }
        //},

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
                //{
                //    extend: "remove",
                //    className: 'btn-danger',
                //    editor: editor
                //},
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
                        var rows = InBoundBarcodeDataTablesBody.rows({ selected: true }).indexes();

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
                //        var selectedData = InBoundBarcodeDataTablesBody.rows('.selected').data();
                //        if (selectedData.length == 0) {
                //            swal.fire("請選擇要刪除的條碼");
                //            return;
                //        }

                //        swal.fire({
                //            title: "條碼資料刪除",
                //            text: "確定刪除嗎?",
                //            type: "warning",
                //            showCancelButton: true,
                //            confirmButtonColor: "#DD6B55",
                //            confirmButtonText: "確定",
                //            cancelButtonText: "取消"
                //        }).then(function (result) {
                //            if (result.value) {
                //                DeleteBarcode(selectedData);
                //            }
                //        });

                //    }
                //},
                {
                    text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                    //className: 'btn-default btn-sm',
                    action: function (e) {
                        var data = InBoundBarcodeDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }
                        var barcode = [];
                        var transferPickedIdList = [];
                        for (var i = 0; i < data.length; i++) {
                            transferPickedIdList.push(data[i].ID);
                            if (data[i].PALLET_STATUS == '2') { //是否為併板
                                barcode.push(data[i].BARCODE);
                            }
                        }
                        if (barcode.length > 0) {
                            swal.fire({
                                title: "注意",
                                html: "以下為併板後的條碼，請更換庫存棧板上的舊條碼。<br>" + barcode.join('<br>'),
                                type: "warning",
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "確定",
                            }).then(function (result) {
                                if (result.value) {
                                    printInboundLabel(transferPickedIdList)
                                }
                            });
                        } else {
                            printInboundLabel(transferPickedIdList)
                        }
                    },
                    className: "btn-primary",
                    enabled: false,
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                //{
                //    text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                //    //className: 'btn-default btn-sm',
                //    action: function (e) {
                //        PrintLable(InBoundBarcodeDataTablesBody, "/Home/GetLabel2", "2");
                //        InBoundBarcodeDataTablesBody.ajax.reload();
                //    },
                //    className: "btn-primary"
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
                //{
                //    text: '併板',
                //    className: 'btn-danger',
                //    action: function (e, dt, node, config) {
                //        var count = dt.rows({ selected: true }).count();

                //        if (count == 0) {
                //            return;
                //        }

                //        mergeBacrodeEditor.edit(InBoundBarcodeDataTablesBody.rows({ selected: true }).indexes())
                //            .title('合併條碼')
                //            .buttons({
                //                text: '確定',
                //                action: function () {
                //                    this.submit();
                //                },
                //                className: 'btn-danger'
                //            });
                //    }
                //},
                {
                    text: '併板',
                    className: 'btn-danger',
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    action: function (e, dt, node, config) {
                        //var count = dt.rows({ selected: true }).count();

                        //if (count == 0) {
                        //    return;
                        //}
                        var selectedData = InBoundBarcodeDataTablesBody.rows('.selected').data();
                        if (selectedData.length == 0) {
                            swal.fire("請選擇待併板的條碼");
                            return;
                        }
                        var list = [];
                        for (i = 0; i < selectedData.length; i++) {
                            list.push(selectedData[i].ID);
                            if (selectedData[i].PACKING_TYPE != "令包") {
                                swal.fire("包裝方式為令包才可以併板");
                                return;
                            }
                            //if (selectedData[i].Status != "已入庫") {
                            //    swal.fire("狀態為已入庫才可以併板");
                            //    return;
                            //}
                        }
                        $.ajax({
                            url: '/StockTransaction/MergeBarcodeDialog/',
                            type: "POST",
                            data: {
                                IDs: list
                            },
                            success: function (result) {
                                $('body').append(result);
                                OpenMergeBarcodeDialog($('#MergeBarcodeModal'));
                            },
                            error: function () {
                                swal.fire("併板失敗");
                            }
                        });




                    }
                }
            ],
        }
    });

    //InBoundBarcodeDataTablesBody.on('select', function (e, dt, type, indexes) {
    //    if (type === 'row') {
    //        var ID = dt.rows(indexes).data().pluck('ID')[0];
    //        var index = $.inArray(ID, selected);
    //        if (index === -1) {
    //            selected.push(ID);
    //        }
    //    }
    //});

    //InBoundBarcodeDataTablesBody.on('deselect', function (e, dt, type, indexes) {
    //    if (type === 'row') {

    //        var ID = dt.rows(indexes).data().pluck('ID')[0];
    //        var index = $.inArray(ID, selected);
    //        selected.splice(index, 1);
    //    }
    //});

    function printInboundLabel(transferPickedIdList) {

        $.ajax({
            url: "/StockTransaction/WaitPrintToWaitInbound",
            type: "post",
            data: {
                transferPickedIdList: transferPickedIdList
            },
            success: function (data) {
                if (data.status) {
                    InBoundBarcodeDataTablesBody.ajax.reload(null, false);
                    PrintLable(InBoundBarcodeDataTablesBody, "/StockTransaction/PrintInboundLabel", "12");
                } else {
                    swal.fire(data.result);
                }

            },
            error: function () {
                swal.fire('列印標籤失敗');
            },
            complete: function (data) {


            }

        });
    }


    function checkTransactionType() {

        var InSubinventoryCode = getInSubinventoryCode();
        var OutSubinventoryCode = getOutSubinventoryCode();
        $.ajax({
            url: "/StockTransaction/CheckTransactionType",
            type: "post",
            data: {
                OutSubinventoryCode: OutSubinventoryCode,
                InSubinventoryCode: InSubinventoryCode
            },
            success: function (data) {
                if (data.status) {

                    //PickInputAreaHide();
                    $('#txtBARCODE').val("");
                    //$('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                    //selectedNumber = "";
                    InBoundBarcodeDataTablesBody.ajax.reload();

                    $('#NumberStatus').html('');
                    InputOpen();

                    if (data.result == "倉庫間移轉") {
                        GetSubinventoryTransferNumberList("新增編號", "新增編號");
                        $('#TransactionType').html("移轉編號");

                    } else {
                        GetShipmentNumberList("新增編號", "新增編號");
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

        });



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
        //return $('#AutoCompleteShipmentNumber').val();
        return $('#ddlShipmentNumber').val();
        //if ($('#SHIPMENT_NUMBER_AREA').is(":visible")) {
        //    return $('#txtShipmentNumber').val();
        //} else if ($('#SUBINVENTORY_TRANSFER_NUMBER_AREA').is(":visible")) {
        //    return $('#txtSubinventoryTransferNumber').val();
        //} else {
        //    return "";
        //}
    }

    function GetStockItemData(ITEM_NO, focusNext) {

        $.ajax({
            url: "/StockTransaction/GetStockItemData",
            type: "post",
            data: {
                SUBINVENTORY_CODE: getInSubinventoryCode(),
                ITEM_NO: ITEM_NO
            },
            success: function (data) {
                if (data.Success) {
                    //$('#QtyInputArea').show();

                    if (data.Data.CatalogElemVal070 == "平版") {
                        $('#PACKING_TYPE_LABEL').show();
                        $('#PACKING_TYPE').show();
                        $('#PACKING_TYPE').html(data.Data.CatalogElemVal110);
                        $('#UNIT').html(data.Data.SecondaryUomCode);
                        if (focusNext) {
                            $('#txtInputTransactionQty').focus();
                        }
                        $('#ROLL_INPUT_AREA').hide();
                        $('#REAM_INPUT_AREA').show();
                    } else {
                        $('#PACKING_TYPE_LABEL').hide();
                        $('#PACKING_TYPE').hide();
                        $('#PACKING_TYPE').html("");
                        $('#UNIT').html(data.Data.PrimaryUomCode);
                        if (focusNext) {
                            $('#txtInputTransactionQty').focus();
                        }
                        $('#ROLL_INPUT_AREA').show();
                        $('#REAM_INPUT_AREA').hide();
                    }

                } else {
                    //$('#QtyInputArea').hide();
                    $('#PACKING_TYPE_LABEL').hide();
                    $('#PACKING_TYPE').html("");
                    $('#PACKING_TYPE').hide();
                    $('#UNIT').html("");
                    $('#ROLL_INPUT_AREA').hide();
                    $('#REAM_INPUT_AREA').hide();
                    //swal.fire(data.Msg);
                }


            },
            error: function () {
                swal.fire('取得料號資料失敗');
            },
            complete: function (data) {


            }

        });
    }

    function BarcodeInbound() {

        var barcode = $('#txtBARCODE').val();
        if (!barcode) {
            swal.fire('請輸入條碼');
            event.preventDefault();
            return;
        }

        $.ajax({
            //url: "/StockTransaction/BarcodeInbound",
            url: "/StockTransaction/BarcodeInboundForShipmentNumber",
            type: "post",
            data: {

                //transferHeaderId: getTransferHeaderId(),
                shipmentNumber: getShipmentNumber(),
                barcode: $('#txtBARCODE').val()
            },
            success: function (data) {
                if (data.status) {
                    InBoundBarcodeDataTablesBody.ajax.reload(null, false);
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
        if (getShipmentNumber() == "新增編號") {
            swal.fire('請選擇出貨編號');
            event.preventDefault();
            return;
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
                    //url: "/StockTransaction/InBoundSaveTransfer",
                    url: "/StockTransaction/InBoundSaveTransferForShipmentNumber",
                    type: "post",
                    data: {
                        //transferHeaderId: getTransferHeaderId()
                        shipmentNumber: getShipmentNumber()
                    },
                    success: function (data) {
                        if (data.status) {

                            swal.fire(data.result);
                            SelectShipmentNumber();
                            //GetNumberStatus();
                        } else {
                            swal.fire(data.result);
                        }
                    },
                    error: function () {
                        swal.fire('入庫存檔失敗');
                    },
                    complete: function (data) {


                    }

                });

            }
        });


    }

    function InBoundSaveTransferNoCheckStockStatus() {

        swal.fire({
            title: "直接入庫存檔",
            text: "確定直接入庫存檔嗎?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    //url: "/StockTransaction/InBoundSaveTransferNoCheckStockStatus",
                    url: "/StockTransaction/InBoundSaveTransferNoCheckStockStatusForShipmentNumber",
                    type: "post",
                    data: {
                        //transferHeaderId: getTransferHeaderId()
                        shipmentNumber: getShipmentNumber()
                    },
                    success: function (data) {
                        if (data.status) {

                            swal.fire(data.result);
                            SelectShipmentNumber();
                            //GetNumberStatus();
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

    function GetRollReamWT() {
        if ($('#REAM_INPUT_AREA').is(":visible")) {
            return $('#txtRollReamWT').val();
        } else {
            //return $('#txtInputTransactionQty').val();
            return 0;
        }
    }

    function GetLotNumber() {
        if ($('#ROLL_INPUT_AREA').is(":visible")) {
            return $('#txtLotNumber').val();
        } else {
            return "";
        }
    }


    function CheckCreateInboundBarcode() {

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

        //if ($('#ddlOutLocatorArea').is(":visible") && $('#ddlInLocatorArea').is(":visible")) {
        //    if (getOutLocatorId() == getInLocatorId()) {
        //        swal.fire('同倉庫儲位要不同');
        //        event.preventDefault();
        //        return;
        //    }
        //}

        //if ($('#ddlOutLocatorArea').is(":hidden ") && $('#ddlInLocatorArea').is(":hidden ")) {
        //    if (getOutSubinventoryCode() == getInSubinventoryCode()) {
        //        swal.fire('不可同倉庫無儲位移轉');
        //        event.preventDefault();
        //        return;
        //    }
        //}

        //if ($('#AutoCompleteShipmentNumber').val() == "") {
        //    swal.fire('請輸入編號');
        //    event.preventDefault();
        //    return;
        //}

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

        var shipmentNumber = getShipmentNumber();
        if (shipmentNumber == "新增編號") {
            //CreateShipmnetNumber();

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
                    InboundCreateDetail(null, getShipmentNumber());

                }
            });
        } else {
            InboundCreateDetail(null, getShipmentNumber());
            //$.ajax({
            //    url: "/StockTransaction/InobundCheckShipmentNumberExist",
            //    type: "post",
            //    data: {
            //        shipmentNumber: getShipmentNumber()
            //    },
            //    success: function (data) {
            //        if (data.Success) {
            //            if (data.Data == 0) {
            //                swal.fire({
            //                    title: "新增編號",
            //                    text: "確定新增此出貨編號" + shipmentNumber + "嗎?",
            //                    type: "warning",
            //                    showCancelButton: true,
            //                    confirmButtonColor: "#DD6B55",
            //                    confirmButtonText: "確定",
            //                    cancelButtonText: "取消"
            //                }).then(function (result) {
            //                    if (result.value) {
            //                        InboundCreateDetail();

            //                    }
            //                });
            //            } else if (data.Data == 1) {
            //                InboundCreateDetail();
            //            } else {
            //                swal.fire(data.Msg);
            //            }
            //        } else {
            //            swal.fire(data.Msg);
            //        }

            //    },
            //    error: function () {
            //        swal.fire('檢查出貨編號失敗');
            //    },
            //    complete: function (data) {

            //    }

            //});
        }






        //$.ajax({
        //    url: "/StockTransaction/CheckNumber",
        //    type: "post",
        //    data: {
        //        TransactionType: GetTransactionType(),
        //        OUT_SUBINVENTORY_CODE: getOutSubinventoryCode(),
        //        OUT_LOCATOR_ID: $('#ddlOutLocator').val(),
        //        IN_SUBINVENTORY_CODE: getInSubinventoryCode(),
        //        IN_LOCATOR_ID: $('#ddlInLocator').val(),
        //        Number: $('#ddlShipmentNumber').val()
        //        //Number: $('#AutoCompleteShipmentNumber').val()
        //    },
        //    success: function (data) {
        //        if (data.status) {
        //            if (data.result == "是否新增編號?") {
        //                swal.fire({
        //                    title: "新增編號",
        //                    text: "確定新增編號嗎?",
        //                    type: "warning",
        //                    showCancelButton: true,
        //                    confirmButtonColor: "#DD6B55",
        //                    confirmButtonText: "確定",
        //                    cancelButtonText: "取消"
        //                }).then(function (result) {
        //                    if (result.value) {
        //                        CreateInboundBarcode();

        //                    }
        //                });
        //            } else {
        //                CreateInboundBarcode();

        //            }


        //        } else {
        //            swal.fire(data.result);
        //        }

        //    },
        //    error: function () {
        //        swal.fire('新增入庫單明細失敗');
        //    },
        //    complete: function (data) {

        //    }

        //});
    }

    function CreateShipmnetNumber() {

        $.ajax({
            url: '/StockTransaction/CreateShipmnetNumberDialog',
            type: "POST",
            data: {},
            success: function (result) {
                $('body').append(result);
                OpenCreateShipmnetNumberDialog($('#CreateShipmentNumberModal'));
            }
        });
    }

    function OpenCreateShipmnetNumberDialog(modal_dialog) {
        modal_dialog.modal({
            backdrop: "static",
            keyboard: true,
            show: true
        });

        $('#DialogDdlCreateType').change(function () {
            var createType = $("#DialogDdlCreateType option:selected").val();
            if (createType == "手動輸入") {
                $('#DialogTxtShipmentNumberArea').show();
            } else {
                $('#DialogTxtShipmentNumberArea').hide();
                $('#DialogTxtShipmentNumber').val("");
            }


        })

        modal_dialog.on('hidden.bs.modal', function (e) {
            $("div").remove(modal_dialog.selector);
        });

        modal_dialog.on('show.bs.modal', function (e) {
            $.validator.unobtrusive.parse('form');
        });

        //確認按鍵
        modal_dialog.on('click', '#btnConfirm', function (e) {
            var createType = $("#DialogDdlCreateType option:selected").val();
            var shipmentNumber = "";

            if (createType == "手動輸入") {
                shipmentNumber = $('#DialogTxtShipmentNumber').val();
                if (!(shipmentNumber.trim())) {
                    swal.fire("請輸入出貨編號");
                    return
                }
            }

            if (createType == "新增編號") {
                shipmentNumber = "新增編號";
            }

            if (shipmentNumber != "新增編號") {
                $.ajax({
                    url: "/StockTransaction/InobundCheckShipmentNumberExist",
                    type: "post",
                    data: {
                        shipmentNumber: shipmentNumber
                    },
                    success: function (data) {
                        if (data.Success) {
                            if (data.Data == 0) {
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
                                        InboundCreateDetail(modal_dialog, shipmentNumber);
                                    }
                                });
                            } else if (data.Data == 1) {
                                swal.fire("出貨編號已存在");
                            } else {
                                swal.fire(data.Msg);
                            }
                        } else {
                            swal.fire(data.Msg);
                        }

                    },
                    error: function () {
                        swal.fire('檢查出貨編號失敗');
                    },
                    complete: function (data) {

                    }

                });
            } else {
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
                        InboundCreateDetail(modal_dialog, shipmentNumber);
                    }
                });
            }
        });

        modal_dialog.modal('show');

    }

    

    function InboundCreateDetail(modal_dialog, shipmentNumber) {

        $.ajax({
            url: "/StockTransaction/InboundCreateDetail",
            type: "post",
            data: {
                shipmentNumber: shipmentNumber,
                transferType: $("#ddlTransferType").val(),
                itemNumber: $('#AutoCompleteItemNumber').val(),
                outOrganizationId: getOutOrganizationId(),
                outSubinventoryCode: getOutSubinventoryCode(),
                outLocatorId: getOutLocatorId(),
                inOrganizationId: getInOrganizationId(),
                inSubinventoryCode: getInSubinventoryCode(),
                inLocatorId: getInLocatorId(),
                requestedQty: $('#txtInputTransactionQty').val(),
                rollReamWt: GetRollReamWT(),
                lotNumber: GetLotNumber()
            },
            success: function (data) {
                if (data.status) {
                    if ($("#chkCustomShipmentNumber").prop("checked")) {
                        $("#chkCustomShipmentNumber").prop("checked", false);
                        $(".custom-combobox").show(100);
                        $("#txtShipmentNumber").hide();
                    }
                    GetShipmentNumberList(data.transferHeaderId, data.shipmentNumber);
                    if (modal_dialog) {
                        $(modal_dialog.selector).modal('hide');
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
        var TransactionType = GetTransactionType();
        $.ajax({
            url: "/StockTransaction/CreateInboundBarcode",
            type: "post",
            data: {
                TransactionType: TransactionType,
                OUT_SUBINVENTORY_CODE: getOutSubinventoryCode(),
                OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
                IN_SUBINVENTORY_CODE: getInSubinventoryCode(),
                IN_LOCATOR_ID: $("#ddlInLocator").val(),
                Number: $('#ddlShipmentNumber').val(),
                //Number: $('#AutoCompleteShipmentNumber').val(),
                ITEM_NUMBER: $('#AutoCompleteItemNumber').val(),
                REQUESTED_QTY: $('#txtInputTransactionQty').val(),
                ROLL_REAM_WT: GetRollReamWT(),
                LOT_NUMBER: GetLotNumber()
            },
            success: function (data) {
                if (data.status) {
                    GetShipmentNumberList(data.shipmentNumber, data.shipmentNumber);
                    //if (TransactionType == "出貨編號") {
                    //    GetShipmentNumberList(data.shipmentNumber, data.shipmentNumber);
                    //}
                    //else if (TransactionType == "移轉編號") {
                    //    GetSubinventoryTransferNumberList(data.shipmentNumber, data.shipmentNumber);
                    //}
                    //else {

                    //}
                    //InBoundBarcodeDataTablesBody.ajax.reload();
                    ////$('#NumberStatus').html(data.result);
                    //InBoundBarcodeDataTablesBody.buttons().disable();
                    //$('#AutoCompleteItemNumber').attr('disabled', true);
                    //$('#txtInputTransactionQty').attr('disabled', true);
                    //$('#txtRollReamWT').attr('disabled', true);
                    //$('#txtLotNumber').attr('disabled', true);
                    //$('#txtBARCODE').attr('disabled', true);
                    //$('#btnImportFile').attr('disabled', true);
                    //$('#btnExampleDownload').attr('disabled', true);
                    //$('#btnExampleDownload2').attr('disabled', true);
                    //$('#btnPrintRollPick').attr('disabled', true);
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
        for (i = 0; i < selectedData.length; i++) {
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

            var excelList = [];
            $.each(dd, function (index, value) {
                var InboundImportExcelModel = {
                    'ItemNumber': value.ITEM_NUMBER,
                    'Qty': value.PRIMARY_QUANTITY,
                    'LotNumber': value.LOT_NUMBER,
                    'RollReamWt': 0,
                    'ContainerNo': value.CONTAINER_NO
                }
                excelList.push(InboundImportExcelModel);
            });

            $.ajax({
                "url": "/StockTransaction/InboundImportExcel",
                "type": "POST",
                //"datatype": "json",
                //contentType: 'application/json',
                "data": {
                    excelList: excelList,
                    shipmentNumber: getShipmentNumber(),
                    transferType: $("#ddlTransferType").val(),
                    outOrganizationId: getOutOrganizationId(),
                    outSubinventoryCode: getOutSubinventoryCode(),
                    outLocatorId: getOutLocatorId(),
                    inOrganizationId: getInOrganizationId(),
                    inSubinventoryCode: getInSubinventoryCode(),
                    inLocatorId: getInLocatorId()
                },
                success: function (data) {
                    if (data.status) {
                        if ($("#chkCustomShipmentNumber").prop("checked")) {
                            $("#chkCustomShipmentNumber").prop("checked", false);
                            $(".custom-combobox").show(100);
                            $("#txtShipmentNumber").hide();
                        }
                        GetShipmentNumberList(data.transferHeaderId, data.shipmentNumber);
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
            //var StockTransferDTList = [];
            //$.each(dd, function (index, value) {
            //    var StockTransferDT = {
            //        'IN_SUBINVENTORY_CODE': value.Subinventory,
            //        'IN_LOCATOR_ID': value.Locator,
            //        'ITEM_NUMBER': value.ITEM_NUMBER,
            //        'REQUESTED_QUANTITY': value.REQUESTED_QUANTITY,
            //        'REQUESTED_QUANTITY2': value.REQUESTED_QUANTITY2,
            //        'ROLL_REAM_WT': value.ROLL_REAM_WT,
            //        'ROLL_REAM_QTY': value.ROLL_REAM_QTY
            //    }
            //    StockTransferDTList.push(StockTransferDT);
            //});
            //var TransactionType = GetTransactionType();
            //var data = {
            //    TransactionType: TransactionType,
            //    OUT_SUBINVENTORY_CODE: getOutSubinventoryCode(),
            //    OUT_LOCATOR_ID: $("#ddlOutLocator").val(),
            //    IN_SUBINVENTORY_CODE: getInSubinventoryCode(),
            //    IN_LOCATOR_ID: $("#ddlInLocator").val(),
            //    Number: $('#ddlShipmentNumber').val(),
            //    //Number: $('#AutoCompleteShipmentNumber').val(),
            //    'StockTransferDTList': StockTransferDTList
            //}

            //var json = JSON.stringify(data);

            var excelList = [];
            $.each(dd, function (index, value) {
                var InboundImportExcelModel = {
                    'ItemNumber': value.ITEM_NUMBER,
                    'Qty': value.REQUESTED_QUANTITY2,
                    'LotNumber': "",
                    'RollReamWt': value.ROLL_REAM_WT
                }
                excelList.push(InboundImportExcelModel);
            });

            $.ajax({
                "url": "/StockTransaction/InboundImportExcel",
                "type": "POST",
                //"datatype": "json",
                //contentType: 'application/json',
                "data": {
                    excelList: excelList,
                    shipmentNumber: getShipmentNumber(),
                    transferType: $("#ddlTransferType").val(),
                    outOrganizationId: getOutOrganizationId(),
                    outSubinventoryCode: getOutSubinventoryCode(),
                    outLocatorId: getOutLocatorId(),
                    inOrganizationId: getInOrganizationId(),
                    inSubinventoryCode: getInSubinventoryCode(),
                    inLocatorId: getInLocatorId()
                },
                success: function (data) {
                    if (data.status) {
                        if ($("#chkCustomShipmentNumber").prop("checked")) {
                            $("#chkCustomShipmentNumber").prop("checked", false);
                            $(".custom-combobox").show(100);
                            $("#txtShipmentNumber").hide();
                        }
                        GetShipmentNumberList(data.transferHeaderId, data.shipmentNumber);
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

    //彈出dialog
    function OpenMergeBarcodeDialog(modal_dialog) {
        modal_dialog.modal({
            backdrop: "static",
            keyboard: true,
            show: true
        });

        modal_dialog.on('hidden.bs.modal', function (e) {
            $("div").remove(modal_dialog.selector);
        });

        modal_dialog.on('show.bs.modal', function (e) {
            //$.validator.unobtrusive.parse('form');
        });


        modal_dialog.on('click', '#btnConfirm', function (e) {

            var BARCODE = $('#txtMergeBarcode').val();

            var IDs = [];

            $.each($('#WaitMergeBarcode').find('.pickId'), function (index, value) {
                IDs.push($(value).text());
            });

            $.ajax({
                url: "/StockTransaction/GetMergeBarocdeStatus",
                type: "post",
                data: {
                    MergeBarocde: BARCODE,
                    waitMergeIDs: IDs
                },
                success: function (data) {
                    $("#OriginalBarcode").text("");
                    $("#OriginalQty").text("");
                    $("#OriginalUnit").text("");
                    $("#AfterBarcode").text("");
                    $("#AfterQty").text("");
                    $("#AfterUnit").text("");
                    if (data.status) {
                        $("#OriginalBarcode").text(data.OriginalBarcode);
                        $("#OriginalQty").text(data.OriginalQty);
                        $("#OriginalUnit").text(data.OriginalUnit);
                        $("#AfterBarcode").text(data.AfterBarcode);
                        $("#AfterQty").text(data.AfterQty);
                        $("#AfterUnit").text(data.AfterUnit);
                    }
                    else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('取得條碼資料失敗');
                },
                complete: function (data) {


                }

            });


        });

        modal_dialog.on('keydown', '#txtMergeBarcode', function (e) {
            var key = e.which || e.keyCode;
            if (key == 13) {
                $('#btnConfirm').click();
            }
        });

        modal_dialog.on('click', '#btnMergeBarcode', function (e) {

            var BARCODE = $('#txtMergeBarcode').val();

            var IDs = [];

            $.each($('#WaitMergeBarcode').find('.pickId'), function (index, value) {
                IDs.push($(value).text());
            });

            $.ajax({
                url: "/StockTransaction/MergeBarcode",
                type: "post",
                data: {
                    MergeBarocde: BARCODE,
                    waitMergeIDs: IDs
                },
                success: function (data) {
                    if (data.status) {
                        InBoundBarcodeDataTablesBody.ajax.reload();
                        swal.fire(data.result);
                        modal_dialog.modal('hide');
                    } else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('取得條碼資料失敗');
                },
                complete: function (data) {


                }

            });


        });

        modal_dialog.on('click', '#btnCancel', function () {
            $.ajax({
                url: '//'
            });
        });


        modal_dialog.modal('show');

    }

    function ImportPaperRoll() {
        var fileInput = $('#file').get(0).files;
        var ddlInSubinventory = getInSubinventoryCode();
        var ddlInLocator = $('#ddlInLocator option:selected').text();
        var Number = $('#ddlShipmentNumber').val();
        //var Number = $('#AutoCompleteShipmentNumber').val();
        var OUT_LOCATOR_ID = $("#ddlOutLocator").val();
        var IN_SUBINVENTORY_CODE = getInSubinventoryCode();
        var IN_LOCATOR_ID = $("#ddlInLocator").val();
        var TransactionType = $('#TransactionType').text();
        var OUT_SUBINVENTORY_CODE = getOutSubinventoryCode();
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
                { data: "CONTAINER_NO", "name": "櫃號", "autoWidth": true, "className": "dt-body-left" },
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
        var ddlInSubinventory = getInSubinventoryCode();
        var ddlInLocator = $('#ddlInLocator option:selected').text();
        var Number = $('#ddlShipmentNumber').val();
        //var Number = $('#AutoCompleteShipmentNumber').val();
        var OUT_LOCATOR_ID = $("#ddlOutLocator").val();
        var IN_SUBINVENTORY_CODE = getInSubinventoryCode();
        var IN_LOCATOR_ID = $("#ddlInLocator").val();
        var TransactionType = $('#TransactionType').text();
        var OUT_SUBINVENTORY_CODE = getOutSubinventoryCode();
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
                { data: "REQUESTED_QUANTITY", "name": "數量(噸)", "autoWidth": true },
            ]
        });
    }


    function GetShipmentNumberList(selectValue, selectText) {

        var InSubinventoryCode = getInSubinventoryCode();
        //var InLocator = $("#ddlInLocator").val();
        var OutSubinventoryCode = getOutSubinventoryCode();
        //var OutLocator = $("#ddlOutLocator").val();

        if (InSubinventoryCode == "請選擇" || OutSubinventoryCode == "請選擇") {
            return;
        }

        $.ajax({
            url: "/StockTransaction/GetInboundShipmentNumberList",
            type: "post",
            data: {
                outOrganizationId: getOutOrganizationId(),
                outSubinventoryCode: OutSubinventoryCode,
                inOrganizationId: getInOrganizationId(),
                inSubinventoryCode: InSubinventoryCode

            },
            success: function (data) {
                if (data.status) {
                    $('#ddlShipmentNumber').empty();
                    for (var i = 0; i < data.items.length; i++) {
                        $('#ddlShipmentNumber').append($('<option></option>').val(data.items[i].Value).html(data.items[i].Text));
                    }
                    $("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);
                    //$('#TransferHeaderId').text(selectValue);
                    //$('#ddlShipmentNumber option[text="' + selectText + '", value="' + selectValue + '"]').attr('selected', 'selected');
                    SelectShipmentNumber();
                } else {
                    swal.fire(data.result);
                }

                //$('#ddlShipmentNumber').empty();
                //for (var i = 0; i < data.items.length; i++) {
                //    $('#ddlShipmentNumber').append($('<option></option>').val(data.items[i].Value).html(data.items[i].Text));
                //}


                //var transferCatalog = data.transferCatalog;
                //$("#ddlShipmentNumber").val("");
                //$("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);

                //$("#ddlShipmentNumber").val(selectValue).trigger('change');
                //GetNumberStatus();
            },
            error: function () {
                swal.fire('更新出貨編號失敗');
            },
            complete: function (data) {


            }

        })

    }
    //沒用到
    function GetSubinventoryTransferNumberList(selectValue, selectText) {

        var InSubinventoryCode = getInSubinventoryCode();
        var InLocator = $("#ddlInLocator").val();
        var OutSubinventoryCode = getOutSubinventoryCode();
        var OutLocator = $("#ddlOutLocator").val();

        $.ajax({
            url: "/StockTransaction/GetSubinventoryTransferNumberList",
            type: "post",
            data: {
                OutSubinventoryCode: OutSubinventoryCode,
                OutLocator: OutLocator,
                InSubinventoryCode: InSubinventoryCode,
                InLocator: InLocator
            },
            success: function (data) {
                $('#ddlShipmentNumber').empty();
                for (var i = 0; i < data.length; i++) {
                    $('#ddlShipmentNumber').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }

                //$("#ddlSubinventoryTransferNumber").val("");
                $("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);

                //$("#ddlShipmentNumber").val(selectValue).trigger('change');
                GetNumberStatus();

            },
            error: function () {
                swal.fire('更新移轉編號失敗');
            },
            complete: function (data) {


            }

        })
    }
}



