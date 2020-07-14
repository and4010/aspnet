$(document).ready(function () {
    $(document).on('click', '#btnConfirm', function (e) {
        DeliveryConfirm();
        return false;
    });

    $(document).on('click', '#btnCancelConfirm', function (e) {
        CancelConfirm();
        return false;
    });

    $(document).on('click', '#btnAuthorize', function (e) {
        DeliveryAuthorize();
        return false;
    });

    $(document).on('click', '#btnCancelAuthorize', function (e) {
        CancelAuthorize();
        return false;
    });

});

function UpdateDeliveryDetailViewHeader() {

    $.ajax({
        url: "/Delivery/UpdateDeliveryDetailViewHeader",
        type: "post",
        data: {
            TripDetailDT_ID: $("#TripDetailDT_ID").text()
        },
        success: function (model) {
            $("#DeliveryPartial").html(model);
        },
        error: function () {
            swal.fire('明細輸入抬頭更新失敗');
        }
    });
}



function DeliveryConfirm() {
   
   
    var list = [];
    list.push($("#TripDetailDT_ID").text());
    

    $.ajax({
        url: "/Delivery/DeliveryConfirm",
        type: "post",
        data: {
            'id': list
        },
        success: function (data) {
            if (data.status) {
                if (data.result == "出貨確認成功") {
                    UpdateDeliveryDetailViewHeader();
                }
            }
            else {
                swal.fire(data.result);
            }
        },
        error: function () {
            swal.fire('出貨申請失敗');
        },
        complete: function (data) {


        }

    });
}

function CancelConfirm() {
    var list = [];
    list.push($("#TripDetailDT_ID").text());

    $.ajax({
        url: "/Delivery/CancelConfirm",
        type: "post",
        data: {
            'id': list
        },
        success: function (data) {
            if (data.status) {
                if (data.result == "取消出貨確認成功") {
                    UpdateDeliveryDetailViewHeader();
                }
            }
            else {
                swal.fire(data.result);
            }
        },
        error: function () {
            swal.fire('取消申請失敗');
        },
        complete: function (data) {


        }

    });
}

function PrintPickList() {
    var list = [];
    list.push($("#TripDetailDT_ID").text());

    $.ajax({
        url: "/Delivery/PrintPickList",
        type: "post",
        data: {
            'id': list
        },
        success: function (data) {
            if (data.status) {
                if (data.result == "列印備貨單成功") {
                    UpdateDeliveryDetailViewHeader();
                }
            }
            else {
                swal.fire(data.result);
            }
        },
        error: function () {
            swal.fire('列印備貨單失敗');
        },
        complete: function (data) {


        }

    });
}

function DeliveryAuthorize() {
    var list = [];
    list.push($("#TripDetailDT_ID").text());

    $.ajax({
        url: "/Delivery/DeliveryAuthorize",
        type: "post",
        data: {
            'id': list
        },
        success: function (data) {
            if (data.status) {
                if (data.result == "出貨核准成功") {
                    UpdateDeliveryDetailViewHeader();
                }
            }
            else {
                swal.fire(data.result);
            }
        },
        error: function () {
            swal.fire('出貨核准失敗');
        },
        complete: function (data) {


        }

    });
}

function CancelAuthorize() {
    var list = [];
    list.push($("#TripDetailDT_ID").text());

   
    $.ajax({
        url: "/Delivery/CancelAuthorize",
        type: "post",
        data: {
            'id': list
        },
        success: function (data) {
            if (data.status) {
                if (data.result == "取消出貨核准成功") {
                    UpdateDeliveryDetailViewHeader();
                }
            }
            else {
                swal.fire(data.result);
            }
        },
        error: function () {
            swal.fire('取消出貨核准失敗');
        },
        complete: function (data) {


        }

    });
}