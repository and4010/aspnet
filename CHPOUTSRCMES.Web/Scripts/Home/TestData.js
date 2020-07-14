$(document).ready(function () {

    $("#btnRecoverTestData").click(function () {
        RecoverTestData();
    });

    function RecoverTestData() {

        $.ajax({
            url: "/Home/RecoverTestData",
            type: "post",
            data: {},
            success: function (data) {
                if (data.status) {
                    swal.fire(data.result);
                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('測試資料還原失敗');
            },
            complete: function (data) {


            }

        });

    }
});