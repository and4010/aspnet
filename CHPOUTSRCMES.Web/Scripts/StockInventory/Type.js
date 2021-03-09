$(document).ready(function () {
    init();
});

function init() {
    $('#ddlProfit').change(function () {

        var inventoryType = $("#ddlProfit").val();
        if (inventoryType == "請選擇") {
            $('#Content').empty();
            return;
        }
        select(inventoryType);
    });
}

function select(inventoryType) {
    $.ajax({
        url: "/StockInventory/GetContent",
        type: "GET",
        dataType: 'html',
        data: {
            inventoryType: inventoryType
        },
        success: function (data) {
            $('#Content').empty();
            $('#Content').html(data);
            if (inventoryType == "356") {
                ProfitInit(); //盤盈
            } else {
                LossInit(); //盤虧
            }
        },
        error: function () {
            swal.fire('更新內容失敗');
        },
        complete: function (data) {


        }

    })
}

