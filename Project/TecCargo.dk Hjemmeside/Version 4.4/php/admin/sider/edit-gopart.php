<?php

include '../../config.php';
include '../../is-admin.php';

session_start();

$status = "";
$statusDisplay = "style='display: none;'";

if(isset($_SESSION['Gopart']))
{
    $status = $_SESSION['Gopart'];
    $statusDisplay = "style='display: block;'";
}
unset($_SESSION['Gopart']);

    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database)or die("cannot connect");
    
    $sqlcmd = "SELECT * FROM `prisliste_kg`";
    $resultPris = mysqli_query($sqlConnect, $sqlcmd);
    $priceList = array();
    while ($row = mysqli_fetch_array($resultPris))
    {
        $priceList[] = $row[1];
    }
    
    $sqlcmd = "SELECT * FROM `prisliste_takst`";
    $resultProcent = mysqli_query($sqlConnect, $sqlcmd);   
    $procentList = array();
    while ($row = mysqli_fetch_array($resultProcent))
    {
        $procentList[] = $row[1];
        $procentList[] = $row[2];
        $procentList[] = $row[3];
        $procentList[] = $row[4];
        $procentList[] = $row[5];
        $procentList[] = $row[6];
        $procentList[] = $row[7];
        $procentList[] = $row[8];
        $procentList[] = $row[9];
    }
    $priceEdit = "";
    $fieldname = array("30 - 100 kg","125 - 250 kg","300 - 500 kg","600 - 1.000 kg",
        "1.100 - 1.500 kg","1.600 - 2.000 kg","2.100 - 2.500 kg", "2.600 - 3.000 kg");
    
    $kgform = array("30 kg","40 kg","50 kg","75 kg","100 kg","125 kg","150 kg","175 kg",
        "200 kg","250 kg","300 kg","350 kg","400 kg","450 kg","500 kg","600 kg","700 kg",
        "800 kg","900 kg","1.000 kg","1.100 kg","1.200 kg","1.300 kg","1.400 kg","1.500 kg",
        "1.600 kg","1.700 kg","1.800 kg","1.900 kg","2.000 kg","2.100 kg","2.200 kg","2.300 kg",
        "2.400 kg","2.500 kg","2.600 kg","2.700 kg","2.800 kg","2.900 kg","3.000 kg");
    
    
    //bliver brugt til at finde det næste $kgform
    $kgnum = 0;
    //
    $takstnum = 0;
    for ($i = 0; $i<8; $i++)
    {
        $priceEdit .= "<fieldset class='fieldsetprice'>";
        $priceEdit .= "<legend>" .$fieldname[$i] ."</legend>";
        $priceEdit .= "<table style='float: left;'>";
        $priceEdit .= "<tr>";
        $priceEdit .= "<th>KG</th>";
        $priceEdit .= "<th>Pris</th>";
        $priceEdit .= "</tr>";
        
        //kilo prisen
        for ($a = 0; $a < 5; $a++)
        {
            $priceEdit .= "<tr>";
            $priceEdit .= "<td>" .$kgform[$kgnum + $a] ."</td>";
            $priceEdit .= "<td><input type='text' name='priceArray_" .($kgnum + $a) ."' class='priceClass kilosaveP' value='" .number_format($priceList[$kgnum + $a],2,",",".") ."'></td>";
            $priceEdit .= "<td>.kr</td";
            $priceEdit .= "</tr>";
        }
        $priceEdit .= "</table>";
        $priceEdit .= "<table style='float:left; margin-left: 30px;'>";
        $priceEdit .= "<label style='padding: 2px;'><b>Forøg Takst med Procent</b></label><br>";
        
        //takst procent 2-6
        for ($b = 2; $b < 7; $b++)
        {
            $priceEdit .= "<tr>";
            $priceEdit .= "<td>Takst: " .$b ."</td>";
            $priceEdit .= "<td><input type='text' name='takstArray_" .$takstnum ."' class='priceClass takstProP' value='" .number_format($procentList[$takstnum],2,",",".") ."'></td>";
            $priceEdit .= "<td>%</td>";
            $priceEdit .= "</tr>";
            $takstnum++;
        }
        $priceEdit .= "</table>";
        $priceEdit .= "<table style='padding-left: 15px;'>";
        
        //takst procent 7-10
        for ($b = 7; $b < 11; $b++)
        {
            $priceEdit .= "<tr>";
            $priceEdit .= "<td>Takst: " .$b ."</td>";
            $priceEdit .= "<td><input type='text' name='takstArray_" .$takstnum ."' class='priceClass takstProP' value='" .number_format($procentList[$takstnum],2,",",".") ."'></td>";
            $priceEdit .= "<td>%</td>";
            $priceEdit .= "</tr>";
            $takstnum++;
        }
        $priceEdit .= "</table>";
        $priceEdit .= "</fieldset>";
        
        $kgnum += 5;
    }
    mysqli_close($sqlConnect);
    
    //templates

$mainIndexLink = "../../../themes/index.html";
$mainNavbarLink = "../../../themes/navbarAdmin.html";
$thisThemeLink = "../themes/goPart.html";

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);
$thisTheme = file_get_contents($thisThemeLink);

$thisTheme = str_replace("%STATUSDISPLAY%", $statusDisplay, $thisTheme);
$thisTheme = str_replace("%STATUS%", $status, $thisTheme);
$thisTheme = str_replace("%GOPARTL%", $priceEdit, $thisTheme);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

echo $mainIndex;