<?php

include '../../config.php';
include '../../is-admin.php';

session_start();

$status = "";
$statusDisplay = "style='display: none;'";
if(isset($_SESSION['Pakke']))
{
    $status = $_SESSION['Pakke'];
    $statusDisplay = "style='display: block;'";
}

$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);

$sqlcmdPlus = "SELECT * FROM pakkkePrisList WHERE type = 'Plus'";
$sqlcmdGreen = "SELECT * FROM pakkkePrisList WHERE type = 'Green'";

$plusResult = mysqli_query($sqlConnect, $sqlcmdPlus);
$greenResult = mysqli_query($sqlConnect, $sqlcmdGreen);

$goPlus = mysqli_fetch_array($plusResult);
$goGreen = mysqli_fetch_array($greenResult);

mysqli_close($sqlConnect);


$pakkeInfo = array("XS","S","M","L","XL","2XL","3XL","Gebyr");


$goPlusTable = "<table style='float: right;'><tr><th colspan = '2'>Plus</th></tr>";

for($i = 2;$i < 10;$i++)
{
    $goPlusTable .= "<tr>"
            . "<td style='width: 50px; text-indent: 5px;'>" .$pakkeInfo[($i - 2)] ."</td>"
            . "<td><input type='text' name='goplus_" .$pakkeInfo[($i - 2)] ."' value='" .$goPlus[$i] ."'></td>"
            . "</tr>";
}
$goPlusTable .= "</table>";

$goGreenTable = "<table><tr><th colspan = '2'>Green</th></tr>";

for($i = 2;$i < 10;$i++)
{
    $goGreenTable .= "<tr>"
            . "<td style='width: 50px; text-indent: 5px;'>" .$pakkeInfo[($i - 2)] ."</td>"
            . "<td><input type='text' name='gogreen_" .$pakkeInfo[($i - 2)] ."' value='" .$goGreen[$i] ."'></td>"
            . "</tr>";
}
$goGreenTable .= "</table>";

//templates

$mainIndexLink = "../../../themes/index.html";
$mainNavbarLink = "../../../themes/navbarAdmin.html";
$thisThemeLink = "../themes/green-plus.html";

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);
$thisTheme = file_get_contents($thisThemeLink);

$thisTheme = str_replace("%STATUSDISPLAY%", $statusDisplay, $thisTheme);
$thisTheme = str_replace("%STATUS%", $status, $thisTheme);
$thisTheme = str_replace("%INPUT%", $goPlusTable .$goGreenTable, $thisTheme);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

echo $mainIndex;

unset($_SESSION['Pakke']);