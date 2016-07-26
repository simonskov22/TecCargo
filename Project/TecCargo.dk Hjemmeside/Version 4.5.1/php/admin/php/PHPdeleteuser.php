<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //og om de nødvendige bruger settings er sat
    if(!isset($_SESSION['myusername']) || !isset($_POST['id'])){
        
        echo "Error: 1";
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        header("location:" .Teccargo_url);
        exit();
    }
    $logCreatorUser = $_SESSION['myusername'];
    $id = json_decode($_POST['id']);
    
    
    $countId = count($id);
    $sqlDeleteIds = "";
    
    do {
        $sqlDeleteIds .= "id=".$id[$countId -1] ." OR ";
        $countId--;
    } while ($countId != 0);
    
    $sqlDeletelent = strlen($sqlDeleteIds) -4;
    
    //tjekker om der er nogle admins der bliver sletet
    //og hvis der er må han ikke være den sidste
    $result = databaseQuery("SELECT id FROM members WHERE rank='Admin' AND (" .substr($sqlDeleteIds, 0, $sqlDeletelent) .");");
    $resultCountDeleteAdmins = mysqli_num_rows($result);
    
    //hvormange admins er der ialt
    $result = databaseQuery("SELECT id FROM members WHERE rank='Admin'");
    $resultCountAllAdmins = mysqli_num_rows($result);
    
    //echo "Admin: $resultCountDeleteAdmins All admin: $resultCountAllAdmins SQL: WHERE rank='Admin' AND (" .substr($sqlDeleteIds, 0, $sqlDeletelent) .")";
    //WHERE id=17 OR id=6 AND rank='Admin'
    if($resultCountDeleteAdmins == $resultCountAllAdmins){
        
        echo "Error: 2";
        exit();
    }
    
    //$sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "DELETE FROM members WHERE " .  substr($sqlDeleteIds, 0, $sqlDeletelent);
    $result = databaseQuery($sqlcmd);
    
    //om det er gået igennem
     if($result){
        //skiver til log
        writeToLog("Brugerne er nu slettet." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL ."SQL: $sqlcmd");
    }
    else{
        echo "Error: 3";
        
        //skriver til log
        writeToLog("Kunne ikke slette brugerne." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL ."SQL: $sqlcmd");
    }