<?php

define("DB_host", "");
define("DB_username", "");
define("DB_password", "");
define("DB_database", "");

//husk http://
//define("Teccargo_url", "http://teccargo.dk/testside/");
define("Teccargo_url", "http://teccargo.dk/");


function getUserRole ($sendBack, $userSendback) {
    if(session_id() == '')
    {
        session_start();
    }
    
    if(isset($_SESSION["myrole"])) {
        $defult_role = $_SESSION["myrole"];
        
        if($defult_role != "Admin" && $defult_role != "User") {
            $defult_role = "Guest";
        }
    }
    else {
        $defult_role = "Guest";
    }

    if($userSendback && $defult_role == "User") {
        header("location:" .Teccargo_url ."php/mainindex.php");
    }
    if($sendBack && $defult_role == "Guest") {
        header("location:" .Teccargo_url);
    }
    //echo $defult_role;
    
    return $defult_role;
}

function writeToLog($comment){
    //skiver til log
    $log  = "Dato: " .date("d/m/Y - H:i:s").PHP_EOL.
    "Kommentar: " .$comment.PHP_EOL.
    "IP: ".$_SERVER['REMOTE_ADDR'].PHP_EOL.
    "Fil: " .$_SERVER['REQUEST_URI'].PHP_EOL.
    "############################################################".PHP_EOL.
    "############################################################".PHP_EOL;

    $fp = fopen($_SERVER['DOCUMENT_ROOT'] ."/testside/logs/logfile.txt", "a");
    fwrite($fp, $log);
    fclose($fp);
}

function databaseSelect($select, $table, $ekstra){
    $databaseConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);

    $databaseCommand = "SELECT $select FROM $table $ekstra";
    $databaseResult = mysqli_query($databaseConnect, $databaseCommand);

    mysqli_close($databaseConnect);

    return $databaseResult;
}


function databaseQuery($sqlCMD, $error = 0){
    $databaseConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);

    $databaseResult = mysqli_query($databaseConnect, $sqlCMD);
    
    if ($error == 1) {
        echo "MySql Error: " .mysqli_error($databaseConnect) ."\nMysql kode: " .  mysqli_errno($databaseConnect);
    }
    
    mysqli_close($databaseConnect);

    return $databaseResult;
}