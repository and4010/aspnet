var QueryTable;


$(document).ready(function () {

    $('#PlanStartDateFrom').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    $('#PlanStartDateTo').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    $('#btnPrint').click(function () {
        var batchNo = getBatchNo();
        if (batchNo && batchNo.length < 4) {
            swal.fire("工單號須輸入4碼以上");
            return;
        }
        //$('#ReportBox').show();
        ShowWait(function () {
            $.ajax({
                url: "/Report/OspCutSumReport",
                type: "post",
                data: {
                    planStartDateFrom: getPlanStartDateFrom(),
                    planStartDateTo: getPlanStartDateTo(),
                    batchNo: batchNo,
                    paperType: getPaperType()
                },
                success: function (model) {
                    CloseWait();
                    $("#ReportPartial").html(model);
                },
                error: function () {
                    swal.fire('更新報表失敗');
                }
            });
        });
    });
});

function getPlanStartDateFrom() {
    return $("#PlanStartDateFrom").val();
}

function getPlanStartDateTo() {
    return $("#PlanStartDateTo").val();
}

function getBatchNo() {
    return $("#BatchNo").val();
}

function getPaperType() {
    return $("#PaperType").val();
}