$(document).ready(function () {

    btnDailog();
});

//更改密碼dialog
function btnDailog() {
    $('#BtnchagnePassword').click(function () {
        ShowWait(function () {
            $.ajax({
                url: '/Account/_ChangePassword',
                type: "POST",
                datatype: 'json',
                success: function (result) {
                    CloseWait();
                    $('body').append(result);
                    Open($('#changePasswordModal'));
                },
                error: function () {
                    swal.fire("修改密碼失敗")
                }
            });
        });
    });
}


//彈出dialog
function Open(modal_dialog) {
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
    modal_dialog.on('click', '#btnConfirm', function (e) {
        var OldPassword = $('#OldPassword').val().trim();
        var NewPassword = $('#NewPassword').val().trim();
        var ConfirmPassword = $('#ConfirmPassword').val().trim();

        if (OldPassword.length == 0) {
            swal.fire("舊密碼不得空白。");
            return
        }
        if (NewPassword.length == 0) {
            swal.fire("新密碼不得空白。");
            return
        }
        if (ConfirmPassword.length == 0) {
            swal.fire("確認密碼不得空白。");
            return
        }

        if (NewPassword != ConfirmPassword) {
            swal.fire("新密碼與確認密碼要相同。");
            return
        }

        var ManageUserViewModel = {
            OldPassword: OldPassword,
            NewPassword: NewPassword,
            ConfirmPassword: ConfirmPassword,
        };
        //Add validation token
        ManageUserViewModel.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

        ShowWait(function () {
            $.ajax({
                url: '/Account/ChangPasswordAsync',
                type: "POST",
                datatype: 'json',
                data: { ManageUserViewModel: ManageUserViewModel },
                success: function (data) {
                    if (data.resultModel.Success) {
                        CloseWait();
                        swal.fire("更改成功");
                        modal_dialog.modal('hide');
                    } else {
                        swal.fire(data.resultModel.Msg);
                    }
                },
                error: function () {
                    swal.fire("失敗");
                }

            });
        });
        
    });

    modal_dialog.modal('show');

}