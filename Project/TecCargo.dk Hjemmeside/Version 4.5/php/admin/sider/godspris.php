<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    if(!isset($_GET['type'])) {
        header("location:" .Teccargo_url ."php/admin/");
        exit();
    }
    $pristype = $_GET['type'];
    
    $fieldname = array("30 - 100 kg","125 - 250 kg","300 - 500 kg","600 - 1.000 kg",
        "1.100 - 1.500 kg","1.600 - 2.000 kg","2.100 - 2.500 kg", "2.600 - 3.000 kg");
    
    $kgform = array("30 kg","40 kg","50 kg","75 kg","100 kg","125 kg","150 kg","175 kg",
        "200 kg","250 kg","300 kg","350 kg","400 kg","450 kg","500 kg","600 kg","700 kg",
        "800 kg","900 kg","1.000 kg","1.100 kg","1.200 kg","1.300 kg","1.400 kg","1.500 kg",
        "1.600 kg","1.700 kg","1.800 kg","1.900 kg","2.000 kg","2.100 kg","2.200 kg","2.300 kg",
        "2.400 kg","2.500 kg","2.600 kg","2.700 kg","2.800 kg","2.900 kg","3.000 kg");
    
    $takstArray = array("Takst 2","Takst 3","Takst 4","Takst 5","Takst 6","Takst 7","Takst 8","Takst 9","Takst 10");
    
    //henter priserne
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmdPris = "SELECT id, startPris FROM godstransportPris ORDER BY id ASC";
    $sqlcmdTakst = "SELECT * FROM prisliste_takst";
    
    $resultPris = mysqli_query($sqlConnect, $sqlcmdPris);
    $resultTakst = mysqli_query($sqlConnect, $sqlcmdTakst);
    
    mysqli_close($sqlConnect);
    
    $startPris = array("id" => array(), "pris" => array());
    
    $alltakst = array("id" => array(), array(), 
        array(), array(),
        array(), array(),
        array(), array(),
        array(), array());
    
    //sætter priserne i en array
    while ($row = mysqli_fetch_array($resultPris)) {
        $startPris['id'][] = $row[0];
        $startPris['pris'][] = $row[1];
    }
    
    while ($row = mysqli_fetch_array($resultTakst))
    {
        $alltakst['id'][] = $row[0];
        $alltakst[1][] = $row[1];
        $alltakst[2][] = $row[2];
        $alltakst[3][] = $row[3];
        $alltakst[4][] = $row[4];
        $alltakst[5][] = $row[5];
        $alltakst[6][] = $row[6];
        $alltakst[7][] = $row[7];
        $alltakst[8][] = $row[8];
        $alltakst[9][] = $row[9];
    }
    ob_start();
