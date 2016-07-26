<?php
    include_once '../../../config.php';
    getUserRole(true, false);

    $sqlConnect =  mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    
    //om det er green og plus priser der skal vise
        $sqlcmd = "SELECT * FROM pakkkePrisList WHERE type=$type";
        $tableColspan = 4;
    //hente priser fra database
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    //variable til database data
    $pakkeData = array();
    
    
    while ($row = mysqli_fetch_array($result))
    {
        //fjener id
        for($i = 1;$i < 10;$i++)
        {
            $pakkeData[] = $row[$i];
        }
    }
    
    $billedeLink = array();
    
    $result = mysqli_query($sqlConnect, "SELECT billede FROM `billeder`");
    while ($row1 = mysqli_fetch_array($result)) {
        $billedeLink[] = $row1[0];
    }
    mysqli_close();
    
    //variable til table tr
    $pakkeTable = array();
    
    //hvormange gange den skal køre loopet
    $forcount = count($pakkeData);
    
    for($i = 0;$i <= $forcount;$i++)
    {
        switch ($pakkeData[$i])
        {
            case "Green":
                $pakkeTable[] = "<td class='ptPris'><b>" .$pakkeData[$i]."</b><br>Priser</td>";
                break;
            case "Plus":
                $pakkeTable[] = "<td class='ptPris'><b>" .$pakkeData[$i]."</b><br>Priser</td>";
                break;
            
            default:
                $pakkeTable[] = "<td>" .$pakkeData[$i]." kr.</td>";
                break;
        }
    }
    
    //ligger to <td> sammen i en variable hvis begge priser skal vise
    //f.eks
    //"<td>green<br>Priser</td>" og "<td>plus<br>Priser</td>"
    
    //variable til det der skal skrives ud
    $outputTable = array();
    if($type == "full")
    {
        for($i = 0;$i < 9; $i++)
        {
            $outputTable[] = $pakkeTable[$i] .$pakkeTable[9 + $i];
        }
    }
    else
    {
        for($i = 0;$i < 9; $i++)
        {
            $outputTable[] = $pakkeTable[$i];
        }
    }
?>
<html>
    <body>
        <div class="pakketransportPris">
            <table>
                <tr>
                    <th rowspan="2"><img src="../../../images/logo.png" style="height: 150px;"></th>
                    <th colspan="<?php echo $tableColspan; ?>">
                        <span style="font-size: 26px;">Pakketransport</span> Prisliste DK Domestic<br>
                        <span style="font-size: 15px;"><i>Ekskl. moms</i></span>
                    </th>
                </tr>
                <tr>
                    <td colspan="<?php echo $tableColspan; ?>" style="text-align: left;">
                        Prisen afhænger af pakkens størrelse. Der er i alt syv priskategorier. For at finde prisen, skal<br>
                        du måle den lægste og den korsteste side af pakken og lægge de to tal sammen. Summen<br>
                        afgør, om pakken er XS, S, M, L, Xl, 2XL eller 3XL.<br>
                        <br>
                        <b>
                            TECCARgo accepterer ikke enheder i deres pakketransport,<br>
                            der overskrider følgende mål og vægt:<br>
                        </b>
                        <i>
                            (Længde maksimal. 180cm)<br>
                            (Længden +  den største omkreds målt i en anden retning end længden maksimal. 330 cm)<br>
                            (Fysisk brutto vægt maksimal 25 kg)
                        </i>
                    </td>
                </tr>
                <tr>
                    <td>Pakkens<br>Størrelse</td>
                    <td>Lægste +<br>Korteste side</td>
                    <td>Længde +<br>Omkreds</td>
                    <td>Fysisk Vægt<br>(Brutti)</td>
                    <?php echo $outputTable[0];?>
                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[5];?>"></td>
                    <td>maks. 30 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 2,5 kg</td>
                    <?php echo $outputTable[1];?>

                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[6];?>"></td>
                    <td>maks. 40 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 5,0 kg</td>
                    <?php echo $outputTable[2];?>
                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[7];?>"></td>
                    <td>maks. 50 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 9,0 kg</td>
                    <?php echo $outputTable[3];?>
                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[8];?>"></td>
                    <td>maks. 60 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 13,0 kg</td>
                    <?php echo $outputTable[4];?>
                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[9];?>"></td>
                    <td>maks. 70 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 17,0 kg</td>
                    <?php echo $outputTable[5];?>
                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[10];?>"></td>
                    <td>maks. 80 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 21,0 kg</td>
                    <?php echo $outputTable[6];?>
                </tr>
                <tr>
                    <td><img src="../images/<?php echo $billedeLink[11];?>"></td>
                    <td>maks. 245 cm</td>
                    <td>maks. 330 cm</td>
                    <td>maks. 25,0 kg</td>
                    <?php echo $outputTable[7];?>
                </tr>
                <tr>
                    <td colspan="3"><img src="../images/<?php echo $billedeLink[12];?>" style="width: 600px; height: 150px; max-width: none; max-height: none;"></td>
                    <td>Servicegebyr<br>pr. pakke</td>
                    <?php echo $outputTable[8];?>
                </tr>
            </table>
        </div>
    </body>
</html>