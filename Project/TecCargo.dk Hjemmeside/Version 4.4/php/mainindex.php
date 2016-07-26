<?php
  // Check if session is not registered, redirect back to main page. 
  // Put this code in first line of web page. 
  // echo "login_success";
include 'config.php';
include 'is-user.php';


$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
$sqlcmd = "SELECT * FROM billeder";
$result = mysqli_query($sqlConnect, $sqlcmd);
$billeder = array();
while ($row = mysqli_fetch_array($result))
{
    $billeder[] = $row[1];
}
ob_start();
?>
<!-- ## Forside ## -->
<div class="side-link page1 showli" style="padding-top: 20px;">
    <div class="logoLink">
        <a href="#" id="page2"><p id="logoLink-1" style="float: left; margin-left: 215px;position: absolute;margin-top: 6px;">Express<span class="redCargo">GoRush</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page2"><p id="logoLink-2" style="float: left; margin-left: 125px;position: absolute;margin-top: 158px;">Express<span class="redCargo">GoFlex</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page2"><p id="logoLink-3" style="float: left; margin-left: 124px;position: absolute;margin-top: 280px;">Express<span class="redCargo">GoVIP</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page3"><p id="logoLink-4" style="float: left; margin-left: 206px;position: absolute;margin-top: 397px;">Priority<span class="redCargo">GoPlus</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page3"><p id="logoLink-5" style="float: left; margin-left: 376px;position: absolute;margin-top: 428px;">Economy<span class="greenCargo">GoGreen</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page4"><p id="logoLink-6" style="float: left; margin-left: 567px;position: absolute;margin-top: 397px;">Cargo<span class="redCargo">GoFull</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page4"><p id="logoLink-7" style="float: left; margin-left: 601px;position: absolute;margin-top: 280px;">Cargo<span class="redCargo">GoPart</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page5"><p id="logoLink-8" style="float: left; margin-left: 601px;position: absolute;margin-top: 158px;">WHS<span class="redCargo">Logistics</span><span class="blackTm">&#8482;</span></p></a>
        <a href="#" id="page6"><p id="logoLink-9" style="float: left; margin-left: 514px;position: absolute;margin-top: 6px;">Tech<span class="redCargo">Montage</span><span class="blackTm">&#8482;</span></p></a>

        <img src="../images/logo.png" style="margin-left: 346px;margin-top: 88px;position: absolute;top: 296px;">
        
        <svg>
            <line class="logoLink-1" x1="310" y1="38" x2="373" y2="100"></line>
            <line class="logoLink-2" x1="220" y1="190" x2="373" y2="215"></line>
            <line class="logoLink-3" x1="220" y1="281" x2="373" y2="250"></line>
            <line class="logoLink-4" x1="295" y1="398" x2="357" y2="336"></line>
            <line class="logoLink-5" x1="480" y1="430" x2="460" y2="339" style="stroke: #5AC440;"></line>
            <line class="logoLink-6" x1="643" y1="398" x2="560" y2="339"></line>
            <line class="logoLink-7" x1="680" y1="281" x2="526" y2="249"></line>
            <line class="logoLink-8" x1="670" y1="190" x2="526" y2="210"></line>
            <line class="logoLink-9" x1="580" y1="38" x2="526" y2="100"></line>
        </svg>
    </div>
</div>

<!-- ## kurertransport ## -->
<div class="side-link page2 hiddeli">
    <?php 
        include 'sider/kurertransport/kurertransport.php';
        include 'sider/kurertransport/gorush.php';
        include 'sider/kurertransport/goflex.php';
        include 'sider/kurertransport/govip.php';
    ?>

    <div class="centerWindow showrushprices">
        <div>
        <div id="showrushprices_head"</div>
            <?php include 'sider/pris/kurertransport/kurertransportBiler.php'; ?>
        </div>
        </div>
        <div id="showrushprices_view" class="hiddeli"></div>
        <center>
            <button class="backWindow hiddeli">Tilbage</button>
            <button class="lukWindow">Luk</button>
        </center>
    </div>

    <div class="centerWindow showflexprices">
        <div>
        <div id="showflexprices_head"</div>
            <?php include 'sider/pris/kurertransport/kurertransportBiler.php'; ?>
        </div>
        </div>
        <div id="showflexprices_view" class="hiddeli"></div>
        <center>
            <button class="backWindow hiddeli">Tilbage</button>
            <button class="lukWindow">Luk</button>
        </center>
    </div>
