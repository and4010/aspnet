$(document).ready(function () {
    //select("請選擇"); 
    init();
});

function init() {
    $('#ddlProfit').change(function () {

        var TransactionType = $("#ddlProfit").val();
        if (TransactionType == "請選擇") {
            $('#Content').empty();
            return;
        }
        select(TransactionType);
    });
}

function select(TransactionType) {
    $.ajax({
        url: "/StockInventory/GetContent",
        type: "GET",
        dataType: 'html',
        data: {
            TransactionType: TransactionType
        },
        success: function (data) {
            $('#Content').empty();
            $('#Content').html(data);
            if (TransactionType == "盤盈") {
                ProfitInit();
            } else {
                LossInit();
            }
        },
        error: function () {
            swal.fire('更新內容失敗');
        },
        complete: function (data) {


        }

    })
}

