<?php
include '../../config.php';
include '../../is-admin.php';

session_start();

$status = "";
$statusDisplay = "style='display: none;'";
if(isset($_SESSION['Delete']))
{
   $status = $_SESSION['Delete'];
$statusDisplay = "style='display: block;'";
}

//henter brugere liste

//mysql
$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database)or die("cannot connect");
$sqlcmdEdit = "SELECT Username,name,`last name` FROM user";

$result = mysqli_query($sqlConnect, $sqlcmdEdit);


$brugerList = "";

//tilføjer brugerne til en <tr>
while ($row = mysqli_fetch_array($result))
{
    $brugerList .= "<tr id ='userid_" .$row[0] ."'>";
    $brugerList .= "<input type='hidden' value='userid_" .$row[0] ."' name='DeleteUser[]'>";
    $brugerList .= "<td style='width: 70px'><img src='" .Teccargo_url ."images/icons/user.png'></td>";
    $brugerList .= "<td style='width: 130px'>" .$row[1] ." " .$row[2] ."</td>";
    $brugerList .= "<td>" .$row[0] ."</td>";
    $brugerList .= "</tr>";
}
mysqli_close($sqlConnect);

//templates

$mainIndexLink = "../../../themes/index.html";
$mainNavbarLink = "../../../themes/navbarAdmin.html";
$thisThemeLink = "../themes/delete-user-theme.html";

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);
$thisTheme = file_get_contents($thisThemeLink);

$thisTheme = str_replace("%STATUSDISPLAY%", $statusDisplay, $thisTheme);
$thisTheme = str_replace("%STATUS%", $status, $thisTheme);
$thisTheme = str_replace("%BRUGERL%", $brugerList, $thisTheme);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

echo $mainIndex;

unset($_SESSION['Delete']);