var PaperRollInvestDataTables
var PaperRollProductionDataTables
var EditorInvest
var EditorProduction



$(document).ready(function () {

    BtnRecord();
    init();
    LoadPaperRollInvestDataTable();
    LoadPaperRollProductionDataTable();
    onclick();

    var Process_Status = $('#Process_Status').val();
    if (Process_Status == "已完工" || Process_Status == "待核准") {
  
        //完工紀錄使用
        DsiplayPaperRollHide();
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
    } else {
        DisplayInvestPaperRollEnable(true);
        DisplayProductionPaperRollEnable(true);
        DsiplayPaperRollShow();
    }

    //setTimeout(function () {
    //    var table = $('#PaperRollInvestDataTables').DataTable();
    //    if (table.data().length == 0) {
    //        DisplayPaperRollEnable(true);
    //    } else {
    //        DisplayPaperRollEnable(true);
    //    }
    //}, 100);

    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#PaperRollInvestDataTables').DataTable().table().node().style.width = '';


        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });

    PaperRollInvestDataTables.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorInvest.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });

    PaperRollInvestDataTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorInvest.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
    });

    PaperRollProductionDataTables.on('click', '#btnDeleteProductionTable', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorProduction.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });

    PaperRollProductionDataTables.on('click', '#btnEdit', function (e) {
        $('#Production_Loss').html("");
        e.preventDefault();
        EditorProduction.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });

        EditorProduction.on('preSubmit', function (e, d) {
            var Weight = this.field('Weight');
            if (Weight.val() === '') {
                this.field('Weight').error('請勿空白');
                return false;
            }
            return true;
        });
    });

    
})

function init() {

    //條碼欄位事件
    $('#PaperRoll_Invest_Barcode').change(function (e) {
        var Barcode = $('#PaperRoll_Invest_Barcode').val();
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        $.ajax({
            url: '/Process/Barcode',
            type: 'post',
            datatype: 'json',
            data: { Barcode: Barcode, Process_Detail_Id: Process_Detail_Id },
            success: function (data) {
                if (data.result == false) {
                    swal.fire("條碼無資料");
                    ClearTextPaperRoll();
                } else {
                    PaperRollDispalyText(data);
                }
            },
            error: function () {
                swal.fire("失敗");
            }
        });

    });

    //1 有殘捲 0 無殘捲
    $('#PaperRoll_Invest_Remaining_Weight').attr("disabled", true);
    $('#PaperRoll_Invest_Remnant').change(function (e) {
        var Remnant = $("#PaperRoll_Invest_Remnant").val();
        if (Remnant == 1) {
            $('#PaperRoll_Invest_Remaining_Weight').attr("disabled", false);
        } else {
            $('#PaperRoll_Invest_Remaining_Weight').val("");
            $('#PaperRoll_Invest_Remaining_Weight').attr("disabled", true);

        }

    });


    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#PaperRoll_Production_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $('#BtnPaperRollBarcodeSave').click();
        }
    });

    //列印標籤紙捲
    $('#BtnRePrint').click(function () {
        PrintLable(PaperRollInvestDataTables, "/Home/GetLabel", "1");
    });


    //列印標籤紙捲
    $('#BtnLabel').click(function () {
        PrintLable(PaperRollProductionDataTables, "/Home/GetLabel", "1");
    });


    $("#Process_Batch_no").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnOrderNumber").click();
        }
    });


}

