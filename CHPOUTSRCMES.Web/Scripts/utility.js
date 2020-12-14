
function ShowWait(callBack) {
    if (!swal.isVisible()) {
        swal.fire({
            title: "請等待...",
            allowOutsideClick: false,
            allowEscapeKey: false,
            onOpen: function () {
                callBack();
            }
        })
        swal.showLoading();
    }
}


function CloseWait() {
    if (swal.isVisible()) {
        swal.close();
    }
}