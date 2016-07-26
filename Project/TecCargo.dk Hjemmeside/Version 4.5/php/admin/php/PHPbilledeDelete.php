<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    if (!isset($_SESSION['myusername']) || !isset($_POST['billedeName'])){
        
        //skiver til log
        writeToLog("variabler ikke udflydt.");
        echo json_encode(array('Text' => 'Der skete en fejl. tjek logs.', 'Error' => true));
        
        exit();
    }
    $logDeletePicturs = "";
    
    //henter varibaler
    $admin = $_SESSION['myusername'];
    $billedeDeleteName = json_decode($_POST['billedeName']);
    $billedecount = count($billedeDeleteName);

    $uploaddir = '../../../images/';
    
    //sletter valgte billeder
    for($i = 0;$i < $billedecount; $i++)
    {
        if(file_exists($uploaddir .$billedeDeleteName[$i]))
        {
            $errorText = "None";
            unlink($uploaddir .$billedeDeleteName[$i]);
        }
        else
        {
            $errorText = "Filen findes ikke.";
            $error = true;
        }
        $logDeletePicturs .= "Navn: " .$billedeDeleteName[$i] .", Fejl Besked: " .$errorText.PHP_EOL;
    }
    
    //hvis der er fejl
    if ($error) {
        echo json_encode(array('Text' => 'Der var nogle billeder der ikke kunne slettes. tjek logs.', 'Error' => $error));
        
        //skiver til log
        writeToLog("Der var nogle billeder der ikke kunne slettes." .PHP_EOL ."Admin: $admin" .PHP_EOL ."Filer: --------------------------------------".PHP_EOL
                .$logDeletePicturs .PHP_EOL ."--------------------------------------");
    }
    else{
        echo json_encode(array('Text' => 'Billederne er nu slettet.', 'Error' => $error));
        //skiver til log
        writeToLog("Billederne er nu slettet." .PHP_EOL ."Admin: $admin" .PHP_EOL ."Filer: --------------------------------------".PHP_EOL
                .$logDeletePicturs .PHP_EOL ."--------------------------------------");
        
    }