var TransferReasonTable;
var selected = [];
var LocatorAreaBoolean;

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

function TransferReasonInit() {
    $(":file").filestyle({ buttonText: "選擇照片" });
    TransferReasonTableInit();
    GetTop();
    OnClick();
    photoView();
    


    ////重新整理表格寬度
    //$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

    //    // Need to reset width on table, not sure why.
    //    $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

    //    // .draw() is necessary for "no results" message to be properly positioned
    //    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    //});


};

function OnClick() {
    $('#btnSearchStock').click(function () {
        SearchStock();
    });


    $('#BtnSave').click(function () {
        if (selected.length == 0) {
            swal.fire("請先選擇一個項目");
            return;
        }


        var StockId = selected[0];
        var ReasonCode = $('#Reason').val();
        var TransferLocatorId = $('#TransferLocator').val();
        var Note = $('#Note').val();
        if (LocatorAreaBoolean) {
            if (TransferLocatorId == "請選擇") {
                swal.fire("請先選擇儲位");
                return;
            }
        } else {
            TransferLocatorId = null;
        }

        if (Reason == "請選擇") {
            swal.fire("請先選擇原因");
            return;
        }

        SaveReason(StockId, ReasonCode, TransferLocatorId, Note);

    });

}

function TransferReasonTableInit() {

    TransferReasonTable = $('#TransferReasonTable').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        orderMulti: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/StockTransaction/SearchStock",
            "type": "Post",
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
            { data: "BARCODE", name: "條碼號", autoWidth: true },
            { data: "PRIMARY_AVAILABLE_QTY", name: "數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_UOM_CODE", name: "單位", autoWidth: true },
            {
                data: "SECONDARY_AVAILABLE_QTY", name: "數量", autoWidth: true, className: "dt-body-right", "mRender": function (data, type, full) {
                    if (data != null) {
                        if (data == 0) {
                            return '';
                        }
                        return data;
                    } else {
                        return '';
                    }
                }},
            { data: "SECONDARY_UOM_CODE", name: "單位", autoWidth: true },
            { data: "REASON_DESC", name: "原因", autoWidth: true },
            { data: "NOTE", name: "備註", autoWidth: true },
        ],

        "order": [[1, 'desc']],
        select: {
            style: 'single',
            //blurable: true,
            //selector: 'td:first-child'
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

    TransferReasonTable.on('select', function (e, dt, type, indexes) {
        if (type = 'row') {
            var StockId = dt.rows(indexes).data().pluck('ID')[0];
            $("#StockId").text(StockId);
            var SUB_ID = dt.rows(indexes).data().pluck('SUB_ID')[0];
            $("#SUB_ID").text(SUB_ID);
            var SUBINVENTORY_CODE = dt.rows(indexes).data().pluck('SUBINVENTORY_CODE')[0];
            $("#TransferReasonSubinventory").text(SUBINVENTORY_CODE);
            var SEGMENT3 = dt.rows(indexes).data().pluck('SEGMENT3')[0];
            $("#TransferReasonLocator").text(SEGMENT3);
            var LOCATOR_ID = dt.rows(indexes).data().pluck('LOCATOR_ID')[0];
            $("#TransferLocator").val(LOCATOR_ID);
            var ITEM_NO = dt.rows(indexes).data().pluck('ITEM_NO')[0];
            $('#TransferReasonItem').text(ITEM_NO);
            var BARCODE = dt.rows(indexes).data().pluck('BARCODE')[0];
            $("#TransferReasonBarcode").text(BARCODE);
            var REASON_CODE = dt.rows(indexes).data().pluck('REASON_CODE')[0];
            //var REASON_DESC = dt.rows(indexes).data().pluck('REASON_DESC')[0];
            //var Reason = REASON_CODE + "-" + REASON_DESC;
            if (REASON_CODE) {
                $("#Reason").val(REASON_CODE);
            }
            var NOTE = dt.rows(indexes).data().pluck('NOTE')[0];
            $('#Note').val(NOTE)


            var rowsData = TransferReasonTable.rows({ page: 'current' }).data();
            for (i = 0; i < rowsData.length; i++) {
                for (j = 0; j < selected.length; j++) {
                    if (selected[j] == rowsData[i].ID) {
                        selected.splice(j, 1);
                    }
                }
            }

            //var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(StockId, selected);
            if (index === -1) {
                selected.push(StockId);
            }

        }

    });

    TransferReasonTable.on('deselect', function (e, dt, type, indexes) {
        if (type = 'row') {
            clearText();
            var StockId = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(StockId, selected);
            selected.splice(index, 1);
        }

    });

}