?>
<div>
    <div id='adminSucces' class="centerWindow">
        <div class="adminForm" style="margin-top: 100px;">
            <img class="closePictureImg" src="../../../images/icons/close.png">
            <div class="adminNbox"><h2>Resultat</h2></div>
            <div id='loader' class="loader">
                <img src='../../../images/icons/loader.gif'>
            </div>
            <p id='succuesTextStatus' style="margin-left: 20px;margin-right: 40px;"></p>
            <img id='succuesImgStatus' src="../../../images/admin/happy.png" style="height: 60px;display: block;margin: 0 auto; margin-bottom: 12px;">
            <div class="adminNbox"><button class="closeSuccesButton">Luk</button></div>
        </div>
    </div>
    
    <img src="../../../images/admin/left_circular.png" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">
    
    <form class="adminNgodsPris adminForm inputChangeAllow" style="width: 700px;">
        <div class="adminNbox"><h2>Rediger pris for GoPart</h2></div>
        <?php
        //bruges til at finde prisen og id
        $prisNum = 0;
        $pridId = 0;
        
        for($i = 0; $i < 8; $i++) {
        ?>
        <fieldset>
            <legend><?php echo $fieldname[$i];?></legend>
            
            <table style='border-collapse: collapse; margin-top: 0px;'>
                <tr>
                    <th>KG</th>
                    <th colspan='2'>Pris</th>
                    <th colspan='8'>Forøg Takst med Procent</th>
                </tr>
                
                <?php 
                
                for($a = 0; $a < 5; $a++){
                    
                    //til at finde takst for 7 - 10
                    $ta = $a + 5;
                    
                    //bruges til at makere den man har valgt med blåt                    
                    $godsSelectTakst = "godsSelect_T_$i" ."_$a";
                    $godsSelectTakst2 = "godsSelect_T_$i" ."_$ta";
                    $godsSelectName = "godsSelect_P_$i" ."_$a";
                    
                    //bruges til når det skal upload at finde rundt i hvad der hører til hvad
                    $godsInputNameTakst = "godsName_T_$a";//"godsName_T_$i" ."_$a";
                    $godsInputNameTakst2 = "godsName_T_$ta";                    
                    $godsInputName = "godsName_P_" .($pridId + $a);
                    
                    //den nuværende værdi der står i databasen
                    $godsInputValueTakst = number_format(($alltakst[$a +1][$i]),2,",",".");
                    $godsInputValueTakst2 = number_format(($alltakst[$ta +1][$i]),2,",",".");
                    $godsInputValue = number_format(($startPris['pris'][$prisNum]),2,",",".");
                    
                    //bruges at se hvad takst/kilo det er
                    $godsValueTakst = $takstArray[$a];
                    $godsValueTakst2 = $takstArray[$ta];
                    $godsKiloC = $kgform[$a];
                
                    $prisNum++;
                ?>
                
                <tr>
                    <td class="<?php echo $godsSelectName;?> removeGodsSelect godsKiloTr"><?php echo $godsKiloC;?></td> 
                    <td class="<?php echo $godsSelectName;?> removeGodsSelect">
                        <input name="<?php echo $godsInputName; ?>" id="<?php echo $godsSelectName;?>" class="inputGodsStart" type="text" value="<?php echo $godsInputValue;?>">
                    </td>
                    <td class="<?php echo $godsSelectName;?> removeGodsSelect">.kr</td>
                    
                    <td style='width: 20px;'></td>
                    
                    <td class="<?php echo $godsSelectTakst;?> removeGodsSelect godsKiloTr"><?php echo $godsValueTakst;?></td>
                    <td class="<?php echo $godsSelectTakst;?> removeGodsSelect">
                        <input name="<?php echo $godsInputNameTakst; ?>" id="<?php echo $godsSelectTakst;?>" class="inputGodsStart" type="text" value="<?php echo $godsInputValueTakst;?>">
                    </td>
                    <td class="<?php echo $godsSelectTakst;?> removeGodsSelect">%</td>
                    <td style='width: 20px;'></td>
                    
                    <?php 
                    if ($a < 4){
                    ?>
                    
                    <td class="<?php echo $godsSelectTakst2;?> removeGodsSelect godsKiloTr"><?php echo $godsValueTakst2;?></td>
                    <td class="<?php echo $godsSelectTakst2;?> removeGodsSelect">
                        <input name="<?php echo $godsInputNameTakst2; ?>" id="<?php echo $godsSelectTakst2;?>" class="inputGodsStart" type="text" value="<?php echo $godsInputValueTakst2;?>">
                    </td>
                    <td class="<?php echo $godsSelectTakst2;?> removeGodsSelect">%</td>
                    
                    <?php 
                    } else {
                    ?>
                    
                    <td></td>
                    <td></td>
                    <td></td>
                    
                    <?php
                    }
                    ?>
                    
                <tr>

                <?php
                }
                ?>
                
            </table>
        </fieldset>
           
        <?php
            $pridId += 5;
        }
        ?>
        
        <div class="adminNbox" style="text-align: inherit;">
            <input type="reset" onclick="return false; reloadPage();" class="buttonInactive removeInactive" style="margin-left: 10px;">
            <input type="submit" onclick="return false;" class="buttonInactive removeInactive" value="Gem" style="float: right; margin-right: 8px;">
        </div>
    </form>
</div>
<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';