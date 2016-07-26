<?php

$backLink = "location:../../sider/delete-user.php";

include '../../../config.php';

session_start();
$users = $_POST['DeleteUser'];

$count = count($users);

$usersId = array();

//fjerner userid_
for($i = 0;$i < $count;$i++)
{
    $usersId[] = substr($users[$i],7);
}

//mysql
$sqlConnect = mysqli_connect(DB_host,DB_username,DB_password,DB_database)or die('cannot connect');

if($count == 1)
{
    $sqlcmd = "DELETE FROM user WHERE Username='" .$usersId[0]."'";
}
else 
{
    $deleteCmd = "";
        
    for($i = 0;$i < $count;$i++)
    {
        $deleteCmd .= "Username = '$usersId[$i]' OR ";

    }
    $deleteCmdDone = substr($deleteCmd, 0, strlen($deleteCmd) -3);
    $sqlcmd = "DELETE FROM user WHERE $deleteCmdDone";
}

$result = mysqli_query($sqlConnect, $sqlcmd);

if($result)
{

    $_SESSION['Delete'] = "<b>Godkendt:</b><br><br>De valgte brugere er blivet slettet.";
    //echo $sqlcmdDelete;
}
else
{
    $_SESSION['Delete'] = "<b>Fejl:</b><br><br>De valgte brugere blev ikke slettet.".$sqlcmd;
    //echo $sqlcmdDelete;
}
//echo $_SESSION['Delete'];

header($backLink);