<?php
    include '../../../config.php';
    getUserRole(TRUE, FALSE);
    
    $biltype = $_SESSION['bilType'];
    $mode = $_POST['mode'];
    
    if($biltype == "flex"){
        switch ($mode) {
            case 1:
                $mode = 5;
                break;
            case 2:
                $mode = 6;
                break;
            case 3:
                $mode = 7;
                break;
            case 4:
                $mode = 8;
                break;
        }
    }
    echo $mode;
    
    $sqlConnect =  mysqli_connect(DB_host, DB_username, DB_password, DB_database)or die('cannot connect');


    $kurerTransportFast = array(
        "Ladmål (l x b x h)", "læsseåbning (b x h)", "Lift (max vægt)", "Guvareal (m&sup2)",
        "Rumindhold (m&sup3)", "Minimun pris pr. tur", "Startgebyr", "Kilometertakst. pr.kørte km",
        "Tidsforbrug (ekstra læssetid/ ventetid) taxametre pr. min.", "Tidsforbrug textmetre pr. påbegyndt 30min.",
        "Chaufførmedhjælper pr. påbegynft 30min.", "Flytte tilæg, (enheder over 90 kg) pr. mand/pr. enhed",
        "ADR-tilæg (Fagligt gods) pr. forsendelese", "Aften- og nattillæg (18:00-06:00) pr. forsendelse",
        "Weekendtillæg (lørdag-søndag) pr. forsendelse", "Yderzonetillæg beregnes af nettofragt",
        "Byttepalletillæg (franko/ufranko) pr. Palle", "SMS servicetillæg pr. advisering",
        "Adresse korrektion pr. forsendelse", "Brændstofgebyr Beregnes af nettofragt",
        "Miljagebyr beregnes af nettofragt", "Adminnistrationsgebyr pr. faktura"
    );
    $kurerTransportHead = array("Vogn Beskrivelse", "Nettofragt", "Tid/ minut", "Tillæg for særlige ydelser", "Gebyr");
    
    $kurerAdd = array("","","","",""," cm", " cm", "", " m&sup2", " m&sup3", " kr", " kr <br><span style='font-size: 15px;'>(inkl. 20min. læssetid)</span>",
        " kr", " kr", " kr", " kr", " kr", " kr", " kr", " kr", " %", " kr", " kr", " kr", " %", " %", " kr");

    if ($mode == "full") 
        {
        $sqlcmd = "SELECT * FROM kurertransportPris WHERE type = '$biltype'";

        //colspan for html dellen
        $fullcolspan = 4;
        //colspan for overskrifterne
        $trcolspan = 5;
        
        //tabellen størrelse
        $width = "1150px";
    }
    else 
        {
        $sqlcmd = "SELECT * FROM kurertransportPris WHERE id = $mode AND type = '$biltype'";
        //echo $sqlcmd;
        //colspan for html
        $fullcolspan = 1;
        //colspan for overskrifterne
        $trcolspan = 2;
        
        //tabellen størrelse
        $width = "700px";
    }

    $resultKurer = mysqli_query($sqlConnect, $sqlcmd);

    $kurerTransportInfo = array();
    
    //while loop til at add tekst
    while ($row = mysqli_fetch_array($resultKurer))
    {
        for ($i = 1; $i < 27; $i++) 
        {
            $kurerTransportInfo[] = $row[$i] .$kurerAdd[$i];
        }
    }
    
    //finder hvor mange værdier der er skal bruges til at fjerne indhold
    $valueCount = count($kurerTransportInfo);
    
    //loop til at fjerne tekst hvis det indeholder - kr
    for($i = 0; $i < $valueCount; $i++)
    {
        $kurerTransportInfo[$i] = str_replace("- kr", "-", $kurerTransportInfo[$i]);
    }
    mysqli_close($sqlConnect);

    //tr fra Vogn Beskrivelse til Gebyr

    for ($i = 0; $i < 22; $i++) {
        //hvis $i = 5, 8,11,19 eller 22
        switch ($i) {
            case 0:
                $kurerTransportTabel = '<tr class="bilTypeHead"><th colspan="' . $trcolspan . '">' . $kurerTransportHead[0] . '</th></tr>';
                break;
            case 5:
                $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="' . $trcolspan . '">' . $kurerTransportHead[1] . '</th></tr>';
                break;
            case 8:
                $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="' . $trcolspan . '">' . $kurerTransportHead[2] . '</th></tr>';
                break;
            case 11:
                $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="' . $trcolspan . '">' . $kurerTransportHead[3] . '</th></tr>';
                break;
            case 19:
                $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="' . $trcolspan . '">' . $kurerTransportHead[4] . '</th></tr>';
                break;
            case 22:
                $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="' . $trcolspan . '">' . $kurerTransportHead[5] . '</th></tr>';
                break;
        }
        $kurerTransportTabel .= '<tr class="bilTypeBody topborder">';

        //indhold
        $kurerTransportTabel .= '<td>' . $kurerTransportFast[$i] . '</td>';
        $kurerTransportTabel .= '<td>' . $kurerTransportInfo[$i + 4] . '</td>';

        if ($mode == "full") {
            $kurerTransportTabel .= '<td>' . $kurerTransportInfo[$i + 30] . '</td>';
            $kurerTransportTabel .= '<td>' . $kurerTransportInfo[$i + 56] . '</td>';
            $kurerTransportTabel .= '<td>' . $kurerTransportInfo[$i + 82] . '</td>';
        }
        $kurerTransportTabel .= '</tr>';
    }
