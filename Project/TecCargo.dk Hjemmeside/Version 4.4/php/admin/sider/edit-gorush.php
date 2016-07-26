<?php

include '../../config.php';
include '../../is-admin.php';

session_start();

$status = "";
$statusDisplay = "style='display: none;'";

if(isset($_SESSION['Kurer']))
{
    $status = $_SESSION['Kurer'];
    $statusDisplay = "style='display: block;'";
}
unset($_SESSION['Kurer']);

    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    
    $sqlcmdGrp1 = "SELECT * FROM kurertransportPris WHERE id=1";
    $sqlcmdGrp2 = "SELECT * FROM kurertransportPris WHERE id=2";
    $sqlcmdGrp3 = "SELECT * FROM kurertransportPris WHERE id=3";
    $sqlcmdGrp4 = "SELECT * FROM kurertransportPris WHERE id=4";

    $resultGrp1 = mysqli_query($sqlConnect, $sqlcmdGrp1);
    $resultGrp2 = mysqli_query($sqlConnect, $sqlcmdGrp2);
    $resultGrp3 = mysqli_query($sqlConnect, $sqlcmdGrp3);
    $resultGrp4 = mysqli_query($sqlConnect, $sqlcmdGrp4);
    
    $rushArrayGrp1 = mysqli_fetch_array($resultGrp1);
    $rushArrayGrp2 = mysqli_fetch_array($resultGrp2);
    $rushArrayGrp3 = mysqli_fetch_array($resultGrp3);
    $rushArrayGrp4 = mysqli_fetch_array($resultGrp4);
    
    mysqli_close($sqlConnect);
    
    $info = array("Billede af bilen", "overskrift", "Billede af kg", "Vægt", "Ladmål (l x b x h)", "Læsseåbning (b x h)", "Lift (max vægt)",
        "Guvareal (m²)", "Rumindhold (m³)", "Minimun pris pr. tur", "Startgebyr", "Kilometertakst. pr.kørte km",
        "Tidsforbrug (ekstra læssetid/ ventetid) taxametre pr. min.", "Tidsforbrug textmetre pr. påbegyndt 30min.",
        "Chaufførmedhjælper pr. påbegynft 30min.", "Flytte tilæg, (enheder over 90 kg) pr. mand/pr. enhed",
        "ADR-tilæg (Fagligt gods) pr. forsendelese", "Aften- og nattillæg (18:00-06:00) pr. forsendelse",
        "Weekendtillæg (lørdag-søndag) pr. forsendelse", "Yderzonetillæg beregnes af nettofragt",
        "Byttepalletillæg (franko/ufranko) pr. Palle", "SMS servicetillæg pr. advisering",
        "Adresse korrektion pr. forsendelse", "Brændstofgebyr Beregnes af nettofragt",
        "Miljagebyr beregnes af nettofragt", "Adminnistrationsgebyr pr. faktura");
    
    $tableGrp1 = "";
    $tableGrp2 = "";
    $tableGrp3 = "";
    $tableGrp4 = "";
    
    for($i = 0;$i < 26;$i++)
    {
        $tableGrp1 .= "<tr>"
                        . "<td>$info[$i]</td>"
                        . "<td style='width: 123px;'><input type='text' style='width: 123px;' value='" .$rushArrayGrp1[$i + 1] ."' name='kurerArray_$i'></td>"
                    . "</tr>";
        
        $tableGrp2 .= "<tr>"
                        . "<td>$info[$i]</td>"
                        . "<td style='width: 123px;'><input type='text' style='width: 123px;' value='" .$rushArrayGrp2[$i + 1] ."' name='kurerArray_$i'></td>"
                    . "</tr>";
        
        $tableGrp3 .= "<tr>"
                        . "<td>$info[$i]</td>"
                        . "<td style='width: 123px;'><input type='text' style='width: 123px;' value='" .$rushArrayGrp3[$i + 1] ."' name='kurerArray_$i'></td>"
                    . "</tr>";
        
        $tableGrp4 .= "<tr>"
                        . "<td>$info[$i]</td>"
                        . "<td style='width: 123px;'><input type='text' style='width: 123px;' value='" .$rushArrayGrp4[$i + 1] ."' name='kurerArray_$i'></td>"
                    . "</tr>";
    }
    
    //templates

$mainIndexLink = "../../../themes/index.html";
$mainNavbarLink = "../../../themes/navbarAdmin.html";
$thisThemeLink = "../themes/gorush.html";

$mainIndex = file_get_contents($mainIndexLink);
$mainNavbar = file_get_contents($mainNavbarLink);
$thisTheme = file_get_contents($thisThemeLink);

$thisTheme = str_replace("%STATUSDISPLAY%", $statusDisplay, $thisTheme);
$thisTheme = str_replace("%STATUS%", $status, $thisTheme);
$thisTheme = str_replace("%GRUPPE1%", $tableGrp1, $thisTheme);
$thisTheme = str_replace("%GRUPPE2%", $tableGrp2, $thisTheme);
$thisTheme = str_replace("%GRUPPE3%", $tableGrp3, $thisTheme);
$thisTheme = str_replace("%GRUPPE4%", $tableGrp4, $thisTheme);

$thisTheme = str_replace("%G1ID%", 1, $thisTheme);
$thisTheme = str_replace("%G2ID%", 2, $thisTheme);
$thisTheme = str_replace("%G3ID%", 3, $thisTheme);
$thisTheme = str_replace("%G4ID%", 4, $thisTheme);
$thisTheme = str_replace("%TYPE%", "GoRush", $thisTheme);

$thisTheme = str_replace("%GRP1%", $rushArrayGrp1[1], $thisTheme);
$thisTheme = str_replace("%GRP2%", $rushArrayGrp2[1], $thisTheme);
$thisTheme = str_replace("%GRP3%", $rushArrayGrp3[1], $thisTheme);
$thisTheme = str_replace("%GRP4%", $rushArrayGrp4[1], $thisTheme);

$mainIndex = str_replace("%NavBar%", $mainNavbar, $mainIndex);
$mainIndex = str_replace("%Index%", $thisTheme, $mainIndex);
$mainIndex = str_replace("%TECCARGO%", Teccargo_url, $mainIndex);

echo $mainIndex;