function onclick() {

    //投入
    $('#BtnProcess_Batch_no').click(function (e) {
        var ProcessBatchNo = $('#ProcessBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayInvestPaperRollEnable(false);
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });

    //投出驗證單號
    $('#BtnProcess_Production_Batch_no').click(function (e) {
        var ProcessBatchNo = $('#ProcessProductionBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayProductionPaperRollEnable(false);
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });



    //儲存條碼
    $('#Btn_PaperRoll_ProcessSave').click(function () {
        var Barcode = $('#PaperRoll_Invest_Barcode').val().trim();
        var Remnant = $('#PaperRoll_Invest_Remnant').val();
        var Remaining_Weight = $('#PaperRoll_Invest_Remaining_Weight').val();
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        if (Barcode.length == 0) {
            swal.fire("投入條碼不得空白");
            return;
        }
        if (Remnant == "1") {
            if (Remaining_Weight.length == 0) {
                swal.fire("餘重不得空白");
                return;
            }

        }
        PaperInvestSaveBarcode(Barcode, Remnant, Remaining_Weight, Process_Detail_Id);
        $('#Btn_PaperRoll_ProcessSave').select();
    });


    //產出產生明細
    $('#Btn_PaperRoll_Product_detail').click(function () {
        var PaperRoll_Basic_Weight = $('#PaperRoll_Basic_Weight').val().trim();
        var PaperRoll_Specification = $('#PaperRoll_Specification').val().trim();
        var PaperRoll_Lot_Number = $('#PaperRoll_Lot_Number').val().trim();
        var PaperRollInvestDataTables = $('#PaperRollInvestDataTables').DataTable().data();
        var Product_Item = $('#PaperRoll_Product_Item').text();
        var Process_Detail_Id = $("#Process_Detail_Id").val();


        if (PaperRollInvestDataTables.length == 0) {
            swal.fire("請先新增投入條碼。");
            return;
        }

        if (PaperRoll_Basic_Weight.length == 0) {
            swal.fire("基重不得空白");
            return;
        }
        if (PaperRoll_Specification.length == 0) {
            swal.fire("寬幅不得空白");
            return;
        }
        if (PaperRoll_Lot_Number.length == 0) {
            swal.fire("捲號不得空白");
            return;
        }

        PaperRollProductionDetail(PaperRoll_Basic_Weight, PaperRoll_Specification, PaperRoll_Lot_Number, Product_Item, Process_Detail_Id);

    })

    $('#BtnPaperRollBarcodeSave').click(function () {

        var Production_Barcode = $('#PaperRoll_Production_Barcode').val();
        var table = $('#PaperRollProductionDataTables').DataTable();
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        if (Production_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return;
        }
        if (table.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return;
        }
        PaperRollChangeProductionStauts(Production_Barcode, Process_Detail_Id);
        $('#PaperRoll_Production_Barcode').select();
    })

    //計算損失
    $('#BtnPaperRollCalculate').click(function () {
        var Process_Detail_Id = $("#Process_Detail_Id").val()
        var Investtable = $('#PaperRollInvestDataTables').DataTable();
        var Productiontable = $('#PaperRollProductionDataTables').DataTable();
        if (Investtable.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return;
        }
        if (Productiontable.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return;
        }

        var PaperRollProductionDataTables = $('#PaperRollProductionDataTables').DataTable().column(6).data();

        for (i = 0; i < PaperRollProductionDataTables.length; i++) {
            if (PaperRollProductionDataTables[i] == "待入庫") {
                Swal.fire({
                    title: '產出資料錯誤',
                    text: "尚有資料未入庫",
                    icon: 'warning',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: '確定',
                    cancelButtonText: '取消'
                });
                return;
            }
        }
  
        var data = PaperRollInvestDataTables.column(6).data();
        var OriginalWeight = 0;
        var OW = data[0];
        //投入重量
        for (i = 0; i < OW.length; i++) {
            OriginalWeight += parseFloat(OW[i]);
        }
        var InvestWeight = OriginalWeight;


        //訂單總重
        var OrderWeight = $('#CHP_PROCESS_T_Weight').val();
        var ProductWeight = parseFloat(OrderWeight*100);
        var TotalWeight = (ProductWeight / InvestWeight).toFixed(2);
        var Percentage = ((InvestWeight / ProductWeight) * 100).toFixed(2);
        $.ajax({
            "url": "/Process/Loss",
            "type": "POST",
            "datatype": "json",
            "data": { TotalWeight: TotalWeight, Percentage: Percentage, Process_Detail_Id: Process_Detail_Id },
            success: function () {
                $('#Production_Loss').html("重量(KG)&ensp;" + TotalWeight + "&emsp;得率" + Percentage + "%");
            }
        });


    });

    $('#BtnReturn').click(function () {
        window.location.href = '/Process/Index';

    });

    $('#BtnSave').click(function () {
        var loss = $('#Production_Loss').text();

        var Process_Batch_no = $('#PaperRoll_Process_Batch_no').text();
        var PaperRollProductionDataTables = $('#PaperRollProductionDataTables').DataTable().column(6).data();

        for (i = 0; i < PaperRollProductionDataTables.length; i++) {
            if (PaperRollProductionDataTables[i] == "待入庫") {
                Swal.fire({
                    title: '產出資料錯誤',
                    text: "尚有資料未入庫",
                    icon: 'warning',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: '確定',
                    cancelButtonText: '取消'
                });
                return;
            }
        }

        if (loss.length == 0) {
            swal.fire("損耗量未計算");
            return;
        }

        $.ajax({
            url: '/Process/_Subinventory/',
            type: 'POST',
            success: function (result) {
                $('body').append(result);
                Open($('#SubinventoryModal'), Process_Batch_no);
            }
        });


    });
}

//初始化投入table
function LoadPaperRollInvestDataTable() {


    EditorInvest = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/InvestEdit',
            "type": "POST",
            "dataType": "json",
            "contentType": 'application/json',
            data: function (d) {
                var InvestList = [];
                $.each(d.data, function (key, value) {

                    var Invest = {
                        'Invest_Id': d.data[key]['Invest_Id'],
                        'Remnant': d.data[key]['Remnant'],
                        'Remaining_Weight': d.data[key]['Remaining_Weight']
                    }
                    InvestList.push(Invest);
                });

                var data = {
                    'Action': d.action,
                    'InvestList': InvestList
                }

                return JSON.stringify(data);
            },
            success: function () {
                LoadPaperRollInvestDataTable();
            }
        },
        table: "#PaperRollInvestDataTables",
        idSrc: 'Invest_Id',
        fields: [
            {
                label: "殘捲",
                name: "Remnant",
                type: "select",
                options: [
                    { label: "無", value: "無" },
                    { label: "有", value: "有" },
                ]
            },
            {
                label: "餘重:",
                name: "Remaining_Weight",
                attr: {
                    type: "number",
                    min: "0"
                }
            }
            ,
            {
                label: "ID:",
                name: "Invest_Id",
            }
        ],
        i18n: {
            edit: {
                button: "編輯",
                title: "更改原因",
                submit: "確定",
            },
        }


    });

    EditorInvest.hide('Invest_Id');

    EditorInvest.on('preSubmit', function (e, d) {
        var Remnant
        var Remaining_Weight
        $.each(d.data, function (key, values) {
            Remnant = d.data[key]['Remnant']
            Remaining_Weight = d.data[key]['Remaining_Weight']
        });

        if (Remnant == "有") {
            if (Remaining_Weight == '') {
                this.field('Remaining_Weight').error('請勿空白');
                return false;
            }
        } else {

        }

        return true;
    });

    EditorInvest.dependent('Remnant', function (val, data, callback) {
        if (val == '無') {
            EditorInvest.field('Remaining_Weight').set('');
        }
        return val === '無' ?
            { hide: 'Remaining_Weight' } : { show: 'Remaining_Weight' };


    });

    var Process_Detail_Id = $("#Process_Detail_Id").val();
    PaperRollInvestDataTables = $('#PaperRollInvestDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        //processing: true,
        //serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/InvestLoadTable",
            "type": "POST",
            "datatype": "json",
            "data": { Process_Detail_Id: Process_Detail_Id }
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        columnDefs: [{
            orderable: false, targets: [0, 7], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Basic_Weight", "name": "基重", "autoWidth": true, "className": "dt-body-center"},
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center"},
            { data: "Lot_Number", "name": "捲號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Paper_Type", "name": "紙別", "autoWidth": true, "className": "dt-body-center"},
            { data: "Original_Weight", "name": "原重", "autoWidth": true, "className": "dt-body-right"},
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                    //'<button class="btn btn-primary btn-sm" id= "btnEdit">編輯</button>' +
                        //'&nbsp|&nbsp' +
                }
            }
        ],

    });



}

