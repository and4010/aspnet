var selected = [];

$(document).ready(function () {




    var PaperRollDataTablesBody = $('#PaperRollDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },

        //destroy: true,
        autoWidth: false,
        //rowId: 'PaperRollEditDT_ID',
        //select: true,
        "serverSide": true,
        processing: true,
        orderMulti: true,
        //pageLength: 2,
        //"paging": true,
        //"pagingType": "full_numbers",
        //"lengthMenu": [[1, 10, 5, 2], [1, 10, 5, 2]],
        //stateSave: true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetRollEdit",
            "type": "POST",
            "datatype": "json",
            "data": {
                TripDetailDT_ID: $("#TripDetailDT_ID").text()
            }
        },
        columns: [
         { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
         { data: "ID", name: "項次", autoWidth: true },
         { data: "ORDER_NUMBER", name: "訂單", autoWidth: true },
         { data: "ORDER_SHIP_NUMBER", name: "訂單行號", autoWidth: true },
         { data: "OSP_BATCH_NO", name: "工單號碼", autoWidth: true },
         { data: "TMP_ITEM_NUMBER", name: "代紙料號", autoWidth: true, className: "dt-body-left" },
         { data: "ITEM_DESCRIPTION", name: "料號", autoWidth: true, className: "dt-body-left" },
         { data: "PAPER_TYPE", name: "紙別", autoWidth: true },
         { data: "BASIC_WEIGHT", name: "基重", autoWidth: true },
         { data: "SPECIFICATION", name: "規格", autoWidth: true },
         { data: "REQUESTED_QUANTITY", name: "預計出庫量", autoWidth: true, className: "dt-body-right" },
         { data: "PICKED_QUANTITY", name: "出庫已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "REQUESTED_QUANTITY_UOM", name: "庫存單位", autoWidth: true },
         { data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_PICKED_QUANTITY", name: "訂單已揀數量", autoWidth: true, className: "dt-body-right" },
         { data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", autoWidth: true },
         //{ data: "REMARK", name: "備註", autoWidth: true },


        ],

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
            if ($.inArray(data.DT_RowId, selected) !== -1) {
                //$(row).find('input[type="checkbox"]').prop('checked', true);
                var selectRow = ':eq(' + dataIndex + ')';
                PaperRollDataTablesBody.row(selectRow, { page: 'current' }).select();
                //$(row).toggleClass('selected');
                //$("#PaperRollEditDT_ID").text(data.PaperRollEditDT_ID);
                //$("#ORDER_NUMBER").text(data.ORDER_NUMBER);
                //$("#ORDER_SHIP_NUMBER").text(data.ORDER_SHIP_NUMBER);
                //$("#ITEM_DESCRIPTION").text(data.ITEM_DESCRIPTION);

            }
        },
        //"drawCallback": function( settings ) {
        //    $("#PaperRollEditDT_ID").text("");
        //    $("#ORDER_NUMBER").text("");
        //    $("#ORDER_SHIP_NUMBER").text("");
        //    $("#ITEM_DESCRIPTION").text("");
        //},
        "preDrawCallback": function (settings) {
            $("#PaperRollEditDT_ID").text("");
            $("#ORDER_NUMBER").text("");
            $("#ORDER_SHIP_NUMBER").text("");
            $("#ITEM_DESCRIPTION").text("");
        }
        //"initComplete": function(settings, json) {
        //    $("#PaperRollEditDT_ID").text("");
        //    $("#ORDER_NUMBER").text("");
        //    $("#ORDER_SHIP_NUMBER").text("");
        //    $("#ITEM_DESCRIPTION").text("");
        //}

    });

    //PaperRollDataTablesBody.on('order.dt search.dt', function () {
    //    PaperRollDataTablesBody.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();

    PaperRollDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var PaperRollEditDT_ID = dt.rows(indexes).data().pluck('ID')[0];
            $("#PaperRollEditDT_ID").text(PaperRollEditDT_ID);
            var ORDER_NUMBER = dt.rows(indexes).data().pluck('ORDER_NUMBER')[0];
            $("#ORDER_NUMBER").text(ORDER_NUMBER);
            var ORDER_SHIP_NUMBER = dt.rows(indexes).data().pluck('ORDER_SHIP_NUMBER')[0];
            $("#ORDER_SHIP_NUMBER").text(ORDER_SHIP_NUMBER);
            var ITEM_DESCRIPTION = dt.rows(indexes).data().pluck('ITEM_DESCRIPTION')[0];
            $("#ITEM_DESCRIPTION").text(ITEM_DESCRIPTION);

            var rowsData = PaperRollDataTablesBody.rows({ page: 'current' }).data();
            for (i = 0 ; i < rowsData.length; i++) {
                for (j = 0; j < selected.length; j++) {
                    if (selected[j] == rowsData[i].DT_RowId) {
                        selected.splice(j, 1);
                    }
                }
            }

            var DT_RowId = dt.rows(indexes).data().pluck('DT_RowId')[0];
            var index = $.inArray(DT_RowId, selected);
            if (index === -1) {
                selected.push(DT_RowId);
            }

            $('#txtBARCODE').focus().select();
        }
    });

    PaperRollDataTablesBody.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {
            $("#PaperRollEditDT_ID").text("");
            $("#ORDER_NUMBER").text("");
            $("#ORDER_SHIP_NUMBER").text("");
            $("#ITEM_DESCRIPTION").text("");

            var DT_RowId = dt.rows(indexes).data().pluck('DT_RowId')[0];
            var index = $.inArray(DT_RowId, selected);
            selected.splice(index, 1);
        }
    });

    //$('#PaperRollDataTablesBody tbody').on('click', 'tr', function () {
    //    var id = this.id;
    //    var index = $.inArray(id, selected);

    //    if (index === -1) {
    //        var rowsData = PaperRollDataTablesBody.rows({ page: 'current' }).data();
    //        for (i = 0 ; i < rowsData.length; i++) {
    //            for (j = 0; j < selected.length; j++) {
    //                if (selected[j] == rowsData[i].DT_RowId) {
    //                    selected.splice(j, 1);
    //                }
    //            }
    //            //selected.splice(rowsData[i].DT_RowId, 1);
    //        }
    //        //$(p).each(function () {
    //        //    selected.splice(p.id, 1);
    //        //});
    //        //selected.length = 0;
    //        //PaperRollDataTablesBody.$('tr.selected').removeClass('selected');
    //        selected.push(id);
    //        //table.row(this).select();
    //        //$(this).addClass('selected');
    //    } else {
    //        selected.splice(index, 1);
    //        //$(this).removeClass('selected');
    //    }

    //    //$(this).toggleClass('selected');
    //});


    var PaperRollBarcodeDataTablesBody = $('#PaperRollBarcodeDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        //destroy: true,
        autoWidth: false,
        orderMulti: true,
        //pageLength: 2,
        serverSide: true,
        processing: true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/GetRollEditBarcode",
            "type": "Post",
            "datatype": "json",
            "data": {
                TripDetailDT_ID: $("#TripDetailDT_ID").text()
            },
        },
        columns: [
         { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
         { data: "PaperRollEditDT_ID", name: "項次", autoWidth: true },
         { data: "ITEM_DESCRIPTION", name: "料號", autoWidth: true, className: "dt-body-left" },
         { data: "BARCODE", name: "條碼號", autoWidth: true },
         { data: "PRIMARY_QUANTITY", name: "主要數量", autoWidth: true, className: "dt-body-right" },
         { data: "PRIMARY_UOM", name: "主要單位", autoWidth: true },
         { data: "REMARK", name: "備註", autoWidth: true, className: "dt-body-left" },
         { data: "LAST_UPDATE_DATE", name: "更新日期", autoWidth: true, visible: false }

        ],

        "order": [[6, 'desc']],
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
                    var selectedData = PaperRollBarcodeDataTablesBody.rows('.selected').data();
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
                    PrintLable(PaperRollBarcodeDataTablesBody, "/Home/GetLabel", "3");
                },
                className: "btn-primary",
                enabled: false
            }
            ],
        }


    });

    //ImportPaperRollBarcodeEditDT();
    //ImportPaperRollEditDT();

    $('#txtBARCODE').keydown(function (e) {
        if (e.keyCode == 13) {
            InputBarcode();
        }
    })



    $("#btnSaveBarcode").click(function () {
        InputBarcode();
    });


    $("#btnCheckDeliveryName").click(function () {
        deliveryNameCheck();

    });

    $('#txtDeliveryName').keydown(function (e) {
        if (e.keyCode == 13) {
            deliveryNameCheck();
        }
    })

    function InputBarcode() {
        var PaperRollEditDT_ID_Text = $('#PaperRollEditDT_ID').text();
        if (!PaperRollEditDT_ID_Text) {
            swal.fire('請選擇料號');
            event.preventDefault();
            return false;
        }

        $('#txtBARCODE').attr('disabled', true);

        $.ajax({
            url: "/Delivery/InputRollEditBarcode",
            type: "post",
            data: {
                BARCODE: $('#txtBARCODE').val(),
                TripDetailDT_ID: $("#TripDetailDT_ID").text(),
                PaperRollEditDT_ID: $("#PaperRollEditDT_ID").text()
            },
            success: function (data) {
                if (data.status) {

                    if (data.result == "條碼儲存成功") {
                        //ImportPaperRollBarcodeEditDT();
                        //ImportPaperRollEditDT();

                        PaperRollDataTablesBody.ajax.reload(null, false);
                        PaperRollBarcodeDataTablesBody.ajax.reload();
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
                //event.preventDefault();

            }

        });

    }

    function DeleteBarcode(selectedData) {


        var list = [];
        for (i = 0 ; i < selectedData.length; i++) {
            list.push(selectedData[i].PICKED_ID);
        }


        $.ajax({
            url: "/Delivery/DeleteRollEditBarcode",
            type: "post",
            data: {
                'PICKED_ID': list,
                TripDetailDT_ID: $("#TripDetailDT_ID").text(),
            },
            success: function (data) {
                if (data.status) {
                    if (data.result == "條碼刪除成功") {
                        //ImportPaperRollBarcodeEditDT();
                        //ImportPaperRollEditDT();
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
        PaperRollDataTablesBody.buttons().enable();
        PaperRollBarcodeDataTablesBody.buttons().enable();
        $('#txtBARCODE').attr('disabled', false);
        $('#btnSaveBarcode').attr('disabled', false);

    }

    function inpuClose() {
        //PaperRollDataTablesBody.buttons().disable(); //初始化時無效
        //PaperRollBarcodeDataTablesBody.buttons().disable(); //初始化時無效
        $('#txtBARCODE').attr('disabled', true);
        $('#btnSaveBarcode').attr('disabled', true);
    }
    inpuClose();
});