function SaveReason(StockId, ReasonCode, TransferLocatorId, Note) {
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }
    formData.append("stockId", StockId);
    formData.append("reasonCode", ReasonCode);
    formData.append("transferLocatorId", TransferLocatorId);
    formData.append("note", Note);

    $.ajax({
        url: "/StockTransaction/SaveReason",
        type: "post",
        dataType: 'json',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (data) {
            if (data.status) {
                TransferReasonTable.ajax.reload();
                clearText();
                clearContent('#imgBox'); //清除預覽圖片
                $('#photo_form').trigger("reset"); //清除圖片檔名
                swal.fire(data.result);
            } else {
                swal.fire(data.result);
            }

        },
        error: function () {
            swal.fire('儲存貨故失敗');
        },
        complete: function (data) {

        }

    });
}

//已存檔入庫預設照片
function photoView() {
    $('#fileinput').on("change", function () {
        imgName = [];
        imgSrc = [];
        imgFile = [];
        files = [];
        var file = $('#fileinput')[0];
        var fileList = file.files;
        for (var i = 0; i < fileList.length; i++) {
            var imgSrcI = getObjectURL(fileList[i]);
            imgName.push(fileList[i].name);
            imgSrc.push(imgSrcI);
            imgFile.push(fileList[i]);
            files.push(fileList[i]);
        }
        //photo.js使用
        addNewContent($('#imgBox'));
    });

}



function SubinventoryChangeCallBack() {
    //clear();
    clearText();
    var SUBINVENTORY_CODE = getSubinventoryCode();
    $.ajax({
        url: "/StockTransaction/GetLocatorListForUserId",
        type: "post",
        data: {
            SUBINVENTORY_CODE: SUBINVENTORY_CODE
        },
        success: function (data) {
            if (data.length == 1) {
                $('#TransferLocatorArea').hide();
                LocatorAreaBoolean = false;
            } else {
                $('#TransferLocatorArea').show();
                LocatorAreaBoolean = true;
            }
            $('#TransferLocator').empty();
            for (var i = 0; i < data.length; i++) {
                $('#TransferLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
            }
        },
        error: function () {
            swal.fire('更新儲位失敗');
        },
        complete: function (data) {
        }

    });
}


function LocatorChangeCallBack() {
    //clear();
    //clearText();
}

function clear() {
    //LoadTransferReasonTable.ajax.reload(null, false);
    selected = [];
   
}

function clearText() {
    $("#TransferLocator").val("請選擇");
    $("#Reason").val("請選擇");
    $("#StockId").text("");
    $("#SUB_ID").text("");
    $("#Note").val("");
    $("#TransferReasonId").text("");
    $("#TransferReasonSubinventory").text("");
    $("#TransferReasonLocator").text("");
    $('#TransferReasonItem').text("");
    $("#TransferReasonBarcode").text("");
}

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

    TransferReasonTable.ajax.reload(null, false);
}

function AutoCompleteItemNumberSelectCallBack(ITEM_NO) {
    //$("#txtItemNumber").val(ITEM_NO);
    $('#btnSearchStock').focus();
    //SearchStock();
}

function LostFocusItemNumberCallBack(ITEM_NO) {

   

}