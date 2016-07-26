<?php
    
    $sqlConnect =  mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    
    $sqlcmd = "SELECT billedBil,billedVaegt FROM kurertransportPris;";
    
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    $billed = Array();
    
    while ($row = mysqli_fetch_array($result))
    {
        $billed[] = $row[0];
        $billed[] = $row[1];
    }
    
    mysqli_close($sqlConnect);

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
                    <th rowspan="2"><img src="../images/logo.png"></th>
                    <td colspan="4" align="center"><span style="font-size: 1.50em; font-weight: bold;">Kurertransport</span> <span style="font-weight: bold;">Prisliste DK Domestic</span><br><i>Ekskl. moms ekskl. broafgift og færgebillet.</i></td>
                </tr>
                <tr class="bilTypeBody" align='center'>
                    <td><img src="../images/<?php echo $billed[0]; ?>"></td>
                    <td><img src="../images/<?php echo $billed[2]; ?>"></td>
                    <td><img src="../images/<?php echo $billed[4]; ?>"></td>
                    <td><img src="../images/<?php echo $billed[6]; ?>"></td>
                </tr>
                <tr class="bilTypeHead">
                    <th colspan="5">Last / Vægt</th>
                </tr>
                <tr class="bilTypeBody" align="center">
                    <td align="left">Vogn type</td>
                    <td><b>Kurertransport Grp. 1</b></td>
                    <td><b>Kurertransport Grp. 2</b></td>
                    <td><b>Kurertransport Grp. 3</b></td>
                    <td><b>Kurertransport Grp. 4</b></td>
                </tr>

                <tr class="bilTypeBody" align="center">
                    <td align="left">Max vægt pr.enhed pr. mand<br>Last indtil(vægt)</td>
                    <td><img class="kiloImg" src="../images/<?php echo $billed[1]; ?>"><br>20kg | 500kg</td>
                    <td><img class="kiloImg" src="../images/<?php echo $billed[3]; ?>"><br>40kg | 750kg</td>
                    <td><img class="kiloImg" src="../images/<?php echo $billed[5]; ?>"><br>60kg | 1.000kg</td>
                    <td><img class="kiloImg" src="../images/<?php echo $billed[7]; ?>"><br>750kg | 1.200kg</td>
                </tr>
                <tr class="bilTypeBodyTop" align="center">
                    <td style="border: 0;"></td>
                    <td><button class="kurerBil" id="Kurertransport_1">Se Pris</button></td>
                    <td><button class="kurerBil" id="Kurertransport_2">Se Pris</button></td>
                    <td><button class="kurerBil" id="Kurertransport_3">Se Pris</button></td>
                    <td><button class="kurerBil" id="Kurertransport_4">Se Pris</button></td>
                </tr>
            </table>
        </div>
    </body>
</html>