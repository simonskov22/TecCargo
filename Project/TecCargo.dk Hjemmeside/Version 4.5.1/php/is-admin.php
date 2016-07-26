<?php

include_once 'config.php';

session_start();

if(isset($_SESSION['myrole'])) { 

    $role = $_SESSION['myrole'];
    
    if($role == "Bruger")
    {
        header("location:" .Teccargo_url ."php/mainindex.php");
    }
    else if($role == "")
    {
        header("location:" .Teccargo_url);
    }
}
else {
    header("location:" .Teccargo_url);
}