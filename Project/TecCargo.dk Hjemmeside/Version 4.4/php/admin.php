<?php
include 'config.php';
include 'is-admin.php';
include 'getBrowser.php';

$browser = getBrowser();

if($browser['name'] == 'Internet Explorer' && $browser['version'] == '9.0' )
{
   $createname = "Navn";
   $createlastname = "Efternavn";
   $createusername = "Brugernavn";
   $createpass = "Password";
}
else 
{    
   $createname = "";
   $createlastname = "";
   $createusername = "";
   $createpass = "";
}
session_start();

$status = "";
$statusDisplay = "style='display: none;'";

if(isset($_SESSION['Create']))
{
   $status = $_SESSION['Create'];
$statusDisplay = "style='display: block;'";
}
unset($_SESSION['Create']);
//templates

$mainIndexLink = "../themes/index.html";
$mainNavbarLink = "../themes/navbarAdmin.html";
$thisThemeLink = "admin/themes/create-user.html";

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);
$thisTheme = file_get_contents($thisThemeLink);

$thisTheme = str_replace("%STATUSDISPLAY%", $statusDisplay, $thisTheme);
$thisTheme = str_replace("%STATUS%", $status, $thisTheme);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

$mainIndex = str_replace("%CREATEN%", $createname, $mainIndex);
$mainIndex = str_replace("%CREATEE%", $createlastname, $mainIndex);
$mainIndex = str_replace("%CREATEB%", $createusername, $mainIndex);
$mainIndex = str_replace("%CREATEK%", $createpass, $mainIndex);

echo $mainIndex;

