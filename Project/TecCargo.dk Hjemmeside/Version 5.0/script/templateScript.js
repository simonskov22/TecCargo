$(document).ready(function () {
    //var startUrl = "http://teccargo.dk/testside/";
    var startUrl = "http://teccargo.dk/";


    $("button.pricesButton, button#pricesPopupButton").click( function () {

        if($(this).hasClass("backWindow")){

            var backUrl = $.session.get("backUrl");
            var type = $.session.get("biltype");

            $.post(backUrl,{type:type.substr(2)},function (data) {
                $("#pricesPopupI").html(data);
                $("#pricesPopupButton").removeClass("backWindow").addClass("lukWindow").html("Luk");
            });
        }
        else{

            enableCenterWindow();
        }
    });

    $(".pricesButton").click(function () {
        //hvis session med bil ikke er sat

        if(!$.session.get("biltype")){

            var buttonName = $(this).attr("id");
            $.session.set("biltype", buttonName);
        }
        else{
            var buttonName = $.session.get("biltype");
        }

        //
        var pricesLink = $("#" + buttonName + " span.prisLink").html();
        //alert(pricesLink + " " + buttonName);
        $.session.set("backUrl", pricesLink);

        $.post(pricesLink, {type:buttonName.substr(2)}, function (data) {
            $("#pricesPopupI").html(data);

            $("#pricesPopupButton").removeClass("backWindow").addClass("lukWindow").html("Luk");
        });

        //enableCenterWindow();
    });
    $(document).on("click",".kurerBil",function () {

        var mode = $(this).attr('id');
        var url = startUrl + "php/sider/prisPopup/kurertransport/kurerTransportRush-Flex.php";
        //alert(mode.substring(15) + " " + url);

        $.post(url,{mode:mode.substring(15)},function (data){
            $("#pricesPopupI").html(data);
            $("#pricesPopupButton").removeClass("lukWindow").addClass("backWindow").html("Tilbage");
        });
    });

    function enableCenterWindow () {
        $("#pricesPopup").toggle();
        $.session.remove("backUrl");
        $.session.remove("biltype");
    };
});