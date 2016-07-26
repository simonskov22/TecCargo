<?php
include_once '../php/config.php';

?>
<!DOCTYPE html>
<html>
    <head>
        <title>TECcargo.dk</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="<?php echo Teccargo_url;?>style/style.css" rel="stylesheet">
        <link href="<?php echo Teccargo_url;?>style/newadminStyle.css" rel="stylesheet">
        
        <link rel="shortcut icon" href="<?php echo Teccargo_url;?>images/icon.ico">
        
        <!-- Load jQuery from Google's CDN -->
        <script src="//ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>
        <!-- Source our javascript file with the jQUERY code -->
        <script type="text/javascript" src="<?php echo Teccargo_url;?>script/newadminScript.js"></script>
        <script type="text/javascript" src="<?php echo Teccargo_url;?>script/script1.js"></script>
        <!--<script type="text/javascript" src="<?php echo Teccargo_url;?>script/popupScript.js"></script>-->
        
        <!-- menu script -->

        <link href="<?php echo Teccargo_url;?>/style/css/dcmegamenu.css" rel="stylesheet" type="text/css" />
        
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>
        <script type='text/javascript' src='<?php echo Teccargo_url;?>script/js/jquery.hoverIntent.minified.js'></script>
        <script type='text/javascript' src='<?php echo Teccargo_url;?>script/js/jquery.dcmegamenu.1.3.3.js'></script>
        <script type='text/javascript' src='<?php echo Teccargo_url;?>script/js/jquery.session.js'></script>
        <script type='text/javascript' src='<?php echo Teccargo_url;?>script/js/jquery.sticky.js'></script>
        <script type="text/javascript">
            $(document).ready(function($){
                $('#mega-menu-9').dcMegaMenu({
                    rowItems: '5',
                    speed: 'fast',
                    effect: 'fade'
                });
                $("#followNav").sticky({topSpacing:0});
            });
        </script>
        <script type="text/javascript">
        
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
        </script>
        
        <link href="<?php echo Teccargo_url;?>style/css/skins/white.css" rel="stylesheet" type="text/css" />
        
        
        <style>
            .sitev2
            {
                padding: 20px;
            }
            .kurerButton
            {
                background: #254078;
                border: 2px outset #4268B8;
            }
            .kurerButton:hover
            {
                background: #31456F;
                border: 2px outset #4C5A79;
            }
            .kurerTableView
            {
                margin-left: auto;
                margin-right: auto;
                border-collapse: collapse;
            }
            .kurerTableView td
            {
                border: 2px solid black;
            }
            .kurerTableView input
            {
                width: 200px;
            }
            .tableFooter
            {
                margin-top: 10px;
                margin-left: auto;
                margin-right: auto;
                width: 1200px;
            }
            .footer1
            {
                float: left;
            }
            .footer2
            {
                text-align: center;
            }
            .footer3
            {
                float: right;
            }
        </style>
       
    </head>
    <body>
        <div class="headv2">
            <div class="head-cont">
                <img class="logo" src="<?php echo Teccargo_url;?>images/logo2.png">
                <img class='logo3' src='<?php echo Teccargo_url;?>images/logo3.png'>
                
            </div>
        </div>
        
        <!-- Menu -->
        <div id="followNav" class="navv2"><?php echo $NavBar;?></div>
        
        <!-- Sidens indhold-->
        <div class="sitev2"><?php echo $Index; ?></div>
            
        <div>
            <table class="tableFooter">
                <tr>
                    <td class="footer1">Version: 4.5</td>
                    <td class="footer2">Copyright &COPY; 2014</td>
                    <td class="footer3">Kodet af: Simon Skov</td>
                </tr>
            </table>
        </div> 
    </body>
</html>
