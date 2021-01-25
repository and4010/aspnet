
function ShowWait(callBack) {
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

//不檢查swal是否已開啟，強制開新的swal。通常用在swal.fire裡，因為swal.isVisible()會判斷錯誤
function ShowWait2(callBack) {
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
    //if (swal.isVisible()) {
    //    swal.close();
    //}
}