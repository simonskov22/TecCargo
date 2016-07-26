<?php
include '../../../config.php';
$backLink = "location:../../sider/billeder-ny.php";

$billeder = array($_POST['menu1'],$_POST['menu2'],$_POST['menu3'],$_POST['menu4'],$_POST['menu5'],
    $_POST['pakke1'],$_POST['pakke2'],$_POST['pakke3'],$_POST['pakke4'],$_POST['pakke5'],$_POST['pakke6'],$_POST['pakke7'],$_POST['pakke8'],);

$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
session_start();
$_SESSION['Upload'] = "";
for($i = 0;$i < 13;$i++)
{
    $sqlcmd = "UPDATE billeder SET billede='$billeder[$i]' WHERE id=" .($i + 1) .";";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    if(!$result)
    {
        $_SESSION['Upload'] .= "<b>Fejl</b> Kunne ikke gemme billedet: $billeder[$i]<br>";
    }
}
header($backLink);