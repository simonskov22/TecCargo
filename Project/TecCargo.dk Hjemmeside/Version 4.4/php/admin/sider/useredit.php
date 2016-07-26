<?php

session_start();
include '../../config.php';
include '../../is-admin.php';
include '../../getBrowser.php';

$browser = getBrowser();

if($browser['name'] == 'Internet Explorer' && $browser['version'] == '9.0')
{
   $editname = "Navn";
   $editlastname = "Efternavn";
   $editusername = "Brugernavn";
   $editpass = "Password";
}
else 
{    
   $editname = "";
   $editlastname = "";
   $editusername = "";
   $editpass = "";
}

$mainIndex = "../../../themes/index.html";
$mainNavbar = "../../../themes/navbarAdmin.html";
$thisTheme = "../themes/editUser.html";

$status = "";
$statusDisplay = "style='display: none;'";

if(isset($_SESSION['user']))
{
    $status = $_SESSION['user'];
    $statusDisplay = "style='display: block;'";
}
//mysql connect
$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);

//sql command`
$sqlcmdEdit = "SELECT Username,name,`last name` FROM user";


$resultEdit = mysqli_query($sqlConnect, $sqlcmdEdit)or die(mysqli_error($sqlConnect) .' :' .  mysqli_errno($sqlConnect));


$brugerList = "";
while ($row = mysqli_fetch_array($resultEdit))
{
    $brugerList .= "<tr id='userid_" .$row[0] ."'>";
    $brugerList .= "<td style='width: 70px'><img src='//teccargo.dk/testside/images/icons/user.png'></td>";
    $brugerList .= "<td style='width: 130px'>" .$row[1] ." " .$row[2] ."</td>";
    $brugerList .= "<td>" .$row[0] ."</td>";
    $brugerList .= "</tr>";
}
mysqli_close($sqlConnect);


//TEAMPLATE

$navbar = file_get_contents($mainNavbar);

$thisIndex = file_get_contents($thisTheme);

$theme = file_get_contents($mainIndex);
$theme = str_replace("%NavBar%", $navbar, $theme);
$theme = str_replace("%Index%", $thisIndex, $theme);

$theme = str_replace("%TableUserList%", $brugerList, $theme);
$theme = str_replace("%SIDE%", $side, $theme);
$theme = str_replace("%SIDER%", $sider, $theme);

$theme = str_replace("%STATUSDISPLAY%", $statusDisplay, $theme);
$theme = str_replace("%STATUS%", $status, $theme);
$theme = str_replace("%TECCARGO%", Teccargo_url, $theme);


$theme = str_replace("%EDITN%", $editname, $theme);
$theme = str_replace("%EDITE%", $editlastname, $theme);
$theme = str_replace("%EDITB%", $editusername, $theme);
$theme = str_replace("%EDITK%", $editpass, $theme);

echo $theme;

unset($_SESSION['user']);