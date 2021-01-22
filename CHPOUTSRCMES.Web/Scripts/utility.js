
function ShowWait(callBack) {
    swal.fire({
        title: "請等待...",
        allowOutsideClick: false,
        allowEscapeKey: false,
        onOpen: function () {
            callBack();
        }
    })
    swal.showLoading();
    //if (!swal.isVisible()) {
    //    swal.fire({
    //        title: "請等待...",
    //        allowOutsideClick: false,
    //        allowEscapeKey: false,
    //        onOpen: function () {
    //            callBack();
    //        }
    //    })
    //    swal.showLoading();
    //}
}


function CloseWait() {
    swal.close();
    //if (swal.isVisible()) {
    //    swal.close();
    //}
}