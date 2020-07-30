var selected = [];

$(document).ready(function () {
    
    var FlatDataTablesBody = $('#FlatDataTablesBody').DataTable({
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
            "url": "/Delivery/GetFlatEdit",
            "type": "Post",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text()
            },
        },
        columns: [
         { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
         { data: "SUB_ID", name: "項次", autoWidth: true },
         { data: "ORDER_NUMBER", name: "訂單", autoWidth: true },
         { data: "ORDER_SHIP_NUMBER", name: "訂單行號", autoWidth: true },   
         //{ data: "OSP_BATCH_ID", name: "代紙工單號碼ID", autoWidth: true, visible: false},
         { data: "OSP_BATCH_NO", name: "代紙工單號碼", autoWidth: true },
         //{ data: "TMP_ITEM_ID", name: "代紙料號ID", autoWidth: true, visible: false },
         { data: "TMP_ITEM_NUMBER", name: "代紙料號", autoWidth: true, className: "dt-body-left" },
         { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
         { data: "REAM_WEIGHT", name: "令重", autoWidth: true },
         { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },
         { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right"},
         { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },

         { data: "REQUESTED_QUANTITY2", name: "預計出庫輔數量", autoWidth: true, className: "dt-body-right" },
         { data: "PICKED_QUANTITY2", name: "出庫已揀輔數量", autoWidth: true, className: "dt-body-right" },
         { data: "REQUESTED_QUANTITY_UOM2", name: "輔單位", autoWidth: true },

         { data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_PICKED_QUANTITY", name: "訂單已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true },

        ],
        //columnDefs: [
        //    {
        //        "targets": [4],
        //        "visible": false,
        //        "searchable": false
        //    }
        //    ],
        "order": [[1, 'asc']],
        select: {
            style: 'single',
            //blurable: true,
            //selector: 'td:first-child'
        },
        buttons: [

            'selectNone'
        ],
        "rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {
            if ($.inArray(data.ID, selected) !== -1) {
                var selectRow = ':eq(' + dataIndex + ')';
                FlatDataTablesBody.row(selectRow, { page: 'current' }).select();
            }
        },
        "preDrawCallback": function (settings) {
            $("#SUB_ID").text("");
            $("#DLV_DETAIL_ID").text("");
            $("#ORDER_NUMBER").text("");
            $("#ORDER_SHIP_NUMBER").text("");
            $("#INVENTORY_ITEM_ID").text("");
            $("#ITEM_NUMBER").text("");
            $('#PACKING_TYPE').text("");
        }

    });

    FlatDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var SUB_ID = dt.rows(indexes).data().pluck('SUB_ID')[0];
            $("#SUB_ID").text(SUB_ID);
            var DLV_DETAIL_ID = dt.rows(indexes).data().pluck('ID')[0];
            $("#DLV_DETAIL_ID").text(DLV_DETAIL_ID);
            //var FlatEditDT_ID = dt.rows(indexes).data().pluck('ID')[0];
            //$("#FlatEditDT_ID").text(FlatEditDT_ID);
            var ORDER_NUMBER = dt.rows(indexes).data().pluck('ORDER_NUMBER')[0];
            $("#ORDER_NUMBER").text(ORDER_NUMBER);
            var ORDER_SHIP_NUMBER = dt.rows(indexes).data().pluck('ORDER_SHIP_NUMBER')[0];
            $("#ORDER_SHIP_NUMBER").text(ORDER_SHIP_NUMBER);
            var TMP_ITEM_ID = dt.rows(indexes).data().pluck('TMP_ITEM_ID')[0];
            var INVENTORY_ITEM_ID = dt.rows(indexes).data().pluck('INVENTORY_ITEM_ID')[0];
            if (TMP_ITEM_ID) {
                $("#INVENTORY_ITEM_ID").text(TMP_ITEM_ID);
            } else {
                $("#INVENTORY_ITEM_ID").text(INVENTORY_ITEM_ID);
            }
            var TMP_ITEM_NUMBER = dt.rows(indexes).data().pluck('TMP_ITEM_NUMBER')[0];
            var ITEM_NUMBER = dt.rows(indexes).data().pluck('ITEM_NUMBER')[0];
            if (TMP_ITEM_NUMBER) {
                $("#ITEM_NUMBER").text(TMP_ITEM_NUMBER);
            } else {
                $("#ITEM_NUMBER").text(ITEM_NUMBER);
            }
            var PACKING_TYPE = dt.rows(indexes).data().pluck('PACKING_TYPE')[0];
            $('#PACKING_TYPE').text(PACKING_TYPE);
            

            var rowsData = FlatDataTablesBody.rows({ page: 'current' }).data();
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

    FlatDataTablesBody.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#SUB_ID").text("");
            $("#DLV_DETAIL_ID").text("");
            $("#ORDER_NUMBER").text("");
            $("#ORDER_SHIP_NUMBER").text("");
            $("#INVENTORY_ITEM_ID").text("");
            $("#ITEM_NUMBER").text("");
            $("#PACKING_TYPE").text("");
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


    var FlatBarcodeDataTablesBody = $('#FlatBarcodeDataTablesBody').DataTable({
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
            "url": "/Delivery/GetFlatEditBarcode",
            "type": "POST",
            "datatype": "json",
            "data": {
                DlvHeaderId: $("#DlvHeaderId").text()
            },
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "SUB_ID", name: "項次", autoWidth: true },
          { data: "BARCODE", name: "條碼號", autoWidth: true },
            { data: "ITEM_NUMBER", name: "料號", autoWidth: true, className: "dt-body-left" },
          { data: "REAM_WEIGHT", name: "令重", autoWidth: true },
         { data: "PACKING_TYPE", name: "包裝方式", autoWidth: true },
         { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
         { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
         { data: "SECONDARY_QUANTITY", name: "次要數量", autoWidth: true, className: "dt-body-right" },
         { data: "SECONDARY_UOM", name: "次要單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true, className: "dt-body-left" },
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
                         var selectedData = FlatBarcodeDataTablesBody.rows('.selected').data();
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

                     },
                     enabled: false
                 },
                {
                    text: '<span class="glyphicon glyphicon-print"></span>&nbsp列印標籤',
                    //className: 'btn-default btn-sm',
                    action: function (e) {
                        PrintLable(FlatBarcodeDataTablesBody, "/Home/GetLabel", "2");
                    },
                    className: "btn-primary",
                    enabled: false
                }
            ],
        }
    });




    $("#SECONDARY_QUANTITY").hide();
    $("#txtSECONDARY_QUANTITY").val("");

    $('#txtBARCODE').keydown(function (e) {
        if (e.keyCode == 13) {

            InputBarcode();

            //InputBarcode();
            //$('#txtSECONDARY_QUANTITY').attr('disabled', true);
        }
    })

    $('#txtSECONDARY_QUANTITY').keydown(function (e) {
        if (e.keyCode == 13) {
            InputBarcode();
        }
    })

    $("#btnSaveBarcode").click(function () {
        InputBarcode();
    });


    $("#txtBARCODE").on(
          "input propertychange paste", function () {
              //$("#SECONDARY_QUANTITY").hide();
              $("#txtSECONDARY_QUANTITY").val("");
              //$('#PACKING_TYPE').text("");
          });

    $("#btnCheckDeliveryName").click(function () {
        
        deliveryNameCheck();
    });

    $('#txtDeliveryName').keydown(function (e) {
        if (e.keyCode == 13) {
            deliveryNameCheck();
        }
    })

    function DeleteBarcode(selectedData) {
       

        var list = [];
        for (i = 0 ; i < selectedData.length; i++) {
            list.push(selectedData[i].PICKED_ID);
        }


        $.ajax({
            url: "/Delivery/DeleteFlatEditBarcode",
            type: "post",
            data: {
                'PICKED_ID': list,
                DlvHeaderId: $("#DlvHeaderId").text()
            },
            success: function (data) {
                if (data.status) {
                    if (data.result == "條碼刪除成功") {
                        FlatBarcodeDataTablesBody.ajax.reload();
                        FlatDataTablesBody.ajax.reload(null, false);
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


    function InputBarcode() {

        var DLV_DETAIL_ID = $('#DLV_DETAIL_ID').text();
        if (!DLV_DETAIL_ID) {
            swal.fire('請選擇料號');
            event.preventDefault();
            return false;
        }

        var BARCODE = $('#txtBARCODE').val();
        var SECONDARY_QUANTITY = $('#txtSECONDARY_QUANTITY').val();
        if (!SECONDARY_QUANTITY) {
            SECONDARY_QUANTITY = null;
        }
        $('#txtBARCODE').attr('disabled', true);

        $.ajax({
            url: "/Delivery/InputFlatEditBarcode",
            type: "post",
            data: {
                'BARCODE': BARCODE,
                'SECONDARY_QUANTITY': SECONDARY_QUANTITY,
                DlvHeaderId: $("#DlvHeaderId").text(),
                DLV_DETAIL_ID: $("#DLV_DETAIL_ID").text()
            },
            success: function (data) {
                if (data.status) {
                    if (data.result == "令包") {
                        //$("#SECONDARY_QUANTITY").show();
                        $('#txtSECONDARY_QUANTITY').focus();
                        //$('#PACKING_TYPE').text("令包");
                    }
                    if (data.result == "令包_條碼儲存成功") {
                        FlatBarcodeDataTablesBody.ajax.reload();
                        FlatDataTablesBody.ajax.reload(null, false);
                        UpdateDeliveryDetailViewHeader();
                    }
                    if (data.result == "無令打件_條碼儲存成功") {
                        //$('#PACKING_TYPE').text("無令打件");
                        FlatBarcodeDataTablesBody.ajax.reload();
                        FlatDataTablesBody.ajax.reload(null, false);
                        UpdateDeliveryDetailViewHeader();
                    }
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
    }

    function deliveryNameCheck() {
        var DELIVERY_NAME = $("#DELIVERY_NAME").text();

        if (DELIVERY_NAME == $("#txtDeliveryName").val()) {
            inputOpen();
        } else {
            swal.fire("交運單號輸入錯誤");
            event.preventDefault();
        }
    }

    function inputOpen() {
        FlatDataTablesBody.buttons().enable();
        FlatBarcodeDataTablesBody.buttons().enable();
        $('#txtBARCODE').attr('disabled', false);
        $('#txtSECONDARY_QUANTITY').attr('disabled', false);
        $('#btnSaveBarcode').attr('disabled', false);
      
    }

    function inpuClose() {
        //FlatDataTablesBody.buttons().disable(); //初始化時無效
        //FlatBarcodeDataTablesBody.buttons().disable(); //初始化時無效
        $('#txtBARCODE').attr('disabled', true);
        $('#txtSECONDARY_QUANTITY').attr('disabled', true);
        $('#btnSaveBarcode').attr('disabled', true);
    }
    inpuClose();
});



