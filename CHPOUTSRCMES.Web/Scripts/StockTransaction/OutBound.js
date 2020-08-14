
function OutBoundInit() {

    var selected = [];
    var editor;
    var selectedNumber;

    function getOutSubinventoryCode() {
        return $("#ddlOutSubinventory option:selected").text();
    }
    function getInSubinventoryCode() {
        return $("#ddlInSubinventory option:selected").text();
    }

    SubinventoryInit();

    

    //jQuery.ui.autocomplete.prototype._resizeMenu = function () {
    //    var ul = this.menu.element;
    //    ul.outerWidth(this.element.outerWidth());
    //}

    //$("#ddlItemNumber").combobox({
    //    select: function (event, ui) {
    //        GetStockItemData(this.value);
    //    }
    //});

    //$("#ddlItemNumber").change(function () {
    //    GetStockItemData(this.value);
    //});


    $("#ddlShipmentNumber").combobox({
        select: function (event, ui) {
            GetNumberStatus();
            //GetStockItemData(this.value);
        }
    });

    $('.custom-combobox-input').css('width', '200px');
    $('#btnPrintPick').css('margin-left', '28px');



    $("#btnPrintPick").click(function () {

    });

    $("#btnSaveBarcode").click(function () {
        SaveStockTransferBarcodeDT();
    });


    $("#btnSaveTransfer").click(function () {
        OutBoundSaveTransfer();
    });

    //$('#txtShipmentNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        //$('#ddlItemNumber').next().find('input').focus().select();
    //        $('#AutoCompleteItemNumber').focus().select();
    //    }
    //});

    //$('#txtSubinventoryTransferNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        //$('#ddlItemNumber').next().find('input').focus().select();
    //        $('#AutoCompleteItemNumber').focus().select();
    //    }
    //});

    //$('#AutoCompleteItemNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        //$('#txtInputTransactionQty').focus();
    //        //$("#AutoCompleteItemNumber").trigger("autocompleteselect");
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
            //if ($("#AutoCompleteItemNumber").attr("disabled") == "undefined") {
            //    $('#AutoCompleteItemNumber').focus().select();
            //}
        }
    });

    //$('#AutoCompleteShipmentNumber').blur(function () {
    //    $(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
    //});

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
            SaveStockTransferBarcodeDT();
        }
    });

    $("#txtBARCODE").on(
          "input propertychange paste", function () {
              $("#txtSECONDARY_QUANTITY").val("");
          });

    $('#txtSECONDARY_QUANTITY').keydown(function (e) {
        if (e.keyCode == 13) {
            SaveStockTransferBarcodeDT();
        }
    });


    $("#btnSaveStockTransferDT").click(function () {
        CheckSaveStockTransferDT();
    });


    function SubinventoryInit() {

        $('#ddlOutSubinventory').change(function () {

            var SUBINVENTORY_CODE = getOutSubinventoryCode();
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

            var SUBINVENTORY_CODE = getInSubinventoryCode();
            $.ajax({
                url: "/OrgSubinventory/GetLocatorList",
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

        checkTransactionType();



      
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
                    $('#NumberStatus').html('');
                    //$('#AutoCompleteShipmentNumber').val("");
                    //selectedNumber = "";
                    //$('#txtSubinventoryTransferNumber').val("");
                    $('#txtInputTransactionQty').val("");
                    $('#txtRollReamQty').val("");
                    OutBoundDataTablesBody.ajax.reload(null, false);
                    OutBoundBarcodeDataTablesBody.ajax.reload();

                    inputOpen();

                    if (data.result == "倉庫間移轉") {
                        //$('#DETAIL_AREA').show();
                        //$('#SHIPMENT_NUMBER_AREA').hide();
                        //$('#SUBINVENTORY_TRANSFER_NUMBER_AREA').show();
                        GetSubinventoryTransferNumberList("新增編號", "新增編號");
                        $('#TransactionType').html("移轉編號");
                        //GetItemNumberList();
                        //document.getElementById('DisplayDetail').value = true;
                        //document.getElementById('DisplayShipmentNumberArea').value = false;
                        //document.getElementById('DisplaySubinventoryTransferNumberArea').value = true;
                    } else {
                        //$('#DETAIL_AREA').show();
                        //$('#SHIPMENT_NUMBER_AREA').show();
                        //$('#SUBINVENTORY_TRANSFER_NUMBER_AREA').hide();
                        GetShipmentNumberList("新增編號", "新增編號");
                        $('#TransactionType').html("出貨編號");
                        //GetItemNumberList();
                        //document.getElementById('DisplayDetail').value = true;
                        //document.getElementById('DisplayShipmentNumberArea').value = true;
                        //document.getElementById('DisplaySubinventoryTransferNumberArea').value = false;
                    }
                } else {
                    $('#TransactionType').html("編號");
                    //$('#DETAIL_AREA').hide();
                    //$('#SHIPMENT_NUMBER_AREA').hide();
                    //$('#SUBINVENTORY_TRANSFER_NUMBER_AREA').hide();
                    //document.getElementById('DisplayDetail').value = false;
                    //document.getElementById('DisplayShipmentNumberArea').value = false;
                    //document.getElementById('DisplaySubinventoryTransferNumberArea').value = false;
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

            multi: {
                "title": "多欄位異動",
                "info": "請注意，您一次選擇多個不同的備註，此次異動將會變成同樣的備註！",
                "restore": "取消更改",
                "noMulti": "This input can be edited individually, but not part of a group."
            }
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
        //pageLength: 2,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockTransaction/GetStockTransferDT",
            "type": "Post",
            "datatype": "json",
            "data": function (d) {
                d.TransactionType = GetTransactionType();
                d.OutSubinventoryCode = getOutSubinventoryCode();
                d.OutLocator = $("#ddlOutLocator").val();
                d.InSubinventoryCode = getInSubinventoryCode();
                d.InLocator = $("#ddlInLocator").val();
                d.Number = $('#ddlShipmentNumber').val();
                //d.Number = $('#AutoCompleteShipmentNumber').val();
                d.NumberStatus = $('#NumberStatus').text();
            }
        },
        columns: [
         { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
         { data: "ID", name: "項次", autoWidth: true },
         { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
         { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },
         { data: "ROLL_REAM_QTY", name: "捲數/板數", autoWidth: true },
         { data: "ROLL_REAM_UOM", name: "捲/板", autoWidth: true },
         { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right" },
         { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right" },
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
         { data: "SRC_REQUESTED_QUANTITY_UOM2", name: "輔單位", autoWidth: true }
        ],

        "order": [[1, 'asc']],
        select: {
            style: 'single',
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
                 'selectNone',
                  {
                      text: '刪除',
                      className: 'btn-danger',
                      action: function () {
                          var selectedData = OutBoundDataTablesBody.rows('.selected').data();
                          if (selectedData.length == 0) {
                              swal.fire("請選擇要刪除的料號");
                              return;
                          }

                          swal.fire({
                              title: "料號資料刪除",
                              text: "將刪除此料號所有條碼資料，確定刪除嗎?",
                              type: "warning",
                              showCancelButton: true,
                              confirmButtonColor: "#DD6B55",
                              confirmButtonText: "確定",
                              cancelButtonText: "取消"
                          }).then(function (result) {
                              if (result.value) {
                                  DeleteItemNumber(selectedData);
                              }
                          });

                      }
                  }

            ]
        },

        "rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {
            if ($.inArray(data.ID, selected) !== -1) {
                var selectRow = ':eq(' + dataIndex + ')';
                OutBoundDataTablesBody.row(selectRow, { page: 'current' }).select();
            }
        },
        "preDrawCallback": function (settings) {
            $("#StockTransferDT_ID").text("");
            $("#SelectedItemNumber2").text("");
            $("#PACKING_TYPE2").text("");
        }

    });

    OutBoundDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var StockTransferDT_ID = dt.rows(indexes).data().pluck('ID')[0];
            $("#StockTransferDT_ID").text(StockTransferDT_ID);
            var SelectedItemNumber2 = dt.rows(indexes).data().pluck('ITEM_NUMBER')[0];
            $("#SelectedItemNumber2").text(SelectedItemNumber2);
            var PACKING_TYPE = dt.rows(indexes).data().pluck('PACKING_TYPE')[0];
            $('#PACKING_TYPE2').text(PACKING_TYPE);

            var rowsData = OutBoundDataTablesBody.rows({ page: 'current' }).data();
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

            if (PACKING_TYPE == "令包") {
                $("#SECONDARY_QUANTITY").show();
                $("#txtSECONDARY_QUANTITY").show();
            }

            $('#txtBARCODE').focus().select();
        }
    });

    OutBoundDataTablesBody.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#StockTransferDT_ID").text("");
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

    var OutBoundBarcodeDataTablesBody = $('#OutBoundBarcodeDataTablesBody').DataTable({
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
            "url": "/StockTransaction/GetStockTransferBarcodeDT",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.TransactionType = GetTransactionType();
                d.OUT_SUBINVENTORY_CODE = getOutSubinventoryCode();
                d.OUT_LOCATOR_ID = $("#ddlOutLocator").val();
                d.IN_SUBINVENTORY_CODE = getInSubinventoryCode();
                d.IN_LOCATOR_ID = $("#ddlInLocator").val();
                d.Number = $('#ddlShipmentNumber').val();
                //d.Number = $('#AutoCompleteShipmentNumber').val();
                d.NumberStatus = $('#NumberStatus').text();
            }
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
         { data: "StockTransferDT_ID", name: "項次", autoWidth: true },
          { data: "BARCODE", name: "條碼", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
            //{ data: "LOT_NUMBER", name: "捲號", autoWidth: true, className: "dt-body-left" },
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

        order: [[10, 'desc']],
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
                         var selectedData = OutBoundBarcodeDataTablesBody.rows('.selected').data();
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
                        PrintLable(OutBoundBarcodeDataTablesBody, "/Home/GetLabel2", "2");
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

                        editor.edit(OutBoundBarcodeDataTablesBody.rows({ selected: true }).indexes())
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

    //$('#ddlShipmentNumber').change(function () {
    //    var ShipmentNumber = $('#ddlShipmentNumber').val();
    //    if (ShipmentNumber == "新增出貨編號") {
    //        $('#txtShipmentNumber').val("");
    //        $('#txtShipmentNumber').focus();
    //    } else {
    //        $('#txtShipmentNumber').val(ShipmentNumber);
    //    }
    //    $('#txtInputTransactionQty').val("");
    //    $('#txtRollReamQty').val("");
    //    OutBoundDataTablesBody.ajax.reload(null, false);
    //    $('#txtBARCODE').val("");
    //    $("#SECONDARY_QUANTITY").hide();
    //    $('#txtSECONDARY_QUANTITY').val("");
    //    OutBoundBarcodeDataTablesBody.ajax.reload();
    //})

    //$('#ddlSubinventoryTransferNumber').change(function () {
    //    var SubinventoryTransferNumber = $('#ddlSubinventoryTransferNumber').val();
    //    if (SubinventoryTransferNumber == "新增移轉編號") {
    //        $('#txtSubinventoryTransferNumber').val("");
    //        $('#txtSubinventoryTransferNumber').focus();
    //    } else {
    //        $('#txtSubinventoryTransferNumber').val(SubinventoryTransferNumber);
    //    }
    //    $('#txtInputTransactionQty').val("");
    //    $('#txtRollReamQty').val("");
    //    OutBoundDataTablesBody.ajax.reload(null, false);
    //    $('#txtBARCODE').val("");
    //    $("#SECONDARY_QUANTITY").hide();
    //    $('#txtSECONDARY_QUANTITY').val("");
    //    OutBoundBarcodeDataTablesBody.ajax.reload();
    //})

    $("#AutoCompleteShipmentNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/AutoCompleteShipmentNumber",
                type: "POST",
                dataType: "json",
                data: {
                    TransactionType: GetTransactionType(),
                    OutSubinventoryCode: getOutSubinventoryCode(),
                    OutLocator: $("#ddlOutLocator").val(),
                    InSubinventoryCode: getInSubinventoryCode(),
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
        },
        //change: function (event, ui) {
        //    if (ui.item) {
        //        selectedNumber = ui.item.value;
        //        $('#txtInputTransactionQty').val("");
        //        $('#txtRollReamQty').val("");
        //        OutBoundDataTablesBody.ajax.reload(null, false);
        //        $('#txtBARCODE').val("");
        //        $("#SECONDARY_QUANTITY").hide();
        //        $('#txtSECONDARY_QUANTITY').val("");
        //        OutBoundBarcodeDataTablesBody.ajax.reload();
        //        $('#AutoCompleteItemNumber').focus().select();
        //    }
        //}
        //close: function (event, ui) {
        //    $('#txtInputTransactionQty').focus();
        //}
    });

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
                    $('#txtInputTransactionQty').val("");
                    $('#txtRollReamQty').val("");
                    OutBoundDataTablesBody.ajax.reload(null, false);
                    $('#txtBARCODE').val("");
                    $("#SECONDARY_QUANTITY").hide();
                    $('#txtSECONDARY_QUANTITY').val("");
                    OutBoundBarcodeDataTablesBody.ajax.reload();
                    inputOpen();
                    if (data.result == "MES未出庫") {

                    } else if (data.result == "MES已出庫") {
                        inputClose();
                    } else if (data.result == "MES已入庫") {
                        inputClose();
                    } else if (data.result == "非MES入庫手動新增") {
                        swal.fire("編號" + $('#ddlShipmentNumber').val() + "沒有出貨紀錄");
                        //swal.fire("編號" + $('#AutoCompleteShipmentNumber').val() + "沒有出貨紀錄");
                        //$('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                        //selectedNumber = "";
                    } else if (data.result == "非MES入庫檔案匯入") {
                        swal.fire("編號" + $('#ddlShipmentNumber').val() + "沒有出貨紀錄");
                        //swal.fire("編號" + $('#AutoCompleteShipmentNumber').val() + "沒有出貨紀錄");
                        //$('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                        //selectedNumber = "";
                    } else if (data.result == "非MES已入庫") {
                        swal.fire("編號" + $('#ddlShipmentNumber').val() + "沒有出貨紀錄");
                        //swal.fire("編號" + $('#AutoCompleteShipmentNumber').val() + "沒有出貨紀錄");
                        //$('#AutoCompleteShipmentNumber').autocomplete('close').val('');
                        //selectedNumber = "";
                    } else {
                        //為新增編號 此編號還未儲存
                        //$('#AutoCompleteItemNumber').focus();
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


    function inputOpen() {
        OutBoundDataTablesBody.buttons().enable();
        OutBoundBarcodeDataTablesBody.buttons().enable();
        $('#AutoCompleteItemNumber').attr('disabled', false);
        $('#txtInputTransactionQty').attr('disabled', false);
        $('#txtRollReamQty').attr('disabled', false);
        $('#txtBARCODE').attr('disabled', false);
        $('#btnPrintPick').attr('disabled', false);
        $('#btnSaveBarcode').attr('disabled', false);
        $('#btnSaveTransfer').attr('disabled', false);
        $('#btnSaveStockTransferDT').attr('disabled', false);
    }

    function inputClose() {
        OutBoundDataTablesBody.buttons().disable();
        OutBoundBarcodeDataTablesBody.buttons().disable();
        $('#AutoCompleteItemNumber').attr('disabled', true);
        $('#txtInputTransactionQty').attr('disabled', true);
        $('#txtRollReamQty').attr('disabled', true);
        $('#txtBARCODE').attr('disabled', true);
        $('#btnPrintPick').attr('disabled', true);
        $('#btnSaveBarcode').attr('disabled', true);
        $('#btnSaveTransfer').attr('disabled', true);
        $('#btnSaveStockTransferDT').attr('disabled', true);
    }


    $("#AutoCompleteItemNumber").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/AutoCompleteItemNumber",
                type: "POST",
                dataType: "json",
                data: {
                    //SubinventoryCode: getOutSubinventoryCode(),
                    //Locator: $("#ddlOutLocator").val(),
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data.slice(0, 20), function (item) {
                        return { label: item.Description, value: item.Value };
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
                GetStockItemData(ui.item.value);
            }
        }
        //change: function (event, ui) {
        //    if (ui.item) {
        //        GetStockItemData(ui.item.value);
        //    }
        //},
        //close: function (event, ui) {
        //    $('#txtInputTransactionQty').focus();
        //}
    });

    function CheckSaveStockTransferDT() {

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

        if ($('#ddlOutLocatorArea').is(":visible") && $('#ddlInLocatorArea').is(":visible")) {
            if ($('#ddlOutLocatorArea').val() == $('#ddlInLocatorArea').val()) {
                swal.fire('同倉庫儲位要不同');
                event.preventDefault();
                return;
            }
        }

        if ($('#ddlOutLocatorArea').is(":hidden ") && $('#ddlInLocatorArea').is(":hidden ")) {
            if (getOutSubinventoryCode() == getInSubinventoryCode()) {
                swal.fire('不可同倉庫無儲位移轉');
                event.preventDefault();
                return;
            }
        }

        //if (GetTransactionType() == "出貨編號") {
        //    if ($('#txtShipmentNumber').val() == "") {
        //        swal.fire('請輸入出貨編號');
        //        event.preventDefault();
        //        return;
        //    }
        //}
        //if (GetTransactionType() == "移轉編號") {
        //    if ($('#txtSubinventoryTransferNumber').val() == "") {
        //        swal.fire('請輸入移轉標號');
        //        event.preventDefault();
        //        return;
        //    }
        //}
        //if ($('#ddlItemNumber').val() == "請選擇") {
        //    swal.fire('請選擇料號');
        //    event.preventDefault();
        //    return;
        //}
        //if ($('#AutoCompleteShipmentNumber').val() == "") {
        //    swal.fire('請輸入編號');
        //    event.preventDefault();
        //    return;
        //}
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
        //var Number = $('#AutoCompleteShipmentNumber').val();
        var Number = $('#ddlShipmentNumber').val();

        var TransactionType = GetTransactionType();

        $.ajax({
            url: "/StockTransaction/CheckNumber",
            type: "post",
            data: {
                TransactionType: TransactionType,
                OUT_SUBINVENTORY_CODE: getOutSubinventoryCode(),
                OUT_LOCATOR_ID: $('#ddlOutLocator').val(),
                IN_SUBINVENTORY_CODE: getInSubinventoryCode(),
                IN_LOCATOR_ID: $('#ddlInLocator').val(),
                Number: Number
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
                                SaveStockTransferDT(TransactionType, Number);

                            }
                        });
                    } else {
                        SaveStockTransferDT(TransactionType, Number);

                    }


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



    }

    function SaveStockTransferDT(TransactionType, Number) {


        $.ajax({
            url: "/StockTransaction/SaveStockTransferDT",
            type: "post",
            data: {
                TransactionType: TransactionType,
                OUT_SUBINVENTORY_CODE: getOutSubinventoryCode(),
                OUT_LOCATOR_ID: $('#ddlOutLocator').val(),
                IN_SUBINVENTORY_CODE: getInSubinventoryCode(),
                IN_LOCATOR_ID: $('#ddlInLocator').val(),
                Number: Number,
                ITEM_NUMBER: $('#AutoCompleteItemNumber').val(),
                REQUESTED_QTY: $('#txtInputTransactionQty').val(),
                PICKED_QTY: 0,
                UNIT: $('#UNIT').text(),
                ROLL_REAM_QTY: $('#txtRollReamQty').val()
            },
            success: function (data) {
                if (data.status) {

                    //GetNumberStatus();
                    //$('#AutoCompleteShipmentNumber').data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $('#AutoCompleteShipmentNumber').val() } });
                    //OutBoundDataTablesBody.ajax.reload(null, false);

                    //$('#txtInputTransactionQty').val("");
                    //$('#txtRollReamQty').val("");
                    //OutBoundDataTablesBody.ajax.reload(null, false);
                    //$('#txtBARCODE').val("");
                    //$("#SECONDARY_QUANTITY").hide();
                    //$('#txtSECONDARY_QUANTITY').val("");
                    //OutBoundBarcodeDataTablesBody.ajax.reload();
                    //$('#AutoCompleteItemNumber').focus().select();

                    if (TransactionType == "出貨編號") {
                        GetShipmentNumberList(data.result, data.result);
                    }
                    else if (TransactionType == "移轉編號") {
                        GetSubinventoryTransferNumberList(data.result, data.result);
                    }
                    else {

                    }

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
    }

    function DeleteItemNumber(selectedData) {
        var TransactionType = GetTransactionType();
        var ID = selectedData[0].ID;

        $.ajax({
            url: "/StockTransaction/DeleteItemNumber",
            type: "post",
            data: {
                'ID': ID,
            },
            success: function (data) {
                if (data.status) {

                    OutBoundDataTablesBody.ajax.reload(null, false);
                    OutBoundBarcodeDataTablesBody.ajax.reload();
                    if (data.result == "刪除料號成功，備貨單已沒任何資料") {
                        $('#txtInputTransactionQty').val("");
                        $('#txtRollReamQty').val("");
                        $('#txtBARCODE').val("");
                        $("#SECONDARY_QUANTITY").hide();
                        $('#txtSECONDARY_QUANTITY').val("");
                        $('#AutoCompleteItemNumber').focus().select();

                        //if (TransactionType == "出貨編號") {
                        //    GetShipmentNumberList("新增出貨編號");
                        //}
                        //else if (TransactionType == "移轉編號") {
                        //    GetSubinventoryTransferNumberList("新增移轉編號");
                        //}
                        //else {

                        //}
                    }
                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('料號刪除失敗');
            },
            complete: function (data) {


            }

        });
    }




    function SaveStockTransferBarcodeDT() {
        var TransactionType = GetTransactionType();
        //var Number = $('#AutoCompleteShipmentNumber').val();
        var Number = $('#ddlShipmentNumber').val();
        var StockTransferDT_ID = $('#StockTransferDT_ID').text();
        if (!StockTransferDT_ID) {
            swal.fire('請選擇料號');
            event.preventDefault();
            return false;
        }

        $.ajax({
            url: "/StockTransaction/SaveStockTransferBarcodeDT",
            type: "post",
            data: {
                TransactionType: TransactionType,
                Number: Number,
                StockTransferDT_ID: StockTransferDT_ID,
                BARCODE: $('#txtBARCODE').val(),
                InputReamQty: $('#txtSECONDARY_QUANTITY').val()
            },
            success: function (data) {
                if (data.status) {
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
    }

    function DeleteBarcode(selectedData) {


        var list = [];
        for (i = 0 ; i < selectedData.length; i++) {
            list.push(selectedData[i].ID);
        }


        $.ajax({
            url: "/StockTransaction/DeleteBarcode",
            type: "post",
            data: {
                IDs: list
            },
            success: function (data) {
                if (data.status) {

                    OutBoundDataTablesBody.ajax.reload(null, false);
                    OutBoundBarcodeDataTablesBody.ajax.reload();
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

    function OutBoundSaveTransfer() {
        var TransactionType = GetTransactionType();
        //var Number = $('#AutoCompleteShipmentNumber').val();
        var Number = $('#ddlShipmentNumber').val();
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
            title: "出庫存檔",
            text: "確定出庫存檔嗎?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: "/StockTransaction/OutBoundSaveTransfer",
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

    function GetStockItemData(ITEM_NO) {

        $.ajax({
            url: "/StockTransaction/GetStockItemData",
            type: "post",
            data: {
                SUBINVENTORY_CODE: getOutSubinventoryCode(),
                ITEM_NO: ITEM_NO
            },
            success: function (data) {
                if (data.Success) {
                    //$('#INPUT_AREA').show();

                    if (data.Data.CatalogElemVal070 == "平版") {
                        $('#PACKING_TYPE_LABEL').show();
                        $('#PACKING_TYPE').show();
                        $('#PACKING_TYPE').html(data.Data.CatalogElemVal110);
                        $('#UNIT').html(data.Data.SecondaryUomCode);
                        $('#txtInputTransactionQty').focus();
                    } else {
                        $('#PACKING_TYPE_LABEL').hide();
                        $('#PACKING_TYPE').hide();
                        $('#PACKING_TYPE').html("");
                        $('#UNIT').html(data.Data.PrimaryUomCode);
                        $('#txtInputTransactionQty').focus();
                    }

                } else {
                    //$('#INPUT_AREA').hide();
                    $('#PACKING_TYPE_LABEL').hide();
                    $('#PACKING_TYPE').html("");
                    $('#PACKING_TYPE').hide();
                    $('#UNIT').html("");
                    swal.fire(data.Msg);
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

        var InSubinventoryCode = getInSubinventoryCode();
        var InLocator = $("#ddlInLocator").val();
        var OutSubinventoryCode = getOutSubinventoryCode();
        var OutLocator = $("#ddlOutLocator").val();

        $.ajax({
            url: "/StockTransaction/GetShipmentNumberList",
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

                //$("#ddlShipmentNumber").val("");
                $("#ddlShipmentNumber").combobox('autocomplete', selectValue, selectText);

                //$("#ddlShipmentNumber").val(selectValue).trigger('change');
                GetNumberStatus();
            },
            error: function () {
                swal.fire('更新出貨編號失敗');
            },
            complete: function (data) {


            }

        })

    }


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









