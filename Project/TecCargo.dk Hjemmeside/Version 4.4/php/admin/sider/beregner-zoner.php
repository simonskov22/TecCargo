<?php
include '../../config.php';
include '../../is-admin.php';
include '../../getBrowser.php';

$browser = getBrowser();

if($browser['name'] == 'Internet Explorer' && $browser['version'] == '9.0' )
{
   $kilo = "Antal Kilo";
}
else 
{    
    $kilo = "";
}
    //templates

$mainIndexLink = "../../../themes/index.html";
$mainNavbarLink = "../../../themes/navbarAdmin.html";
$thisThemeLink = "../themes/beregner-zoner-theme.html";

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);
$thisTheme = file_get_contents($thisThemeLink);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

$mainIndex = str_replace("%KILO%", $kilo, $mainIndex);

echo $mainIndex;