var InvestDataTables
var ProductionTables
var CotangentDataTable
var EditorCotangent
var editorInvest
var editorProduct

$(document).ready(function () {
    LoadInvestDataTable();
    LoadProductionDataTable();
    init();
    BtnClick();
    BtnRecordEdit();
    CotangentDataTables();

    var Process_Status = $('#Process_Status').val();
    if (Process_Status == "已完工" || Process_Status == "待核准") {
        //完工紀錄使用
        DsiplayHide();
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
    } else {
        DisplayInvestEnable(true);
        DisplayProductionEnable(true);
        DsiplayShow();
    }

    //setTimeout(function () {
    //    var table = $('#InvestDataTables').DataTable();
    //    if (table.data().length == 0) {
    //        DisplayEnable(true);
    //    } else {
    //        //EnableBarcode(false);
    //        DisplayEnable(true);
    //    }
    //}, 100);



    //if ($("#Process_Detail_Id").val() == 1) {
       
    //}
 

    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#InvestDataTables').DataTable().table().node().style.width = '';
        $('#ProductionDataTables').DataTable().table().node().style.width = '';
        $('#CotangentDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });

    InvestDataTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        editorInvest.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
    });

    InvestDataTables.on('click', '#btnDelete', function (e) {

        e.preventDefault();
        $('#Production_Loss').html("");
        editorInvest.remove($(this).closest('tr'), {
            title: '刪除',
            message: '確定要刪除?',
            buttons: '確定'
        });
    });


    ProductionTables.on('click', '#btnDeleteProductionTable', function (e) {
        $('#Production_Loss').html("");
        editorProduct.remove($(this).closest('tr'), {
            title: '刪除',
            message: '確定要刪除?',
            buttons: '確定'
        });
    });

    ProductionTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        editorProduct.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
        editorProduct.on('preSubmit', function (e, d) {
            var Roll_Ream_Wt = this.field('Roll_Ream_Wt');
            var Weight = this.field('Weight');

            if (Weight.val() === '') {
                this.field('Weight').error('請勿空白');
                return false;
            }
            if (Roll_Ream_Wt.val() === '') {
                this.field('Roll_Ream_Wt').error('請勿空白');
                return false;
            }

            return true;
        });

    });

    CotangentDataTable.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorCotangent.remove($(this).closest('tr'), {
            title: '刪除',
            message: '確定要刪除?',
            buttons: '確定'
        });
    });

    CotangentDataTable.on('click', '#btnInsert', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorCotangent.edit($(this).closest('tr'), {
            title: '新增',
            buttons: '確定'
        });
        EditorCotangent.on('preSubmit', function (e, d) {
            var Ttl_Roll_Ream = this.field('Cotangent_Ttl_Roll_Ream');
            if (Ttl_Roll_Ream.val() === '') {
                this.field('Cotangent_Ttl_Roll_Ream').error('請勿空白');
                return false;
            }

            return true;
        });
    });


    InvestDataTables.on('select', function (e, dt, items, indexes) {
        var rowData = InvestDataTables.rows(indexes).data();
        displaytext(rowData)
    });


    InvestDataTables.on('deselect', function (e, dt, items, indexes) {
        clear()
    });
})

function init() {
    //1 有殘捲 0 無殘捲
    $('#Invest_Remaining_Weight').attr("disabled", true)
    $('#Invest_Remnant').change(function (e) {
        var Remnant = $("#Invest_Remnant").val();
        if (Remnant == 1) {
            $('#Invest_Remaining_Weight').attr("disabled", false);
        } else {
            $('#Invest_Remaining_Weight').val("");
            $('#Invest_Remaining_Weight').attr("disabled", true);

        }

    });

    //在關鍵字input按下Enter，執行n送出 紙捲
    $("#Production_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnProductionSave").click();
        }
    });

    $("#Process_Batch_no").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnOrderNumber").click();
        }
    });

    $("#Cotangent_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnCotangentSave").click();
        }
    });




    $('#Invest_Barcode').change(function (e) {
        var Barcode = $('#Invest_Barcode').val();
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        $.ajax({
            url: '/Process/Barcode',
            type: 'post',
            datatype: 'json',
            data: { Barcode: Barcode, Process_Detail_Id: Process_Detail_Id },
            success: function (data) {
                if (data.result == false) {
                    swal.fire("條碼無資料");
                    ClearText();
                } else {
                    DispalyText(data);
                }
            },
            error: function () {
                swal.fire("失敗");
            }
        });

    });






}

