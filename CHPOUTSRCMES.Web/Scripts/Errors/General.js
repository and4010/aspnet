$(document).ready(function () {
    if (window.history.length <= 1) {
        $("#btnPrevious").hide();
    }


    $(".btn-group").on('click', '#btnPrevious', function () {
        window.history.go(-1);  
    });
    
});