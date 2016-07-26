<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    if (!isset($_SESSION['myusername']) || !isset($_POST['post1']) || 
        !isset($_POST['post2']) || !isset($_POST['kilo'])){
        echo '111';
        exit();
    }
    
    $post1 = $_POST['post1'];
    $post2 = $_POST['post2'];
    $kilo = doubleval($_POST['kilo']);
    
    //hent takst zone
    $resultTaskt = databaseQuery("SELECT `$post1` FROM beregnPris_takster WHERE post='$post2'");
    
    $takstZone = mysqli_fetch_row($resultTaskt);
    
    //find ud af hvor den ligger i kilo
    $kiloArray = array(0, 30,40,50,75,100,125,150,175,200,250,300,350,400,450,500,600,700,800,900,1000,
        1100,1200,1300,1400,1500,1600,1700,1800,1900,2000,2100,2200,2300,2400,2500,2600,2700,2800,2900,3000);
    
    $kiloArrayCount = count($kiloArray);
    $id = 0;
    
    for($i = 0; $i < $kiloArrayCount -1; $i++){
        if ($kilo > $kiloArray[$i] && $kilo <= $kiloArray[$i +1]) {
            $id = ($i +1);
            break;
        }
    }
    
    //hent start pris og takst procent zone
    $resultStatPrice = databaseQuery("SELECT kg, takst_id FROM prisliste_kg WHERE id='$id'");
    
    $rowStart = mysqli_fetch_array($resultStatPrice);
    $price = $rowStart[0];
    $takstid = $rowStart[1];
    
    //hvis takst zone ikke er 1 
    if ($takstZone[0] != 1) {
        //hent takst procent
        $resultTakstProcent = databaseQuery("SELECT Takst$takstZone[0] FROM prisliste_takst WHERE id='$takstid'");

        $takstProcent = mysqli_fetch_row($resultTakstProcent);

        //gang start prisen med procent
        $intPirce = doubleval($price);
        $intProcent = doubleval($takstProcent[0]);
        
        $price = (($intPirce * ($intProcent / 100)) + $intPirce);
    }
    
    echo number_format($price, 2 ,',','.') . "kr.";