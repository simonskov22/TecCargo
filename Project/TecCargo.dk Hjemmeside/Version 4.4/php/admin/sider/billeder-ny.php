<?php

session_start();

include '../../config.php';
include '../../is-admin.php';
$linkMainIndex = "../../../themes/index.html";
$linkAdminBar = "../../../themes/navbarAdmin.html";
$linkBillede = "../themes/ny-billeder.html";


$status = '';
$statusDisplay = "style='display: none;'";

if(isset($_SESSION['Upload']))
{
    $status = $_SESSION['Upload'];
    $statusDisplay = "style='display: block;'";
}

$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
$sqlcmd = "SELECT * FROM billeder";
$result = mysqli_query($sqlConnect, $sqlcmd);

$bilder = array();
while ($row = mysqli_fetch_array($result))
{
    $bilder[] = $row[1];
}

$mainIndex = file_get_contents($linkMainIndex);
$adminBarthemes = file_get_contents($linkAdminBar);
$billedthemes = file_get_contents($linkBillede);

$billedthemes = str_replace("%TABLEP%", $picTab, $billedthemes);
$billedthemes = str_replace("%SIDE%", $side, $billedthemes);
$billedthemes = str_replace("%SIDER%", $sider, $billedthemes);

$billedthemes = str_replace("%STATUSDISPLAY%", $statusDisplay, $billedthemes);
$billedthemes = str_replace("%STATUS%", $status, $billedthemes);

$mainIndex = str_replace("%NavBar%", $adminBarthemes, $mainIndex);
$mainIndex = str_replace("%Index%", $billedthemes, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

$mainIndex = str_replace("%MENU1%", $bilder[0], $mainIndex);
$mainIndex = str_replace("%MENU2%", $bilder[1], $mainIndex);
$mainIndex = str_replace("%MENU3%", $bilder[2], $mainIndex);
$mainIndex = str_replace("%MENU4%", $bilder[3], $mainIndex);
$mainIndex = str_replace("%MENU5%", $bilder[4], $mainIndex);

$mainIndex = str_replace("%PAKKE1%", $bilder[5], $mainIndex);
$mainIndex = str_replace("%PAKKE2%", $bilder[6], $mainIndex);
$mainIndex = str_replace("%PAKKE3%", $bilder[7], $mainIndex);
$mainIndex = str_replace("%PAKKE4%", $bilder[8], $mainIndex);
$mainIndex = str_replace("%PAKKE5%", $bilder[9], $mainIndex);
$mainIndex = str_replace("%PAKKE6%", $bilder[10], $mainIndex);
$mainIndex = str_replace("%PAKKE7%", $bilder[11], $mainIndex);
$mainIndex = str_replace("%PAKKE8%", $bilder[12], $mainIndex);


echo $mainIndex;

unset($_SESSION['Error']);
unset($_SESSION['Upload']);
