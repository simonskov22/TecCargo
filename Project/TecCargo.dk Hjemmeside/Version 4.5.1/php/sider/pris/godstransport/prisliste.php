<?php
    include_once '../../../config.php';
    getUserRole(true, false);
?>
<?php
    //mysql connect string
    $sqlConnect =  mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    
    //henter prisliste fra databasen og gemmer det i en array
    $sqlcmd_price_kg = "SELECT `kg` FROM `prisliste_kg`";
    $result_price_kg = mysqli_query($sqlConnect, $sqlcmd_price_kg);
    $priceArray = array();
    while ($row = mysqli_fetch_array($result_price_kg)) 
    {
        $priceArray[] = $row[0];
    }
    
    $sqlcmd_price_takst = "SELECT * FROM `prisliste_takst`";
    $result_price_takst = mysqli_query($sqlConnect, $sqlcmd_price_takst);
    $takstArrayDel1 = array();
    while ($row = mysqli_fetch_array($result_price_takst)) 
    {
        $takstArrayDel1[] = $row[1];
        $takstArrayDel1[] = $row[2];
        $takstArrayDel1[] = $row[3];
        $takstArrayDel1[] = $row[4];
        $takstArrayDel1[] = $row[5];
        $takstArrayDel1[] = $row[6];
        $takstArrayDel1[] = $row[7];
        $takstArrayDel1[] = $row[8];
        $takstArrayDel1[] = $row[9];
    }
    mysqli_close($sqlConnect);
    
    //alle kiloerne
    $kiloListArray = array(30, 40, 50, 75, 100,125, 150, 175, 200, 250,300, 350, 400, 450,
        500,600, 700, 800, 900, 1000,1100, 1200, 1300, 1400, 1500,1600, 1700, 1800,
        1900, 2000,2100, 2200, 2300, 2400, 2500,2600, 2700, 2800, 2900, 3000);
    
    //variabler til loop
    $priceRowDel1 = 0;
    $kiloRow = 0;
    $takstRowDel1 = 0;
    $takstnow = 0;
    $takstmax = 5;
    $prisTableDel1 = "";
    
    $color = true;

    //side takst 1-5
    $prisTableDel1 .= "<table>";
    $prisTableDel1 .= "<tr>";
    $prisTableDel1 .= "<th></th>";
    $prisTableDel1 .= "<th>Takst: 1</th>";
    $prisTableDel1 .= "<th>Takst: 2</th>";
    $prisTableDel1 .= "<th>Takst: 3</th>";
    $prisTableDel1 .= "<th>Takst: 4</th>";
    $prisTableDel1 .= "<th>Takst: 5</th>";
    $prisTableDel1 .= "<th>Takst: 6</th>";
    $prisTableDel1 .= "<th>Takst: 7</th>";
    $prisTableDel1 .= "<th>Takst: 8</th>";
    $prisTableDel1 .= "<th>Takst: 9</th>";
    $prisTableDel1 .= "<th>Takst: 10</th>";
    $prisTableDel1 .= "</tr>";

    //loop til at tilføje <tr></tr>
    for($a = 0;$a < 40;$a++)
    {
        if($kiloListArray[$a] == 1100)
        {
            $prisTableDel1 .= "</table>";
            $prisTableDel1 .= "<table clsse='pricesListTabel'>";
            $prisTableDel1 .= "<tr>";
            $prisTableDel1 .= "<th></th>";
            $prisTableDel1 .= "<th>Takst: 1</th>";
            $prisTableDel1 .= "<th>Takst: 2</th>";
            $prisTableDel1 .= "<th>Takst: 3</th>";
            $prisTableDel1 .= "<th>Takst: 4</th>";
            $prisTableDel1 .= "<th>Takst: 5</th>";
            $prisTableDel1 .= "<th>Takst: 6</th>";
            $prisTableDel1 .= "<th>Takst: 7</th>";
            $prisTableDel1 .= "<th>Takst: 8</th>";
            $prisTableDel1 .= "<th>Takst: 9</th>";
            $prisTableDel1 .= "<th>Takst: 10</th>";
            $prisTableDel1 .= "</tr>";
        }
            //number_format() til at add komma
        $antalKilo = number_format($kiloListArray[$kiloRow],0,",",".");
        $kiloPris = number_format($priceArray[$kiloRow],2,",",".");
        
        //hvad farve det skal være
        if($color == true)
        {
            $prisTableDel1 .= "<tr class='trblue'>";
            $color = false;
        }    
        else
        {
            $prisTableDel1 .= "<tr class='trred'>";
            $color = true;
        }
        
        $prisTableDel1 .= "<td>" .$antalKilo ." kg</td>";
        $prisTableDel1 .= "<td>" .$kiloPris ." kr</td>";
        //loop til at add prisen med procent
        for ($b = 0;$b < 9; $b++)
        {
            $kiloPrisProcent = number_format(((($priceArray[$priceRowDel1] / 100) * $takstArrayDel1[$takstRowDel1]) + $priceArray[$priceRowDel1]),2,",",".");

            $prisTableDel1 .= "<td>" .$kiloPrisProcent ." kr</td>";
            $takstRowDel1++;
        }
        $takstnow++;
        
        if($takstnow == $takstmax)
        {
            $takstmax += 5;
        }
        else
        {
            $takstRowDel1 -= 9;
        }
        $prisTableDel1 .= "</tr>";
        $kiloRow++;
        $priceRowDel1++;
    }
    //lukker tabellen for listen
    $prisTableDel1 .= "</table>";
    

?>
<html>
    <body>
            <!-- alle priserne -->
            <div class="prislist">
                <?php echo $prisTableDel1; ?>
                <p style="color: #f73700; text-align: center;">For øer uden selvstændigt postnr. og uden fast broforbindelse gælder taksterne kun til afskibningssted.</p>
            </div>
    </body>
</html>