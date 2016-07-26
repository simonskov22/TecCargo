<?php

include 'config.php';

session_start();

$role = $_SESSION['myrole'];


if($role == "Bruger")
{
    header("location:" .Teccargo_url ."php/mainindex.php");
}
else if($role == "")
{
    header("location:" .Teccargo_url);
}