?>

<html>
    <head>
        <style>
            .typebil img
            {
                height: 100px;
            }
            .typebil button
            {
                margin: 20px;
                width: 100px;
                font-size: 15px;

                background: #0C2C80;
                color: white;
            }
            .typebil button:hover
            {
                background: #6B6B6B;
            }
            .typebil table
            {
                margin-left: auto;
                margin-right: auto;
                margin-top: 30px;
                margin-bottom: 20px;
                background: white;
                border-collapse: collapse;
                border: 2px solid black;
            }
            .typebil th, .typebil td
            {
                font-family: sans-serif;
                padding: 5px;
            }
            .typebil tr.biltypeHeadline td
            {
                border-left: 2px solid black;
                border-bottom: 2px solid black;
            }
            .typebil tr.bilTypeHead th
            {
                padding: 10px;
                border-top: 2px solid black;
                border-bottom: 2px solid black;
            }
            .typebil tr.bilTypeBody td
            {
                border-left: 1px solid black;
            }
            .typebil tr.bilTypeBodyTop td
            {
                border-top: 1px solid black;
                border-left: 1px solid black;
            }
            .typebil .topborder
            {
                border-top: 1px solid black;
            }
        </style>
    </head>
    <body>
        <div class="typebil">
            <table class="kurerPrisBilType" style="width: <?php echo $width; ?>">
                <tr class="biltypeHeadline">
                    <th rowspan="2" style="width: 460px;"><img src="../images/logo.png"></th>
                    <td colspan="<?php echo $fullcolspan; ?>" align="center"><span style="font-size: 1.50em; font-weight: bold;">Kurertransport</span> <span style="font-weight: bold;">Prisliste DK Domestic</span><br><i>Ekskl. moms ekskl. broafgift og færgebillet.</i></td>
                </tr>
                <tr class="bilTypeBody" align='center'>
<?php
if ($mode == "full") {
    echo '<td><img src="../images/' . $kurerTransportInfo[0] . '"></td>';
    echo '<td><img src="../images/' . $kurerTransportInfo[26] . '"></td>';
    echo '<td><img src="../images/' . $kurerTransportInfo[52] . '"></td>';
    echo '<td><img src="../images/' . $kurerTransportInfo[78] . '"></td>';
} else {
    echo '<td><img src="../images/' . $kurerTransportInfo[0] . '"></td>';
}
?>
                </tr>
                <tr class="bilTypeHead">
                    <th  colspan="<?php echo $trcolspan; ?>">Last / Vægt</th>
                </tr>
                <tr class="bilTypeBody" align="center">
                    <td align="left">Vogn type</td>
                    <?php
                    if ($mode == "full") {
                        echo '<td><b>' . $kurerTransportInfo[1] . '</b></td>';
                        echo '<td><b>' . $kurerTransportInfo[27] . '</b></td>';
                        echo '<td><b>' . $kurerTransportInfo[53] . '</b></td>';
                        echo '<td><b>' . $kurerTransportInfo[79] . '</b></td>';
                    } else {
                        echo '<td><b>' . $kurerTransportInfo[1] . '</b></td>';
                    }
                    ?>
                </tr>

                <tr class="bilTypeBody" align="center">
                    <td align="left">Max vægt pr.enhed pr. mand<br>Last indtil(vægt)</td>
                    <?php
                    if ($mode == "full") {
                        echo '<td><img class="kiloImg" src="../images/' . $kurerTransportInfo[2] . '"><br>' . $kurerTransportInfo[3] . '</td>';
                        echo '<td><img class="kiloImg" src="../images/' . $kurerTransportInfo[28] . '"><br>' . $kurerTransportInfo[29] . '</td>';
                        echo '<td><img class="kiloImg" src="../images/' . $kurerTransportInfo[54] . '"><br>' . $kurerTransportInfo[55] . '</td>';
                        echo '<td><img class="kiloImg" src="../images/' . $kurerTransportInfo[80] . '"><br>' . $kurerTransportInfo[81] . '</td>';
                    } else {
                        echo '<td><img class="kiloImg" src="../images/' . $kurerTransportInfo[2] . '"><br>' . $kurerTransportInfo[3] . '</td>';
                    }
                    ?>
                </tr>
                <?php echo $kurerTransportTabel; ?>
            </table>
        </div>
    </body>
</html>

