<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //og om de nødvendige settings er sat
    if(!isset($_GET['billedeName'])) {
        exit();
    }
    
    //fjerner picture_ så man kun har navnet på billedet
    $billed = substr($_GET['billedeName'], 8);
    
    //tjekker om det er i menu databasen hvis der er henter den et navnet
    $result = databaseQuery("SELECT menu_navn FROM billede_menu WHERE billede_link = '$billed'");
    
    //henter navnet
    $menuName = mysqli_fetch_row($result);
    
    
    //tjekker om det er i pakketransport databasen hvis der er henter den et navnet
    $result = databaseQuery("SELECT pakke_navn FROM billede_pakketransport WHERE billede_link = '$billed'");
    
    //henter navnet
    $pakkeName = mysqli_fetch_row($result);
    
    echo json_encode(array("menu" => $menuName, "pakke" => $pakkeName));