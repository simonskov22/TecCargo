$(document).ready(function () {
    //var startUrl = "www.teccargo.dk/testside/";
    var startUrl = "www.teccargo.dk";
    
    
    $("button.testbutton, button.pricesPopupButton").click( function () {
        enableCenterWindow();
    });

    $(".testbutton2").click(function () {

        var buttonName = $(this).attr("id");
        var pricesLink = $("#" + buttonName + " span.prisLink").html();
        //alert(pricesLink);

        $.post(pricesLink, {type:buttonName}, function (data) {
            $("#pricesPopupI").html(data);
            //alert(data);
        });

        enableCenterWindow();
    });
    $(".kurerBil").click(function () {
        alert('test');
        
//var mode = $(this).attr('id').sub(15);
        //alert(mode);
        //var url = startUrl + "php/sider/prisPopup/kurertransportBiler.php";

        //$.post();
    });

    function enableCenterWindow () {
        $("#pricesPopup").toggle();
    };
});
