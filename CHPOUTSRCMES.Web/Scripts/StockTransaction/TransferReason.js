var TransferReasonTable;
var selected = [];
var LocatorAreaBoolean;
function TransferReasonInit() {
    $(":file").filestyle({ buttonText: "選擇照片" });
    LoadTransferReasonTable();
    GetTop();
    OnClick();
    photoView();
    TransferReasonTable.on('select', function (e, dt, type, indexes) {
        if (type = 'row') {
            var ID = dt.rows(indexes).data().pluck('ID')[0];
            $("#TransferReasonId").text(ID);
            var SUBINVENTORY_CODE = dt.rows(indexes).data().pluck('SUBINVENTORY_CODE')[0];
            $("#TransferReasonSubinventory").text(SUBINVENTORY_CODE);
            var SEGMENT3 = dt.rows(indexes).data().pluck('SEGMENT3')[0];
            $("#TransferReasonLocator").text(SEGMENT3);
            var LOCATOR_ID = dt.rows(indexes).data().pluck('LOCATOR_ID')[0];
            $("#Locator").val(LOCATOR_ID);
            var ITEM_NO = dt.rows(indexes).data().pluck('ITEM_NO')[0];
            $('#TransferReasonItem').text(ITEM_NO);
            var BARCODE = dt.rows(indexes).data().pluck('BARCODE')[0];
            $("#TransferReasonBarcode").text(BARCODE);
            var REASON_CODE = dt.rows(indexes).data().pluck('REASON_CODE')[0];
            var REASON_DESC = dt.rows(indexes).data().pluck('REASON_DESC')[0];
            var Reason = REASON_CODE + "-" + REASON_DESC;
            if (REASON_CODE.length != 0) {
                $("#Reason").val(Reason);
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

            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            if (index === -1) {
                selected.push(ID);
            }

        }

    });

    TransferReasonTable.on('deselect', function (e, dt, type, indexes) {
        if (type = 'row') {
            clearText();
            var ID = dt.rows(indexes).data().pluck('ID')[0];
            var index = $.inArray(ID, selected);
            selected.splice(index, 1);
        }

    });


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });


};

function OnClick() {
    $('#btnSearch').click(function () {
        SearchStock();
    });


    $('#BtnSave').click(function () {
        if (selected.length == 0) {
            swal.fire("請先選擇一個項目");
            return;
        }


        var ID = selected[0];
        var Reason = $('#Reason').val();
        var indexof = Reason.indexOf('-');
        var Code = Reason.substring(0, indexof);
        var Desc = Reason.substring(indexof + 1);
        var SaveLocator = $('#Locator').val()
        var Note = $('#Note').val()
        if (LocatorAreaBoolean) {
            if (SaveLocator == "請選擇") {
                swal.fire("請先選擇儲位");
                return;
            }
        }

        if (Reason == "請選擇") {
            swal.fire("請先選擇原因");
            return;
        }

        SaveReason(ID, Code, Desc, SaveLocator, Note)

    });

}

function LoadTransferReasonTable(SubinventoryCode, Locator, ItemNumber) {
    TransferReasonTable = $('#TransferReasonTable').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        destroy: true,
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
            "url": "/StockTransaction/SearchStock",
            "type": "Post",
            "datatype": "json",
            "data": {
                SubinventoryCode: SubinventoryCode,
                Locator: Locator,
                ItemNumber: ItemNumber
            },
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            { data: "ID", name: "項次", autoWidth: true },
            { data: "SUBINVENTORY_CODE", name: "倉庫", autoWidth: true },
            { data: "SEGMENT3", name: "儲位", autoWidth: true },
            { data: "ITEM_NO", name: "料號", autoWidth: true, className: "dt-body-left" },
            { data: "BARCODE", name: "條碼號", autoWidth: true },
            { data: "PRIMARY_TRANSACTION_QTY", name: "數量", autoWidth: true, className: "dt-body-right" },
            { data: "PRIMARY_UOM_CODE", name: "單位", autoWidth: true },
            { data: "SECONDARY_TRANSACTION_QTY", name: "數量", autoWidth: true, className: "dt-body-right" },
            { data: "SECONDARY_UOM_CODE", name: "單位", autoWidth: true },
            { data: "REASON_DESC", name: "原因", autoWidth: true },
            { data: "NOTE", name: "備註", autoWidth: true },
        ],

        "order": [[1, 'asc']],
        select: {
            style: 'single',
            //blurable: true,
            //selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],

    });






}

function SaveReason(ID, Code, Desc, Locator, Note) {
    $.ajax({
        url: "/StockTransaction/SaveReason",
        type: "post",
        dataType :'json',
        data: {
            ID: ID,
            REASON_CODE: Code,
            REASON_DESC: Desc,
            Locator: Locator,
            NOTE: Note

        },
        success: function (data) {
            if (data.status) {
                TransferReasonTable.ajax.reload();
                clearText();
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
        var file = $('#fileinput')[0];
        var fileList = file.files;
        for (var i = 0; i < fileList.length; i++) {
            var imgSrcI = getObjectURL(fileList[i]);
            imgName.push(fileList[i].name);
            imgSrc.push(imgSrcI);
            imgFile.push(fileList[i]);
        }
        addNewContent($('#imgBox'));
    });

}



function SubinventoryChangeCallBack() {
    clear();
    clearText();
    var SUBINVENTORY_CODE = $("#ddlSubinventory").val();
    $.ajax({
        url: "/StockTransaction/GetLocatorList",
        type: "post",
        data: {
            SUBINVENTORY_CODE: SUBINVENTORY_CODE
        },
        success: function (data) {
            if (data.length == 1) {
                $('#LocatorArea').hide();
                LocatorAreaBoolean = false;
            } else {
                $('#LocatorArea').show();
                LocatorAreaBoolean = true;
            }
            $('#Locator').empty();
            for (var i = 0; i < data.length; i++) {
                $('#Locator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
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
    clear();
    clearText();
}

function clear() {
    LoadTransferReasonTable()
    selected = [];
   
}

function clearText() {
    $("#Locator").val("請選擇");
    $("#Reason").val("請選擇");
    $("#Note").val("");
    $("#TransferReasonId").text("");
    $("#TransferReasonSubinventory").text("");
    $("#TransferReasonLocator").text("");
    $('#TransferReasonItem').text("");
    $("#TransferReasonBarcode").text("");
}

function SearchStock() {
    var SubinventoryCode = $("#ddlSubinventory").val();
    var Locator = $("#ddlLocator").val();
    var ItemNumber = $("#txtItemNumber").val();
    LoadTransferReasonTable(SubinventoryCode, Locator, ItemNumber)
}

function AutoCompleteItemNumberSelectCallBack(ITEM_NO) {
    $("#txtItemNumber").val(ITEM_NO);
    SearchStock();
}