//投入條碼
function PaperInvestSaveBarcode(Barcode, Remnant, Remaining_Weight, Process_Detail_Id) {
    $.ajax({
        "url": "/Process/InvestTable",
        "type": "POST",
        "datatype": "json",
        "data": { Barcode: Barcode, Remnant: Remnant, Remaining_Weight: Remaining_Weight, Process_Detail_Id: Process_Detail_Id },
        success: function (data) {
            if (data.check == 0) {
                LoadPaperRollInvestDataTable();
            } else if (data.check == 1) {
                swal.fire("條碼資料已存在");
            } else if (data.check == 2) {
                swal.fire("資料不存在");
            }
        }


    });

}


//初始化產出table
function LoadPaperRollProductionDataTable() {

    EditorProduction = new $.fn.dataTable.Editor({
        ajax: {
            url: '/Process/ProductionEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var ProductionList = [];
                $.each(d.data, function (key, value) {

                    var Production = {
                        'Production_Id': d.data[key]['Production_Id'],
                        'Roll_Ream_Wt': d.data[key]['Roll_Ream_Wt'],
                        'Weight': d.data[key]['Weight']

                    }
                    ProductionList.push(Production);
                });

                var data = {
                    'Action': d.action,
                    'ProductionList': ProductionList
                }

                return JSON.stringify(data);
            },
            success: function () {
                //LoadProductionDataTable();
            }
        },
        table: "#PaperRollProductionDataTables",
        idSrc: 'Production_Id',
        fields: [
            {
                label: "重量",
                name: "Weight"
            },
            {
                name: "Production_Id"
            },
        ],
        i18n: {
            edit: {
                button: "更改",
                title: "更改",
                submit: "確定"
            },
        }

    });

    EditorProduction.field('Production_Id').hide();

    var Process_Detail_Id = $("#Process_Detail_Id").val();
    PaperRollProductionDataTables = $('#PaperRollProductionDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        //processing: true,
        //serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/ProductionLoadDataTables",
            "type": "POST",
            "datatype": "json",
            "data": { Process_Detail_Id: Process_Detail_Id },
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        columnDefs: [{
            orderable: false, targets: [0, 7], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left"},
            { data: "Lot_Number", "name": "捲數", "autoWidth": true, "className": "dt-body-center" },
            { data: "Weight", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return 'KG';
                }
            },
            {
                data: "Status", "name": "狀態", "autoWidth": true, "className": "dt-body-center", "render": function (data) {
                    if (data == "已入庫") {
                        return data;
                    } else {
                        return '<span style="color:red">' + data + '</span>';
                    }
                }
            },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class="btn btn-primary btn-sm" id= "btnEdit">編輯</button>' + '&nbsp|&nbsp' +
                        '<button class = "btn btn-danger btn-sm" id = "btnDeleteProductionTable">刪除</button>' ;
                }
            }
        ],

    });

}

