<?php
    //include '../../databaselogin.php';
    
    //$sqlConnect = mysqli_connect($host, $username, $password, $database);
    //$sqlcmd = "SELECT * FROM kurertransportPris";
    //$resultKurer = mysqli_query($sqlConnect, $sqlcmd);

    if(false)
    {
        
        
        $kurerTransportInfo = array();
        
        
        $kurerTransportFast = array(
            "Ladmål (l x b x h)", "læsseåbning (b x h)", "Lift (max vægt)", "Guvareal (m&sup2)", 
            "Rumindhold (m&sup3)", "Minimun pris pr. tur", "Startgebry", "Kilometertakst. pr.kørte km",
            "Tidsforbrug (ekstra læssetid/ ventetid) taxametre pr. min.", "Tidsforbrug textmetre pr. påbegyndt 30min.",
            "Chaufførmedhjælper pr. påbegynft 30min.", "Flytte tilæg, (enheder over 90 kg) pr. mand/pr. enhed",
            "ADR-tilæg (Fagligt gods) pr. forsendelese", "Aften- og nattillæg (18:00-06:00) pr. forsendelse",
            "Weekendtillæg (lørdag-søndag) pr. forsendelse", "Yderzonetillæg beregnes af nettofragt",
            "Byttepalletillæg (franko/ufranko) pr. Palle", "SMS servicetillæg pr. advisering",
            "Adresse korrektion pr. forsendelse", "Brændstofgebyr Beregnes af nettofragt",
            "Miljagebyr beregnes af nettofragt", "Adminnistrationsgebyr pr. faktura"
        );
        //tr fra Vogn Beskrivelse til Gebyr
        
        //vogn beskrivelse
        $kurerTransportTabel = '<tr class="bilTypeHead"><th colspan="2">Vogn Beskrivelse</th></tr>';
        
        for($i = 0;$i < 5; $i++)
        {
            $kurerTransportTabel .= '<tr class="bilTypeBody">';
            
            //indhold
            $kurerTransportTabel .= '<td>' .$kurerTransportFast[$i] .'</td>';
            $kurerTransportTabel .= '<td>' .$kurerTransportInfo[$i + 4] .'</td>';
            
            $kurerTransportTabel .= '</tr>';
        }
        
        //Nettofragt
        $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="2">Nettofragt</th></tr>';
        
        for($i = 5;$i < 8; $i++)
        {
            $kurerTransportTabel .= '<tr class="bilTypeBody">';
            
            //indhold
            $kurerTransportTabel .= '<td>' .$kurerTransportFast[$i] .'</td>';
            $kurerTransportTabel .= '<td>' .$kurerTransportInfo[$i + 4] .'</td>';
            
            $kurerTransportTabel .= '</tr>';
        }
        
        //tid/ minut
        $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="2">Tid/ minut</th></tr>';
        
        for($i = 8;$i < 11; $i++)
        {
            $kurerTransportTabel .= '<tr class="bilTypeBody">';
            
            //indhold
            $kurerTransportTabel .= '<td>' .$kurerTransportFast[$i] .'</td>';
            $kurerTransportTabel .= '<td>' .$kurerTransportInfo[$i + 4] .'</td>';
            
            $kurerTransportTabel .= '</tr>';
        }
        
        //Tillæg for særlige ydelser
        $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="2">Tillæg for særlige ydelser</th></tr>';
        
        for($i = 11;$i < 19; $i++)
        {
            $kurerTransportTabel .= '<tr class="bilTypeBody">';
            
            //indhold
            $kurerTransportTabel .= '<td>' .$kurerTransportFast[$i] .'</td>';
            $kurerTransportTabel .= '<td>' .$kurerTransportInfo[$i + 4] .'</td>';
            
            $kurerTransportTabel .= '</tr>';
        }
        
        //Gebyr
        $kurerTransportTabel .= '<tr class="bilTypeHead"><th colspan="2">Gebyr</th></tr>';
        
        for($i = 19;$i < 22; $i++)
        {
            $kurerTransportTabel .= '<tr class="bilTypeBody">';
            
            //indhold
            $kurerTransportTabel .= '<td>' .$kurerTransportFast[$i] .'</td>';
            $kurerTransportTabel .= '<td>' .$kurerTransportInfo[$i + 4] .'</td>';
            
            $kurerTransportTabel .= '</tr>';
        }
        
    }
?>

<html>
    <head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                width: 1050px;
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
        </style>
    </head>
    <body>
        <div class="typebil">
            <table class="kurerPrisBilType">
                <tr class="biltypeHeadline">
                    <th rowspan="2"><img src="../../../images/logo.png"></th>
                    <td align="center"><span style="font-size: 1.50em; font-weight: bold;">Kurertransport</span> <span style="font-weight: bold;">Prisliste DK Domestic</span><br><i>Ekskl. moms ekskl. broafgift og færgebillet.</i></td>
                </tr>
                <tr class="bilTypeBody" align='center'>
                    <td><img src="<?php echo $kurerTransportInfo[0]; ?>"></td>
                </tr>
                <tr class="bilTypeHead">
                    <th colspan="5">Last / Vægt</th>
                </tr>
                <tr class="bilTypeBody" align="center">
                    <td align="left">Vogn type</td>
                    <td><b><?php echo $kurerTransportInfo[1]; ?></b></td>
                </tr>

                <tr class="bilTypeBody" align="center">
                    <td align="left">Max vægt pr.enhed pr. mand<br>Last indtil(vægt)</td>
                    <td><img src="<?php echo $kurerTransportInfo[2]; ?>"><br><?php echo $kurerTransportInfo[3]; ?></td>
                </tr>
                <?php echo $kurerTransportTabel; ?>
            </table>
        </div>
    </body>
</html>