</div>


<!-- ## pakketransport ## -->
<div class="side-link page3 hiddeli">
    <?php 
        include 'sider/pakketransport/pakketransport.php';
        include 'sider/pakketransport/goplus.php';
        include 'sider/pakketransport/gogreen.php';
    ?>

    <div class="centerWindow showpakkegreen">
        <center>
            <?php
            //$type om det er plus eller green **husk ''**
            $type = "'green'";
            include 'sider/pris/pakketransport/pakketransportList.php';
            ?>
            <button class="lukWindow">Luk</button>
        </center>
    </div>

    <div class="centerWindow showpakkeplus">
        <center>
            <?php
            //$type om det er plus eller green **husk ''**
            $type = "'plus'";
            include 'sider/pris/pakketransport/pakketransportList.php';
            ?>
            <button class="lukWindow">Luk</button>
        </center>
    </div>
</div>

<!-- ## godstransport ## -->
<div class="side-link page4 hiddeli">
    <?php 
        include 'sider/godstransport/godstransport.php';
        include 'sider/godstransport/gofull.php';
        include 'sider/godstransport/gopart.php';
    ?>

    <div class="centerWindow showgodsprices">
        <center>
            <?php include 'sider/pris/godstransport/prisliste.php'; ?>
            <button class="lukWindow">Luk</button>
        </center>
    </div>

    <div class="centerWindow showgodszoner">
        <center>
            <?php include 'sider/pris/godstransport/overtak.php'; ?>
            <button class="lukWindow">Luk</button>
        </center>
    </div>

    <div class="centerWindow showgodsladmeter">
        <div style="
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 20px;
            margin-top: 20px;
            width: 1000px;
            padding-left: 100px;
            padding-bottom: 20px;
            background: white;
        ">
            <?php include 'sider/pris/godstransport/ladmeter.php'; ?>
        </div>
        <center><button class="lukWindow" align="center">Luk</button></center>
    </div>
</div>



<!-- ## lagerhotel ## -->
<div class="side-link page5 hiddeli">
<?php 
        include 'sider/andet/lagerhotel.php';
?>

    <div class="centerWindow showhotelprices">
        <center>
            <div style="background: white; text-align: left; width: 1000px; padding: 10px; margin-top: 20px; margin-bottom: 20px;">
                <?php include 'sider/pris/andet/lagerhotel.php'; ?>
            </div>
            <button class="lukWindow">Luk</button>
        </center>
    </div>
</div>

<!-- ## montage ## -->
<div class="side-link page6 hiddeli">
<?php 
        include 'sider/andet/montage.php';
?>
</div>
<div class="side-link page7 hiddeli"> 
<?php 
    //$mode til hvad der skal vise
    //$type om det er flex eller rush **husk ''**    
    $mode = "full";
    $type = "'rush'";

    include "sider/pris/kurertransport/kurerTransportRush-Flex.php";
?>
</div>

<div class="side-link page8 hiddeli"> 
<?php 
        //$mode til hvad der skal vise
        //$type om det er flex eller rush **husk ''**
        $mode = "full";
        $type = "'flex'";

        include "sider/pris/kurertransport/kurerTransportRush-Flex.php";
?>
</div>

<!-- ## Priser Plus ##  -->
<div class="side-link page10 hiddeli"> 
<?php 
        //$type om det er plus eller green **husk ''**
        $type = "'plus'";
        include 'sider/pris/pakketransport/pakketransportList.php';
?>
</div>

<!-- ## Priser green ##  -->
<div class="side-link page11 hiddeli"> 
<?php 
        //$type om det er plus eller green **husk ''**
        $type = "'green'";
        include 'sider/pris/pakketransport/pakketransportList.php';
?>
</div>

<!-- ## gopart prisliste ## -->