function PaperRollProductionDetail(PaperRoll_Basic_Weight, PaperRoll_Specification, PaperRoll_Lot_Number ,Product_Item, Process_Detail_Id) {

    $.ajax({
        "url": "/Process/PaperRollProductionDetail",
        "type": "POST",
        "datatype": "json",
        "data": {
            PaperRoll_Basic_Weight: PaperRoll_Basic_Weight,
            PaperRoll_Specification: PaperRoll_Specification,
            PaperRoll_Lot_Number: PaperRoll_Lot_Number,
            Product_Item: Product_Item,
            Process_Detail_Id: Process_Detail_Id
        },
        success: function (data) {
            if (data.boolean) {
                LoadPaperRollProductionDataTable();
            } else {
                swal.fire("");
            }
        }
    })


}

function PaperRollChangeProductionStauts(Production_Barcode, Process_Detail_Id) {
    $.ajax({
        "url": "/Process/ProductionChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { Production_Barcode: Production_Barcode, Process_Detail_Id: Process_Detail_Id },
        success: function (data) {
            if (data.check == 1) {
                swal.fire("此條碼已入庫")
            } else if (data.check == 2) {
                LoadPaperRollProductionDataTable();
            } else if (data.check == 3) {
                swal.fire("無條碼資料");
            }
        }
    })
}


