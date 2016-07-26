<?php

include_once 'config.php';

session_start();

$role = $_SESSION['myrole'];

if($role != "Bruger" && $role != "Admin")
{
    header("location:" .Teccargo_url);
}
