<?php
global $_URL;

function GetCurrentUserId() {
    global $database;

    if(!isset($_COOKIE['AUID']) && empty($_COOKIE['AUID']) &&!isset($_COOKIE['AUPA']) && empty($_COOKIE['AUPA'])){
        return 0;
    }
    
    $login = $database->SQLReadyString($_COOKIE['AUID']);
    $pass = $database->SQLReadyString($_COOKIE['AUPA']);
    
    $query = "SELECT COUNT(*), `userId` FROM `{$database->prefix}Users` WHERE `username`= '$login' AND `password` = '$pass' ;";
    $result = $database->GetRow($query, array_I);
    
    if($result[0] == 1){
        return $result[1];
    }
    else{
        return 0;
    }
}

function IsLogin() {
    
    if(GetCurrentUserId() != 0){
        
        return true;
    }
    else{
        return false;
    }
}

function IsAdmin() {
    global $database;
    
    $userId = GetCurrentUserId();
    
    if($userId == 0){
        return FALSE;
    }
    
    $query = "SELECT `admin` FROM `{$database->prefix}Users` WHERE `userId`= $userId;";
    $result = $database->GetRow($query, array_I);
 
    if($result[0] == 1){
        return true;
    }
    else{
        return false;
    }
}

function EncryptPass($pass, $salt){
    
    //encrypt kodeord
    $hash = hash('sha256', $pass);
    $decryptPassword = hash('sha256', $salt . $hash);
    
    return $decryptPassword;
}

function RandomString($lent = 128) {
    $letters = "1234567890qazwsxedcrfvtgbyhnujmikolp";

    $returnValue = "";
    for($i = 0; $i < $lent; $i++){
        $randomStart = rand(0, strlen($letters)-1);

        $oneLetter = substr($letters, $randomStart, 1);

        if(rand(0, 1) == 0){
            $oneLetter = strtoupper($oneLetter);
        }

        $returnValue .= $oneLetter;
    }

    return $returnValue;
}

function createSalt(){
    $saltString = md5(uniqid(rand(),TRUE));
    return substr($saltString, 2, 5);
}

