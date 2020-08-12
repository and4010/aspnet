
function GetTop() {
    $("#txtItemNumberArea").toggleClass('border-0')
    $.ajax({
        url: "/StockTransaction/GetTop",
        type: "GET",
        dataType: 'html',
        data: {},
        success: function (data) {
            $('#Top').empty();
            $('#Top').html(data);
            TopInit();

            //if (TransferType == "出庫") {
            //    OutBoundInit();
            //} else {
            //    InBoundInit();
            //}
        },
        error: function () {
            swal.fire('更新倉庫搜尋頁面失敗');
        },
        complete: function (data) {


        }

    })
}


function TopInit() {

    $('#ddlSubinventory').change(function () {

        var SUBINVENTORY_CODE = $("#ddlSubinventory").val();
        $.ajax({
            url: "/StockTransaction/GetLocatorListForUserId",
            type: "post",
            data: {
                SUBINVENTORY_CODE: SUBINVENTORY_CODE
            },
            success: function (data) {
                $('#ddlLocator').empty();
                for (var i = 0; i < data.length; i++) {
                    $('#ddlLocator').append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }
                //GetItemNumberList();
                if (data.length == 1) {
                    $('#ddlLocatorArea').hide();
                } else {
                    $('#ddlLocatorArea').show();
                }

            },
            error: function () {
                swal.fire('更新儲位失敗');
            },
            complete: function (data) {

                SubinventoryChangeCallBack();
            }

        })


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
                    SubinventoryCode: $("#ddlSubinventory").val(),
                    Locator: $("#ddlLocator").val(),
                    Prefix: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Description, value: item.Value };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            if (ui.item) {
                AutoCompleteItemNumberSelectCallBack(ui.item.value);
            }
        }
    });

    //$('#txtItemNumber').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        $(this).data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: $(this).val() } });
    //        //AutoCompleteItemNumberEnterCallBack();
    //    }
    //});
}