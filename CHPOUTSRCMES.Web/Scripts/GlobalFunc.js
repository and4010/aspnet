
var GlobalFunc = {

    logout : function () {

    Swal.fire({
        title: '登出確認?',
        text: "您是否確定要登出系統?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '登出!'
    })
        .then((result) => {
        if (result.value) {
            $.ajax({
                url: '/Account/LogOff',
                type: "POST",
                data: {
                    '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val();
                },
                success: function (data) {
                    window.location.replace('~/Home/Index');
                },
                error: function () {
                    swal("登出失敗!!");
                }

            })
        }
    });
    }
}