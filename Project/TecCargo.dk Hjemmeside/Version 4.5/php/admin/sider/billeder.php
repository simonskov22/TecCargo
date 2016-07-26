<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    $side = $_GET['side'];

    if(is_null($side))
    {
        $side = 1;
    }
    
    $dir = "../../../images/";
    if(is_dir($dir)){
        if(($dh = opendir($dir))){
            
            $billedeName = array();
            
            
            while (($file = readdir($dh)) !== false)
            {
                if($file != '.' && $file != '..' && !is_dir($dir .$file))
                {
                    $billedeName[] = $file;
                }
            }
            closedir($dh);

            
            //sorting array
            sort($billedePart);
            sort($billedeName);

            //hvormange filer der er
            $pictureCount = count($billedeName);
            
            $sider = ceil(($pictureCount / 20));

        }
    }
    
    ob_start();
?>
<div id='adminSucces' class="centerWindow">
    <div class="adminForm" style="margin-top: 100px;">
        <img class="closePictureImg" src="../../../images/icons/close.png">
        <div class="adminNbox"><h2>Resultat</h2></div>
        <div id='loader1' class="loader">
            <img src='../../../images/icons/loader.gif'>
        </div>
        <p id='succuesTextStatus' style="margin-left: 20px;margin-right: 40px;"></p>
        <img id='succuesImgStatus' src="../../../images/admin/happy.png" style="height: 60px;display: block;margin: 0 auto; margin-bottom: 12px;">
        <div class="adminNbox"><button class="closeSuccesButton">Luk</button></div>
    </div>
</div>

<div id='changePicture' class="centerWindow removeRadioDot">
    <form class="adminFormStyle adminChangePic" style="margin-top: 100px; background: #fff;">
        <input type="hidden" name="changePicture_NewName">
        <img class="closePictureImg" src="../../../images/icons/close.png">
        <div class="adminNbox"><h2>Indstillinger</h2></div>
        <div id='loader2' class="loader">
            <img src='../../../images/icons/loader.gif'>
        </div>
        <div id="changeLoadeSize" style="overflow: hidden;">
            <p style="margin-left: 20px;font-weight: bold;">Skift billede på:</p>
            <fieldset class="radioMenu">
                <legend>Menu</legend>
                <div id='jpic_Kurertransport' class="changePictureDiv"><input id='cpic_Kurertransport' name='pictureSetting-menu' type="radio">Kurertransport</div>
                <div id='jpic_Pakketransport' class="changePictureDiv"><input id='cpic_Pakketransport' name='pictureSetting-menu' type="radio">Pakketransport</div>
                <div id='jpic_Godstransport' class="changePictureDiv"><input id='cpic_Godstransport' name='pictureSetting-menu' type="radio">Godstransport</div>
                <div id='jpic_Logistics' class="changePictureDiv"><input id='cpic_Logistics' name='pictureSetting-menu' type="radio">Logistics</div>
                <div id='jpic_Montage' class="changePictureDiv"><input id='cpic_Montage' name='pictureSetting-menu' type="radio">Montage</div>
            </fieldset>
            <fieldset class="radioPakke">
                <legend>Pakketransport</legend>
                <div id='jpic_XS' class="changePictureDiv"><input id='cpic_XS' name='pictureSetting-pakke' type="radio">XS Pakke</div>
                <div id='jpic_S' class="changePictureDiv"><input id='cpic_S' name='pictureSetting-pakke' type="radio">S Pakke</div>
                <div id='jpic_M' class="changePictureDiv"><input id='cpic_M' name='pictureSetting-pakke' type="radio">M Pakke</div>
                <div id='jpic_L' class="changePictureDiv"><input id='cpic_L' name='pictureSetting-pakke' type="radio">L Pakke</div>
                <div id='jpic_XL' class="changePictureDiv"><input id='cpic_XL' name='pictureSetting-pakke' type="radio">XL Pakke</div>
                <div id='jpic_2XL' class="changePictureDiv"><input id='cpic_2XL' name='pictureSetting-pakke' type="radio">2XL Pakke</div>
                <div id='jpic_3XL' class="changePictureDiv"><input id='cpic_3XL' name='pictureSetting-pakke' type="radio">3XL Pakke</div>
                <div id='jpic_Servicegebyr' class="changePictureDiv"><input id='cpic_Servicegebyr' name='pictureSetting-pakke' type="radio">Servicegebyr</div>
            </fieldset>
        </div>
        
        <div class="adminNbox" style="text-align: inherit;">
            <input type="reset" onclick="return false;" class="removeInactive" style="margin-left: 10px;">
            <input type="submit" onclick="return false;" class="removeInactive" value="Gem" style="margin-right: 10px; float: right;">
        </div>
    </form>
