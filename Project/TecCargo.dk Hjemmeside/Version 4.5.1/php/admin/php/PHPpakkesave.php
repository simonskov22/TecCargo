<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    if(!isset($_SESSION['myusername']) || !isset($_POST['values'])){
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        header("location:" .Teccargo_url);
        exit();
    }
    
    $values = json_decode($_POST['values']);
    
    
    //databse
    $result = databaseSelect("column_name", "information_schema.columns", "WHERE table_name = 'pakkkePrisList'");
    $columnName = array();
    
    while ($row = mysqli_fetch_array($result)) {
        $columnName[] = $row[0];
    }
    
    
    $countvalues = count($values);
    $sqlExtra = "";
    
    for ($i = 0; $i < $countvalues -1; $i++){
        $sqlExtra .= "`" .$columnName[$i +2] ."` = '" .$values[$i] ."',";
    }
    
    $sqlExtraLengt = strlen($sqlExtra);
    
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "UPDATE pakkkePrisList SET " .substr($sqlExtra, 0,$sqlExtraLengt-1) ." WHERE type = '".$values[$countvalues-1] ."'";
    
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    if($result){
        
        //skiver til log
        writeToLog("Priserne for " .$values[$countvalues-2] ." er nu gemt.");
    }
    else {
        echo 'Error: 1';
        //skiver til log
        writeToLog("Priserne for " .$values[$countvalues-2] ." kunne ikke gemmes.".PHP_EOL."SQL: $sqlcmd");
    }
    