<div class="side-link page13 hiddeli">

    <div class="pricesDivGods">
        <div class="gopart">
            <button id="gopart1"class="gopartButton">Takst Zoner</button>
            <button id="gopart2" class="gopartButton gopartButtonSelect">Prisliste</button>
            <button id="gopart3"class="gopartButton">Beregning af ladmeter</button>
        </div>
        <img id="pricesImgGods" class="pricesImgGods2" src='../images/logo.png'>
        <p id="pricesInfoGods"  class="pricesInfoGods2">Takstzoner prisliste<br>Danmark excl moms</p>

    </div>

    <div class="gopart-link gopart1 hiddeli">
        <?php include 'sider/pris/godstransport/overtak.php'; ?>
    </div>
    <div class="gopart-link gopart2">


        <?php include 'sider/pris/godstransport/prisliste.php'; ?>
    </div>
    <div class="gopart-link gopart3 hiddeli">
        <?php include 'sider/pris/godstransport/ladmeter.php'; ?>
    </div>

</div>

<!-- ## Tracking ## -->
<div class="side-link page15 hiddeli">
    <h2>Tracking</h2>
</div>

<!-- ## Kvalitetspolitik ## -->
<div class="side-link page16 hiddeli">  
<?php 
        include 'sider/teccargo/kvalitetspolitik.php';
?>
</div>

<!-- ## Miljøpolitik ## -->
<div class="side-link page17 hiddeli">  
<?php 
        include 'sider/teccargo/mpolitik.php';
?>
</div>
<!-- ## fragt service ## -->
<div class="side-link page18 hiddeli">  
<?php 
        include 'sider/teccargo/fragtService.php';
?>
</div>
<!-- ## font ## -->
<div class="side-link page19 hiddeli">  
<?php 
        include 'sider/teccargo/font.php';
?>
</div>
<!-- ## farlig gods ## -->
<div class="side-link page21 hiddeli">  
<?php 
        include 'sider/teccargo/farlig-gods.php';
?>
</div>
<!-- ## indledning ## -->
<div class="side-link page22 hiddeli">  
<?php 
        include 'sider/teccargo/Indledning.php';
?>
</div>

<!-- ## Pris lagerhotel ## -->
<div class="side-link page20 hiddeli">
<?php include 'sider/pris/andet/lagerhotel.php'; ?>
</div>

<!-- ## PDF Styr på køre-og hviletiden ## -->
<div  class="side-link page23 hiddeli">
    <center>
        <object data="../pdf/ITD_KoereOGhviletids-folder_Marts2013.pdf" type="application/pdf" style="width: 1000px; height: 650px;">
        alt : <a href="../pdf/ITD_KoereOGhviletids-folder_Marts2013.pdf">ITD_KoereOGhviletids-folder_Marts2013.pdf</a>
        </object>
    </center>
</div>

<?php

    //templates

$mainIndexLink = "../themes/index.html";
$mainNavbarLink = "../themes/navbar.php";

$thisTheme = ob_get_contents();
ob_end_clean();

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

//billeder
$mainIndex = str_replace("%KURERB%", $billeder[0], $mainIndex);
$mainIndex = str_replace("%PAKKEB%", $billeder[1], $mainIndex);
$mainIndex = str_replace("%GODSB%", $billeder[2], $mainIndex);
$mainIndex = str_replace("%LOGB%", $billeder[3], $mainIndex);
$mainIndex = str_replace("%MONB%", $billeder[4], $mainIndex);

$mainIndex = str_replace("%PAKKEB1%", $billeder[5], $mainIndex);
$mainIndex = str_replace("%PAKKEB2%", $billeder[6], $mainIndex);
$mainIndex = str_replace("%PAKKEB3%", $billeder[7], $mainIndex);
$mainIndex = str_replace("%PAKKEB4%", $billeder[8], $mainIndex);
$mainIndex = str_replace("%PAKKEB5%", $billeder[9], $mainIndex);
$mainIndex = str_replace("%PAKKEB6%", $billeder[10], $mainIndex);
$mainIndex = str_replace("%PAKKEB7%", $billeder[11], $mainIndex);
$mainIndex = str_replace("%PAKKEB8%", $billeder[12], $mainIndex);

$adminButton = "hiddeli";
if($role == "Admin")
{
    $adminButton = "";
}

$mainIndex = str_replace("%ADMINVIEW%", $adminButton, $mainIndex);

echo $mainIndex;