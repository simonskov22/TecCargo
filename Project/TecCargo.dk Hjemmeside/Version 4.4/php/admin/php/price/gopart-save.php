<?php

include '../../../config.php';
$backLink = "location:../../sider/edit-gopart.php";

//henter kilo pris
$kilo = array();

for($i = 0;$i < 40; $i++)
{
    $kilo[] = $_POST["priceArray_$i"];
}

//henter kilo procent pris
$takst = array();

for($i = 0;$i < 72; $i++)
{
    $takst[] = $_POST["takstArray_$i"];
}

$saveOk = TRUE;
$kiloEng = array();
$takstEng = array();

for($i = 0; $i < 40;$i++)
{
    if($kilo[$i] == "")
    {
        $saveOk = FALSE;
        break;
    }
    
    $kiloReplace = str_replace('.','',$kilo[$i]);
    $kiloEng[] = str_replace(',','.', $kiloReplace);
}

for($i = 0; $i < 72;$i++)
{
    if($takst[$i] == "" || $saveOk == false)
    {
        $saveOk = FALSE;
        break;
    }
    $takstReplace = str_replace('.','',$takst[$i]);
    $takstEng[] = str_replace(',','.', $takstReplace);
}


if($saveOk == TRUE)
{
    $sqlcmd = array();
    
    for($i = 0; $i < 40;$i++)
    {
        $sqlcmd[] = "UPDATE prisliste_kg SET kg = $kiloEng[$i] WHERE id = " .($i + 1) ."; ";
    }
    
    $a = 0;
    for($i = 0; $i < 8;$i++)
    {
        
        
        $sqlcmd[] = "UPDATE prisliste_takst SET Takst2 = " .$takstEng[$a] .", Takst3 = " .$takstEng[1 + $a] .", Takst4 = " 
                .$takstEng[2 + $a] .", Takst5 = " .$takstEng[3 + $a] .", Takst6 = " .$takstEng[4 + $a] .", Takst7 = "
                .$takstEng[5 + $a] .", Takst8 = " .$takstEng[6 + $a] .", Takst9 = " .$takstEng[7 + $a] .", Takst10 = "
                .$takstEng[8 + $a] ." WHERE id =" .($i + 1) ."; ";
        $a += 9;
        
    }
    
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database)or die("cannot connect");
    
    $priceHead = array('30 kg','40 kg','50 kg','75 kg','100 kg','125 kg','150 kg'
        ,'175 kg','200 kg','250 kg','300 kg','350 kg','400 kg','450 kg','500 kg'
        ,'600 kg','700 kg','800 kg','900 kg','1.000 kg','1.100 kg','1.200 kg'
        ,'1.300 kg','1.400 kg','1.500 kg','1.600 kg','1.700 kg','1.800 kg'
        ,'1.900 kg','2.000 kg','2.100 kg','2.200 kg','2.300 kg','2.400 kg'
        ,'2.500 kg','2.600 kg','2.700 kg','2.800 kg','2.900 kg','3.000'
        ,"30 - 100 kg<br>Takst: 2","30 - 100 kg<br>Takst: 3","30 - 100 kg<br>Takst: 4","30 - 100 kg<br>Takst: 5","30 - 100 kg<br>Takst: 6"
        ,"30 - 100 kg<br>Takst: 7","30 - 100 kg<br>Takst: 8","30 - 100 kg<br>Takst: 9","30 - 100 kg<br>Takst: 10"
        ,"125 - 250 kg<br>Takst: 2","125 - 250 kg<br>Takst: 3","125 - 250 kg<br>Takst: 4","125 - 250 kg<br>Takst: 5","125 - 250 kg<br>Takst: 6"
        ,"125 - 250 kg<br>Takst: 7","125 - 250 kg<br>Takst: 8","125 - 250 kg<br>Takst: 9","125 - 250 kg<br>Takst: 10"
        ,"300 - 500 kg<br>Takst: 2","300 - 500 kg<br>Takst: 3","300 - 500 kg<br>Takst: 4","300 - 500 kg<br>Takst: 5","300 - 500 kg<br>Takst: 6"
        ,"300 - 500 kg<br>Takst: 7","300 - 500 kg<br>Takst: 8","300 - 500 kg<br>Takst: 9","300 - 500 kg<br>Takst: 10"
        ,"600 - 1.000 kg<br>Takst: 2","600 - 1.000 kg<br>Takst: 3","600 - 1.000 kg<br>Takst: 4","600 - 1.000 kg<br>Takst: 5","600 - 1.000 kg<br>Takst: 6"
        ,"600 - 1.000 kg<br>Takst: 7","600 - 1.000 kg<br>Takst: 8","600 - 1.000 kg<br>Takst: 9","600 - 1.000 kg<br>Takst: 10"
        ,"1.100 - 1.500 kg<br>Takst: 2","1.100 - 1.500 kg<br>Takst: 3","1.100 - 1.500 kg<br>Takst: 4","1.100 - 1.500 kg<br>Takst: 5","1.100 - 1.500 kg<br>Takst: 6"
        ,"1.100 - 1.500 kg<br>Takst: 7","1.100 - 1.500 kg<br>Takst: 8","1.100 - 1.500 kg<br>Takst: 9","1.100 - 1.500 kg<br>Takst: 10"
        ,"1.600 - 2.000 kg<br>Takst: 2","1.600 - 2.000 kg<br>Takst: 3","1.600 - 2.000 kg<br>Takst: 4","1.600 - 2.000 kg<br>Takst: 5","1.600 - 2.000 kg<br>Takst: 6"
        ,"1.600 - 2.000 kg<br>Takst: 7","1.600 - 2.000 kg<br>Takst: 8","1.600 - 2.000 kg<br>Takst: 9","1.600 - 2.000 kg<br>Takst: 10"
        ,"2.100 - 2.500 kg<br>Takst: 2","2.100 - 2.500 kg<br>Takst: 3","2.100 - 2.500 kg<br>Takst: 4","2.100 - 2.500 kg<br>Takst: 5","2.100 - 2.500 kg<br>Takst: 6"
        ,"2.100 - 2.500 kg<br>Takst: 7","2.100 - 2.500 kg<br>Takst: 8","2.100 - 2.500 kg<br>Takst: 9","2.100 - 2.500 kg<br>Takst: 10"
        ,"2.600 - 3.000 kg<br>Takst: 2","2.600 - 3.000 kg<br>Takst: 3","2.600 - 3.000 kg<br>Takst: 4","2.600 - 3.000 kg<br>Takst: 5","2.600 - 3.000 kg<br>Takst: 6"
        ,"2.600 - 3.000 kg<br>Takst: 7","2.600 - 3.000 kg<br>Takst: 8","2.600 - 3.000 kg<br>Takst: 9","2.600 - 3.000 kg<br>Takst: 10");
    
    for($i = 0;$i < 48;$i++)
    {
        $result = mysqli_query($sqlConnect, $sqlcmd[$i]);
        
        if(!$result)
        {
            echo $priceHead[$i] .'Blev ikke gemt.<br>';
        }
    }
    
    
    $message="<b>Godkendt:</b><br><br>Prisen er nu gemt.";
}
 else
{
    $message="<b>Fejl:</b><br><br>Prisen kunne ikke gemmes.";
}


session_start();

$_SESSION['Gopart'] = $message;

header($backLink);