function ClearTextPaperRoll() {
    $('#PaperRoll_Invest_Original_Weight').html("");
    $('#PaperRoll_Item_No').html("");
    $('#PaperRoll_Invest_Lot_Number').html("");
    $('#PaperRoll_Invest_Barcode').val('');
}

function PaperRollDispalyText(data) {
    $('#PaperRoll_Invest_Original_Weight').html(data.cpd.Original_Weight);
    $('#PaperRoll_Item_No').html(data.cpd.Item_No);
    $('#PaperRoll_Invest_Lot_Number').html(data.cpd.Lot_Number);
}

function EnableBarcode(boolean) {
    $('#PaperRoll_Invest_Barcode').attr('disabled', boolean);
    $('#Btn_PaperRoll_Product_detail').attr('disabled', boolean);

}


//彈出dialog
function Open(modal_dialog, Process_Batch_no) {
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

    //確認按鍵
    modal_dialog.on('click', '#btnSaveSubinventory', function (e) {
        var Subinventory = $('#dialg_Subinventory').val();



        $.ajax({
            url: '/Process/ChagneIndexStatus/',
            dataType: 'json',
            type: 'post',
            data: { Process_Batch_no: Process_Batch_no, Subinventory: Subinventory },
            success: function (e) {
                window.location.href = '/Process/Index';
            }
        })



    });



    modal_dialog.modal('show');

}
//完工紀錄使用
function BtnRecord() {
    $('#BtnEdit').click(function () {
        var ProcessBatchNo = $('#ProcessBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayInvestPaperRollEnable(false);
                    changeStaus();
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });

    $('#BtnApprove').click(function () {
        var Process_Batch_no = $('#PaperRoll_Process_Batch_no').text();
        var Status = "核准";
        $.ajax({
            url: '/Process/ChagneIndexStatus/',
            dataType: 'json',
            type: 'post',
            data: { Process_Batch_no: Process_Batch_no, Status: Status },
            success: function (e) {
                window.location.href = '/Process/Index';
            }
        });


    });




}

//完工紀錄使用
function DisplayInvestPaperRollEnable(boolean) {
    $('#PaperRoll_Invest_Barcode').attr('disabled', boolean);
    $('#Btn_PaperRoll_ProcessSave').attr('disabled', boolean);

    

}

function DisplayProductionPaperRollEnable(boolean) {
    $('#PaperRoll_Basic_Weight').attr('disabled', boolean);
    $('#PaperRoll_Specification').attr('disabled', boolean);
    $('#PaperRoll_Lot_Number').attr('disabled', boolean);
    $('#Btn_PaperRoll_Product_detail').attr('disabled', boolean);
    $('#PaperRoll_Production_Barcode').attr('disabled', boolean);
    $('#BtnPaperRollBarcodeSave').attr('disabled', boolean);
    $('#BtnPaperRollCalculate').attr('disabled', boolean);

}


function DsiplayPaperRollHide() {
    $('#BtnProcess_Batch_no').hide()
    $("#BtnSave").hide();
    $('#BtnEdit').show()
    $('#BtnApprove').show()

}

function DsiplayPaperRollShow() {
    $('#BtnProcess_Batch_no').show()
    $('#BtnEdit').hide()
    $('#BtnApprove').hide()
}

//完工紀錄使用
function changeStaus() {
    var Process_Batch_no = $('#ProcessBatchNo').val();
    var Status = "完工紀錄";
    $.ajax({
        url: '/Process/ChagneIndexStatus/',
        dataType: 'json',
        type: 'post',
        data: { Process_Batch_no: Process_Batch_no, Status: Status },
        success: function (e) {
            //window.location.href = '/Process/Index';
        }
    });
}