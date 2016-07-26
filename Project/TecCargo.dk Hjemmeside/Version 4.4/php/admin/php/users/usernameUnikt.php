<?php

include '../../../config.php';


$brugernavn = $_POST['bruger'];
if($brugernavn != "")
{
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "SELECT * FROM user WHERE Username='" .$brugernavn ."'";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    $count = mysqli_num_rows($result);
    //echo $count;
    //echo $sqlcmd;
}

mysqli_close($sqlConnect);


if($count != 0)
{
    echo "true";
}
else
{
    echo "false";
}