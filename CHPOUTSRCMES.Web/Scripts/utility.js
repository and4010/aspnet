
function ShowWait(callBack) {
    //Swal.fire('請等待...');
    //Swal.showLoading();

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


function CloseWait() {
    swal.close();
}