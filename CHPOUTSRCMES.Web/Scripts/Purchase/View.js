$(document).ready(function () {
    click();

    addNewContent();
});

var imgSrc = []; //圖片路徑

function click() {
    $("#BtnReturn").click(function () {
        window.history.go(-1);
    });
}



//點擊預覽&&圖片放大
function addNewContent() {
    $(imgBox).html("");
    for (var a = 0; a < imgSrc.length; a++) {
        var oldBox = $('#imgBox').html();
        $('#imgBox').html(oldBox + '<div class="imgContainer"><img title=' + imgName[a] + ' alt=' + imgName[a] + ' src=' + imgSrc[a] + ' onclick="imgDisplay(this)"></div>');
    };
}


//圖片放大
function imgDisplay(obj) {
    var src = $(obj).attr("src");
    var imgHtml = '<div style="width: 100%;height: 100vh;overflow: auto;background: rgba(0,0,0,0.5);text-align: center;position: fixed;top: 0;left: 0;z-index: 1000;"><img src=' + src + ' style="margin-top: 100px;width: 70%;margin-bottom: 100px;"/><p style="font-size: 50px;position: fixed;top: 30px;right: 30px;color: white;cursor: pointer;" onclick="closePicture(this)">×</p></div>'
    $('body').append(imgHtml);
}

//關閉圖片放大
function closePicture(obj) {
    $(obj).parent("div").remove();
}