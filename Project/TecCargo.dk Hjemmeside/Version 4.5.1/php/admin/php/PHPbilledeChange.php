<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    if (!isset($_SESSION['myusername']) || !isset($_POST['billedName'])){;
        exit();
    }
    
    //henter variablerne
    $admin = $_SESSION['myusername'];
    $billede = substr($_POST['billedName'], 8);
    $error = FALSE;
    $sqlCmdMenu = "";
    $sqlCmdPakke = "";
    
    if (isset($_POST['menuName'])) {
        $menuName = substr($_POST['menuName'], 5);
        $sqlCmdMenu = "UPDATE billede_menu SET billede_link='$billede' WHERE menu_navn = '$menuName'";
        $resultMenu = databaseQuery($sqlCmdMenu);
        
        //hvis der er fejl skriv til logs
        if (!$resultMenu) {
            $error = true;
            
            writeToLog("Kunne ikke ændre billedet for menuen." .PHP_EOL ."Admin: $admin" .PHP_EOL ."SQL: " .$sqlCmdMenu);
        }
    }
    
    
    if (isset($_POST['pakkeName'])) {
        $pakkeName = substr($_POST['pakkeName'], 5);
        $sqlCmdPakke = "UPDATE billede_pakke SET billede_link='$billede' WHERE pakke_navn = '$pakkeName'";
        $resultPakke = databaseQuery($sqlCmdPakke);
        
        //hvis der er fejl skriv til logs
        if (!$resultPakke) {
            $error = true;
            
            writeToLog("Kunne ikke ændre billed for menuen." .PHP_EOL ."Admin: $admin" .PHP_EOL ."SQL: " .$sqlCmdPakke);
        }
    }
    
    if ($error) {
        echo 'Handlingen blev ikke gennemført. Tjek logs eller prøv igen.';    
    }
     else {
        echo 'Handlingen gennemført.';
        writeToLog("Opdateret billed for menu/pakketransport." .PHP_EOL ."Admin: $admin" .PHP_EOL ."SQL Menu: " .$sqlCmdMenu .PHP_EOL ."SQL Pakke: " .$sqlCmdPakke);
    }