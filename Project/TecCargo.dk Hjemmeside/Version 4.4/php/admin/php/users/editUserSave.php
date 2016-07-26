<?php

$backLink = "location:../../sider/useredit.php";

$save_name = $_POST['edit-user-name'];
$save_lastName = $_POST['edit-user-lastname'];
$save_username = $_POST['edit-user-username'];
$save_password = $_POST['edit-user-password'];
$save_type = $_POST['edit-user-type'];

session_start();

//tjekker om man har udfyldt felterne
if($save_name == "" || $save_lastName == "" || $save_username == "")
{
    $error = "<b>Fejl:</b><br>";
    if($save_name == "")
    {
        $name = "<br>Udfyld navn.";
    }
    if($save_lastName == "")
    {
        $lastname = "<br>Udfyld efternavn.";
    }
    if($save_username == "")
    {
        $user = "<br>Udfyld brugernavn.";
    }
    
    $_SESSION['user'] = $error .$name .$lastname .$user;
}
else
{
    include '../../../config.php';
    
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    
    //om man skal gemme password eller ej
    if($save_password == "")
    {
        $sqlcmd = "UPDATE user SET `Username`='" .$save_username ."',`name`='" .$save_name ."',`last name`='" .$save_lastName ."',`role`='".$save_type ."' WHERE Username='" .$save_username ."';";
        
    }
    else
    {
        $sqlcmd = "UPDATE user SET `Username`='" .$save_username ."',`Password`='" .$save_password ."',`name`='" .$save_name ."',`last name`='" .$save_lastName ."',`role`='".$save_type ."' WHERE Username='" .$save_username ."';";
    }
    
    if(mysqli_query($sqlConnect, $sqlcmd))
    {
        $_SESSION['user'] = "<b>Godkendt:</b><br><br>Brugen er nu gemt.";
    }
    else
    {
        $_SESSION['user'] = "<b>Fejl:</b><br><br>Brugen kunne ikke gemmes.";
        //echo $sqlcmd;
    }
}

//echo $_SESSION['User'];
header($backLink);
