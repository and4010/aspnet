//顯示請等待Dialog
function ShowWait(callBack) {
    if (swal.isVisible()) { //判斷swal是否已顯示
        if (swal.isLoading()) { //判斷swal是否顯示正在讀取
            //已有顯示正在讀取，只執行callBack
            callBack();
        } else {
            //顯示正在讀取並執行callBack
            swal.fire({ //設定swal
                title: "請等待...",
                allowOutsideClick: false,
                allowEscapeKey: false,
                onOpen: function () {
                    callBack();
                }
            })
            swal.showLoading(); //顯示正在讀取，隱藏按鈕
        }
    } else {
        //顯示正在讀取並執行callBack
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



//關閉請等待Dialog
function CloseWait() {
    if (swal.isVisible()) {
        swal.close();
    }
}