</div>
<img src="../../../images/admin/left_circular.png" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">
    
<div style="min-height: 222px;">
    <div style="position: absolute;">
        <div id="followScoll" style="margin-left: -26px;margin-top: 67px;float: left;">
            <form class="adminNbilledeUp adminFormStyle" style="margin-left: 28px;">
                <div class="adminNbox"><h2>Tilføj billede</h2></div>
                <ul id="nowPictureUpload" style="margin: -18px 0px 13px 30px; padding: 0;">
                    <li>Ingen billeder valgt</li>
                </ul>
                <div class="adminNbox" style="text-align: inherit;">
                    <input type="reset" class="buttonInactive removeInactive" style="margin-left: 10px;" onclick="return false;">
                    <input  onclick="$('#pickPictureUpload').click();" type="button" class="fileUpload" value="Vælg billede">
                    <input id="pickPictureUpload" type="file" class="upload" accept="image/*" multiple style="display: none;">
                    
                    <input type="submit" class="buttonInactive removeInactive" value="Upload" style="float: right; margin-right: 8px;"  onclick="return false;">
                </div>
            </form>
        </div>
    </div>
    <div>
        <form class="adminpictureT adminTable adminForm">
            <div class="adminNbox">
                <h4 style="position: absolute;margin-left: 123px;margin-top: 51px;">Slet</h4>
                <h2 style="margin: 0px;">Billeder</h2>
            </div>
            <table>
                <?php
                for($i = (20 * ($side -1));$i < $pictureCount && $i < (20 * $side);$i++)
                {
                    echo "<tr id=pic_$billedeName[$i]>"
                            . "<td><button id='picture_$billedeName[$i]' class='pictureSettings' onclick='return false;'>Indstillinger</button></td>"
                            . "<td style='width: 45px;'><input name='deletepicture' type='checkbox' value='$billedeName[$i]'></td>"
                            . "<td>$billedeName[$i]</td>"
                            . "<td><img class='maxsizePic' src='$dir/$billedeName[$i]'></td>"
                        . "</tr>";
                }
                ?>
            </table>
            <div class="adminNbox" style="text-align: inherit;">
                <input type="reset" class="buttonInactive removeInactive" style="margin-left: 10px;" onclick="return false;">
                <div style="position: absolute;margin-top: -35px;margin-left: 228px;">
                    <input type="button" value="Forrige side"
                        <?php if($side == 1) {
                             echo "class='buttonInactive' onclick='return false;'";
                        }
                        else{
                            $tSide = $side -1;
                             echo "onclick='window.location.href=\"?side=$tSide\"'";
                        }
                        ?>>
                    <span><?php echo "$side/$sider";?></span>
                    <input type="button" value="Næste side" 
                        <?php if($side == $sider) {
                             echo "class='buttonInactive' onclick='return false;'";
                        }
                        else{
                            $fSide = $side +1;
                             echo "onclick='window.location.href=\"?side=$fSide\"'";
                        }
                        ?>>
                </div>
                <input type="submit" class="buttonInactive removeInactive" value="Slet" style="float: right; margin-right: 8px;"  onclick="return false;">
            </div>
            
        </form>
    </div>
</div>


<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';

