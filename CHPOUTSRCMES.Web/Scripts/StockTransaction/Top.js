
function GetTop() {
    $("#txtItemNumberArea").toggleClass('border-0')
    TopInit();
}


function TopInit() {

    $('#ddlSubinventory').change(function () {

        var SUBINVENTORY_CODE = $("#ddlSubinventory option:selected").text();

        ShowWait(function () {
            $.ajax({
                url: "/StockTransaction/GetLocatorListForUserId",
                type: "post",
                data: {
                    SUBINVENTORY_CODE: SUBINVENTORY_CODE
                },
                success: function (data) {
                    CloseWait();
                    $('#ddlLocator').empty();
                    for (var i = 0; i < data.length; i++) {
                        $('#ddlLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                    }
                    if (data.length == 1) {
                        $('#ddlLocatorArea').hide();
                        $('#ddlLocator').hide();
                    } else {
                        $('#ddlLocatorArea').show();
                        $('#ddlLocator').show();
                    }

                },
                error: function () {
                    swal.fire('更新儲位失敗');
                },
                complete: function (data) {

                    SubinventoryChangeCallBack();
                }

            })
        });
       
    })

    $('#ddlLocator').change(function () {
        LocatorChangeCallBack();
    })

    $("#txtItemNumber").autocomplete({
        autoFocus: true,
        source: function (request, response) {
            $.ajax({
                url: "/StockTransaction/AutoCompleteItemNumber",
                type: "POST",
                dataType: "json",
                data: {
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Value + " " + item.Description, value: item.Value };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            if (ui.item) {
                $('#txtItemNumber').val(ui.item.value);
                AutoCompleteItemNumberSelectCallBack(ui.item.value);
            }
        }
    });

    $('#txtItemNumber').blur(function () {
        LostFocusItemNumberCallBack($('#txtItemNumber').val());
    });

}