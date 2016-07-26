<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //og om de nødvendige bruger settings er sat og values
    if(!isset($_SESSION['myusername']) || !isset($_POST['takst2']) ||
            !isset($_POST['takst3']) || !isset($_POST['takst4']) ||
            !isset($_POST['takst5']) || !isset($_POST['takst6']) ||
            !isset($_POST['takst7']) || !isset($_POST['takst8']) ||
            !isset($_POST['takst9']) || !isset($_POST['takst10']) ||
            !isset($_POST['prices']))
    {
        echo "Error: 1";
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        
        header("location:" .Teccargo_url);
        exit();
    }
    
    //henter values
    $prisValue = json_decode($_POST['prices']);
    $takst2 = json_decode($_POST['takst2']);
    $takst3 = json_decode($_POST['takst3']);
    $takst4 = json_decode($_POST['takst4']);
    $takst5 = json_decode($_POST['takst5']);
    $takst6 = json_decode($_POST['takst6']);
    $takst7 = json_decode($_POST['takst7']);
    $takst8 = json_decode($_POST['takst8']);
    $takst9 = json_decode($_POST['takst9']);
    $takst10 = json_decode($_POST['takst10']);
    
    $sqlcmdT = array();
    $sqlcmdP = array();
    $errorBool = false;
    
    //bliver brugt til at se alle sqlcommandoerne der blev brugt
    $logSql = "";
    
    //til logs
    $logCreatorUser = $_SESSION['myusername'];
    
    //laver sql command tekst
    for ($i = 0; $i < 8; $i++){
        
        $id = $i + 1;
        
        //byt om på kommaerne
        $engTakst2 = numberToEnglish($takst2[$i]);
        $engTakst3 = numberToEnglish($takst3[$i]);
        $engTakst4 = numberToEnglish($takst4[$i]);
        $engTakst5 = numberToEnglish($takst5[$i]);
        $engTakst6 = numberToEnglish($takst6[$i]);
        $engTakst7 = numberToEnglish($takst7[$i]);
        $engTakst8 = numberToEnglish($takst8[$i]);
        $engTakst9 = numberToEnglish($takst9[$i]);
        $engTakst10 = numberToEnglish($takst10[$i]);
        
        $sqlcmdT[] = "UPDATE prisliste_takst "
                . "SET Takst2 = $engTakst2, Takst3 = $engTakst3, Takst4 = $engTakst4, "
                . "Takst5 = $engTakst5, Takst6 = $engTakst6, Takst7 = $engTakst7, "
                . "Takst8 = $engTakst8, Takst9 = $engTakst9, Takst10 = $engTakst10 "
                . "WHERE id = $id; ";   
        
        //til log
        $logSql .= $sqlcmdT[$i].PHP_EOL;
        
        //gemmer de nye procenter til databasen
        $result = databaseQuery($sqlcmdT[$i]);
        
        //hvis der ikke går igennem skrive til log hvad sql command der blev brugt
        if (!$result) {
            $errorBool = true;
            writeToLog("GoPart kunne ikke gemme." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL ."SQL: $sqlcmdT[$i]");
        }
    }
    
    //tilføjer start prisen
    for ($i = 0; $i < 40; $i++){
        $id = $i + 1;
        //byt om på kommaerne
        $engPrisValue = numberToEnglish($prisValue[$i]);
        
        $sqlcmdP[] = "UPDATE godstransportPris "
                . "SET startPris = $engPrisValue "
                . "WHERE id = $id; ";
        
        //til log
        $logSql .= $sqlcmdP[$i].PHP_EOL;
        
        //gemmer de nye procenter til databasen
        $result = databaseQuery($sqlcmdP[$i]);
        
        //hvis der ikke går igennem skrive til log hvad sql command der blev brugt
        if (!$result) {
            $errorBool = true;
            writeToLog("GoPart kunne ikke gemme." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL ."SQL: $sqlcmdP[$i]");
        }
    }
    
    //skriver til log
    writeToLog("GoPart priserne er nu opdateret." .PHP_EOL ."Admin: $logCreatorUser" .PHP_EOL 
            ."SQL: ------------------------".PHP_EOL .$logSql . "----------------------------");
    
    //hvis der var en fejl
     if($errorBool){
        //skiver til log
        echo "Error: 2";
    }

function numberToEnglish($number) {
    $_EngNumber = str_replace('.', '', $number);
    $_EngNumber = str_replace(',', '.', $_EngNumber);
    
    return $_EngNumber;
}