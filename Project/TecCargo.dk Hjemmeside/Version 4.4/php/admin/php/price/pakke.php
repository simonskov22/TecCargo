<?php

include '../../../config.php';

session_start();
$backLink = "location:../../sider/edit-green-plus.php";

$goGreen = array($_POST['gogreen_XS'],$_POST['gogreen_S'],$_POST['gogreen_M'],$_POST['gogreen_L'],$_POST['gogreen_XL'],$_POST['gogreen_2XL'],$_POST['gogreen_3XL'],$_POST['gogreen_Gebyr']);
$goPlus = array($_POST['goplus_XS'],$_POST['goplus_S'],$_POST['goplus_M'],$_POST['goplus_L'],$_POST['goplus_XL'],$_POST['goplus_2XL'],$_POST['goplus_3XL'],$_POST['goplus_Gebyr']);


$ok = true;
for($i = 0;$i < 8; $i++)
{
    if($goGreen[$i] == "" || $goPlus[$i] == "")
    {
        $ok = false;
        break;
    }
}

if($ok)
{
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);

    $sqlcmdplus = "UPDATE  pakkkePrisList SET xs='$goPlus[0]',s='$goPlus[1]',m='$goPlus[2]',"
            . "l='$goPlus[3]',xl='$goPlus[4]',2xl='$goPlus[5]',3xl='$goPlus[6]',gebyr='$goPlus[7]' WHERE type='Plus'";


    $sqlcmdgreen = "UPDATE  pakkkePrisList SET xs='$goGreen[0]',s='$goGreen[1]',m='$goGreen[2]',"
            . "l='$goGreen[3]',xl='$goGreen[4]',2xl='$goGreen[5]',3xl='$goGreen[6]',gebyr='$goGreen[7]' WHERE type='Green'";

    $resultPlus = mysqli_query($sqlConnect, $sqlcmdplus);
    $resultGreen = mysqli_query($sqlConnect, $sqlcmdgreen);

    if($resultPlus)
    {
        $message = "<b>Godkendt:</b><br><br>GoPlus prisen er nu gemt.<br>";
    }
    else
    {
        $message = "<b>Fejl:</b><br><br>GoPlus prisen kunne ikke gemmes.<br>";
    }

    if($resultGreen)
    {
        $message .= "<b>Godkendt:</b><br><br>GoGreen prisen er nu gemt.";
    }
    else
    {
        $message .= "<b>Fejl:</b><br><br>GoGreen prisen kunne ikke gemmes.";
    }
}
else
{
    $message = "<b>Fejl:</b><br><br>Kan ikke gemme prisen.";
}

$_SESSION['Pakke'] = $message;

header($backLink);