function BtnClick() {

    //投入驗證單號
    $('#BtnProcess_Batch_no').click(function (e) {
        var ProcessBatchNo = $('#ProcessBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayInvestEnable(false);
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
                    DisplayProductionEnable(false);
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });


    $('#BtnProcessSave').click(function () {
        var Barcode = $('#Invest_Barcode').val().trim();
        var Remnant = $('#Invest_Remnant').val()
        var Remaining_Weight = $('#Invest_Remaining_Weight').val()
        var Process_Detail_Id = $("#Process_Detail_Id").val()
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

        InvestSaveBarcode(Barcode, Remnant, Remaining_Weight, Process_Detail_Id)
        $('#Invest_Barcode').select();
    });


    //產出產生明細
    $('#BtnProduct_detail').click(function () {
        var Production_Roll_Ream_Qty = $('#Production_Roll_Ream_Qty').val().trim();
        var Production_Roll_Ream_Wt = $('#Production_Roll_Ream_Wt').val().trim();
        var Production_Cotangent = $('#Production_Cotangent').val();
        var InvestDataTables = $('#InvestDataTables').DataTable().data();
        var Product_Item = $('#Product_Item').text();
        var Process_Detail_Id = $("#Process_Detail_Id").val();


        if (InvestDataTables.length == 0) {
            swal.fire("請先新增投入條碼。");
            return;
        }

        if (Production_Roll_Ream_Qty.length == 0) {
            swal.fire("棧板數不得空白");
            return;
        }
        if (Production_Roll_Ream_Wt.length == 0) {
            swal.fire("令數不得空白");
            return;
        }
        ProductionDetail(Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Product_Item, Process_Detail_Id)
        if (Production_Cotangent == "1") {
            CotangentDataTables(Production_Cotangent);
        }
    })

    $('#BtnProductionSave').click(function () {

        var Production_Barcode = $('#Production_Barcode').val();
        var table = $('#ProductionDataTables').DataTable();
        var Process_Detail_Id = $("#Process_Detail_Id").val()
        if (Production_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return;
        }
        if (table.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return;
        }
        ChangeProductionStauts(Production_Barcode, Process_Detail_Id);
        $('#Production_Barcode').select();
    })

    $('#BtnCotangentSave').click(function () {
        var Cotangent_Barcode = $('#Cotangent_Barcode').val();
        var Cotangent = $('#CotangentDataTables').DataTable().column(3).data();
        if (Cotangent_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return
        }
        for (i = 0; i < Cotangent.length; i++) {
            if (Cotangent[i] == "") {
                swal.fire("請先輸入令數。");
                return;
            }
        }

        ChagneCotangent(Cotangent_Barcode);
        $('#Cotangent_Barcode').select();
    })

    //計算損號
    $('#BtnCalculate').click(function () {
        var Process_Detail_Id = $("#Process_Detail_Id").val()
        var table = $('#ProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("產出無資料，請先輸入資料。")
            return
        }
        var Production = $('#ProductionDataTables').DataTable().column(7).data();
        var Cotangent = $('#CotangentDataTables').DataTable().column(7).data();

        for (i = 0; i < Production.length; i++) {
            if (Production[i] == "待入庫") {
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
        for (i = 0; i < Cotangent.length; i++) {
            if (Cotangent[i] == "待入庫") {
                Swal.fire({
                    title: '餘切資料錯誤',
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
        var data = InvestDataTables.columns([7, 8]).data();
        var OriginalWeight = 0;
        var Remaining_Weight = 0;
        var OW = data[0];
        var RW = data[1];
        //投入重量
        for (i = 0; i < OW.length; i++) {
            OriginalWeight += parseFloat(OW[i]);
            if (RW[i] != null) {
                Remaining_Weight += (RW[i].length == 0 ? 0 : parseFloat(RW[i]));
            }

        }
        var InvestWeight = OriginalWeight - Remaining_Weight;


        //餘切總重
        if (CotangentDataTable != null) {
            var Cotangent = CotangentDataTable.columns(3).data();
            var s = (Cotangent[0].length == 0 ? 0 : parseFloat(Cotangent[0]));
            if (Cotangent[0].length == 1) {
                if (Cotangent[0][0] == "") {
                    swal.fire("請先輸入令數");
                    return;
                }

            }
            var Cotangent = CotangentDataTable.columns(5).data();
            var s = (Cotangent[0].length == 0 ? 0 : parseFloat(Cotangent[0]));
            if (Cotangent[0].length == 1) {
                if (Cotangent[0][0] == "") {
                    swal.fire("請先輸入公斤");
                    return;
                }

            }
        }


        //訂單總重
        var OrderWeight = $('#CHP_PROCESS_T_Weight').val();
        var ProductWeight = parseFloat(OrderWeight) + s != null ? OrderWeight * 100 : parseFloat(s);
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
        var Production = $('#ProductionDataTables').DataTable().column(7).data();
        var Cotangent = $('#CotangentDataTables').DataTable().column(7).data();
        var Process_Batch_no = $('#Process_Batch_no').text()
        for (i = 0; i < Production.length; i++) {
            if (Production[i] == "待入庫") {
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
        for (i = 0; i < Cotangent.length; i++) {
            if (Cotangent[i] == "待入庫") {
                Swal.fire({
                    title: '餘切資料錯誤',
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



    //列印標籤紙捲
    $('#BtnRePrint').click(function () {
        PrintLable(InvestDataTables, "/Home/GetLabel", "1");
    })


    //列印標籤紙捲
    $('#BtnLabel').click(function () {
        PrintLable(ProductionTables, "/Home/GetLabel", "1", $('#CotangentDataTables').DataTable(), "1");
    })



}

//初始化投入table
function LoadInvestDataTable() {

    editorInvest = new $.fn.dataTable.Editor({
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
                LoadInvestDataTable();
            }
        },
        table: "#InvestDataTables",
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
            remove: {
                button: '刪除',
                title: "刪除",
                submit: "確定",
                confirm: {
                    "_": "你確定要刪除這筆資料?",
                    "1": "你確定要刪除這筆資料?"
                }
            },
        }


    });

    editorInvest.hide('Invest_Id');

    editorInvest.on('preSubmit', function (e, d) {
        var Remnant
        var Remaining_Weight
        $.each(d.data, function (key, values) {
            Remnant = d.data[key]['Remnant']
            Remaining_Weight = d.data[key]['Remaining_Weight']
        })

        if (Remnant == "有") {
            if (Remaining_Weight == '') {
                this.field('Remaining_Weight').error('請勿空白')
                return false;
            }
        } else {

        }

        return true;
    });

    editorInvest.dependent('Remnant', function (val, data, callback) {
        if (val == '無') {
            editorInvest.field('Remaining_Weight').set('');
        }
        return val === '無' ?
            { hide: 'Remaining_Weight' } : { show: 'Remaining_Weight' };

    });

    var Process_Detail_Id = $("#Process_Detail_Id").val()
    InvestDataTables = $('#InvestDataTables').DataTable({
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
        columnDefs: [{
            orderable: false, targets: [0, 9], width: "60px",
        }],
        buttons: [
            'selectAll',
            'selectNone',

        ],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Remnant", "name": "殘捲", "autoWidth": true, "className": "dt-body-center" },
            { data: "Basic_Weight", "name": "基重", "autoWidth": true, "className": "dt-body-center" },
            { data: "Specification", "name": "寬幅", "autoWidth": true, "className": "dt-body-center" },
            { data: "Lot_Number", "name": "捲號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Paper_Type", "name": "紙別", "autoWidth": true, "className": "dt-body-center" },
            { data: "Original_Weight", "name": "原重", "autoWidth": true, "className": "dt-body-right" },
            { data: "Remaining_Weight", "name": "餘重", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>' +
                        '&nbsp|&nbsp' + '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            }
        ],

    });
}

function InvestSaveBarcode(Barcode, Remnant, Remaining_Weight, Process_Detail_Id) {
    $.ajax({
        "url": "/Process/InvestTable",
        "type": "POST",
        "datatype": "json",
        "data": { Barcode: Barcode, Remnant: Remnant, Remaining_Weight: Remaining_Weight, Process_Detail_Id: Process_Detail_Id },
        success: function (data) {
            if (data.check == 0) {
                LoadInvestDataTable();
            } else if (data.check == 1) {
                swal.fire("條碼資料已存在");
            } else if (data.check == 2) {
                swal.fire("資料不存在");
            }
        }


    });

}

//初始化產出table
function LoadProductionDataTable() {

    editorProduct = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
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
        table: "#ProductionDataTables",
        idSrc: 'Production_Id',
        fields: [
            {
                label: "重量",
                name: "Weight"
            },
            {
                name: "Production_Id"
            },

            {
                label: "令數",
                name: "Roll_Ream_Wt"
            },
        ],
        i18n: {
            edit: {
                button: "新增",
                title: "更改原因",
                submit: "確定"
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
        }
    });

    editorProduct.field('Production_Id').hide();


    var Process_Detail_Id = $("#Process_Detail_Id").val()
    ProductionTables = $('#ProductionDataTables').DataTable({
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
            'selectNone',

        ],
        columnDefs: [{
            orderable: false, targets: [0, 8], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "Weight", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    return 'KG';
                }
            },
            { data: "Roll_Ream_Wt", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    return 'RE';
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
                    return '<button class = "btn btn-primary btn-sm" id = "btnEdit">編輯</button>' +
                        '&nbsp|&nbsp' + '<button class = "btn btn-danger btn-sm" id = "btnDeleteProductionTable">刪除</button>';
                }
            }
        ],

    });

}

//產生明細
function ProductionDetail(Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Product_Item, Process_Detail_Id) {

    $.ajax({
        "url": "/Process/ProductionDetail",
        "type": "POST",
        "datatype": "json",
        "data": {
            Production_Roll_Ream_Qty: Production_Roll_Ream_Qty, Production_Roll_Ream_Wt: Production_Roll_Ream_Wt
            , Product_Item: Product_Item, Process_Detail_Id: Process_Detail_Id
        },
        success: function (data) {
            if (data.boolean) {
                LoadProductionDataTable();
            } else {
                swal.fire(data.msg);
            }
        }
    })

}

function ChangeProductionStauts(Production_Barcode, Process_Detail_Id) {
    $.ajax({
        "url": "/Process/ProductionChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { Production_Barcode: Production_Barcode, Process_Detail_Id: Process_Detail_Id },
        success: function (data) {
            if (data.check == 1) {
                swal.fire("此條碼已入庫");
            } else if (data.check == 2) {
                LoadProductionDataTable();
            } else if (data.check == 3) {
                swal.fire("無條碼資料");
            }
        }
    })
}

//餘切初始化Table
function CotangentDataTables(Production_Cotangent, Product_Item) {


    EditorCotangent = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/CotangentEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var CotangentList = [];
                $.each(d.data, function (key, value) {

                    var Cotangent = {
                        'Cotangent_Id': d.data[key]['Cotangent_Id'],
                        //'Kg': d.data[key]['Kg'],
                        'Cotangent_Ttl_Roll_Ream': d.data[key]['Cotangent_Ttl_Roll_Ream']

                    }
                    CotangentList.push(Cotangent);
                });

                var data = {
                    'Action': d.action,
                    'CotangentList': CotangentList
                }

                return JSON.stringify(data);
            },
            success: function () {
                CotangentDataTables();
            }
        },
        table: "#CotangentDataTables",
        idSrc: 'Cotangent_Id',
        fields: [
            //{
            //    label: "公斤",
            //    name: "Kg",
            //    attr: {
            //        type: "number",
            //        min: "0"
            //    }
            //},
            {
                label: "令數:",
                name: "Cotangent_Ttl_Roll_Ream",
                attr: {
                    type: "number",
                    min: "0"
                }
            }
            ,
            {
                name:"Cotangent_Id"
            }
        ],
        i18n: {
            edit: {
                button: "新增",
                title: "更改原因",
                submit: "確定"
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
        }


    });

    EditorCotangent.field('Cotangent_Id').hide();

    var Process_Detail_Id = $("#Process_Detail_Id").val()
    CotangentDataTable = $('#CotangentDataTables').DataTable({
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
            "url": "/Process/CotangentDataTables",
            "type": "POST",
            "datatype": "json",
            "data": { Production_Cotangent: Production_Cotangent, Product_Item: Product_Item, Process_Detail_Id: Process_Detail_Id },
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
            orderable: false, targets: [0, 8], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center" },
            { data: "Related_item", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "Kg", "name": "餘切", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    return 'KG';
                }
            },
            { data: "Cotangent_Ttl_Roll_Ream", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function (data) {
                    return 'RE';
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
                    return '<button class="btn btn-primary btn-sm" id = "btnInsert">新增</button>' +
                        '&nbsp|&nbsp' + '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            },
        ],
    });

}

function ChagneCotangent(CotangentBarcode) {
    $.ajax({
        "url": "/Process/CotangentChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { CotangentBarcode: CotangentBarcode },
        success: function (data) {
            if (data.check == 1) {
                swal.fire("此條碼已入庫");
            } else if (data.check == 2) {
                CotangentDataTables();
            } else if (data.check == 3) {
                swal.fire("無條碼資料");
            }
        }
    })
}

function DispalyText(data) {
    $('#Invest_Original_Weight').html(data.cpd.Original_Weight);
    $('#Invest_Basic_Weight').html(data.cpd.Basic_Weight);
    $('#Invest_Specification').html(data.cpd.Specification);
    $('#Invest_Lot_Number').html(data.cpd.Lot_Number);
    $('#Item_No').html(data.cpd.Item_No);
}

function ClearText() {
    $('#Invest_Original_Weight').html("");
    $('#Invest_Basic_Weight').html("");
    $('#Invest_Specification').html("");
    $('#Invest_Lot_Number').html("");
    $('#Invest_Remaining_Weight').val("");
    $('#Invest_Barcode').val("");
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
        });



    });



    modal_dialog.modal('show');

}


//完工紀錄使用
function BtnRecordEdit() {
    $('#BtnEdit').click(function () {

        var ProcessBatchNo = $('#ProcessBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayInvestEnable(false);
                    changeStaus();
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });




        //var loss = $('#Production_Loss').text();
        //var Process_Batch_no = $('#Process_Batch_no').text()
        //var Status = "完工紀錄";
        //if (loss.length == 0) {
        //    swal.fire("損耗量未計算");
        //    return;
        //}



    });

    $('#BtnApprove').click(function () {
        var Process_Batch_no = $('#Process_Batch_no').text()
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

function DsiplayHide() {
    $('#BtnProcess_Batch_no').hide()
    $('#BtnSave').hide();
    $('#BtnEdit').show()
    $('#BtnApprove').show()
}

function DsiplayShow() {
    $('#BtnProcess_Batch_no').show()
    $('#BtnEdit').hide()
    $('#BtnApprove').hide()
}


function DisplayInvestEnable(boolean) {
    $('#Invest_Barcode').attr('disabled', boolean);
    $('#Invest_Remnant').attr('disabled', boolean);
    $('#BtnProcessSave').attr('disabled', boolean);
  


}

function DisplayProductionEnable(boolean) {

    $('#Production_Roll_Ream_Qty').attr('disabled', boolean);
    $('#Production_Roll_Ream_Wt').attr('disabled', boolean);
    $('#Production_Cotangent').attr('disabled', boolean);
    $('#BtnProduct_detail').attr('disabled', boolean);
    $('#Production_Barcode').attr('disabled', boolean);
    $('#BtnProductionSave').attr('disabled', boolean);
    $('#BtnCotangentSave').attr('disabled', boolean);
    $('#Cotangent_Barcode').attr('disabled', boolean);
    $('#BtnCalculate').attr('disabled', boolean);
}



//完工紀錄使用
function displaytext(rowData) {
    var Barcode = rowData.pluck('Barcode')[0]
    var Invest_Remnant = rowData.pluck('Remnant')[0]
    var Invest_Original_Weight = rowData.pluck('Original_Weight')[0]
    var Invest_Remaining_Weight = rowData.pluck('Remaining_Weight')[0]
    var Invest_Basic_Weight = rowData.pluck('Basic_Weight')[0]
    var Invest_Specification = rowData.pluck('Specification')[0]
    var Invest_Lot_Number = rowData.pluck('Lot_Number')[0]



    $('#Invest_Barcode').text(Barcode);
    //$('#Invest_Remnant').text(Invest_Remnant);
    $('#Invest_Original_Weight').text(Invest_Original_Weight);
    $('#Invest_Remaining_Weight').text(Invest_Remaining_Weight);
    $('#Invest_Basic_Weight').text(Invest_Basic_Weight);
    $('#Invest_Specification').text(Invest_Specification);
    $('#Invest_Lot_Number').text(Invest_Lot_Number);

}

function clear() {
    $('#Invest_Barcode').text("");
    //$('#Invest_Remnant').text("");
    $('#Invest_Original_Weight').text("");
    $('#Invest_Remaining_Weight').text("");
    $('#Invest_Basic_Weight').text("");
    $('#Invest_Specification').text("");
    $('#Invest_Lot_Number').text("");
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