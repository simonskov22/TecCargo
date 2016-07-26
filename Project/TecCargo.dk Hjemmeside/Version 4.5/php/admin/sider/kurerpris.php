<?php
    
    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    if(!isset($_GET['type'])) {
        //header("location:" .Teccargo_url ."php/admin/");
        exit();
    }
    $biltype = $_GET['type'];
    
    //find database id og headline
    if($biltype == "rush") {
        $databaseId = array(1,2,3,4);
        $headline = "GoRush";
    }
     else {
        $databaseId = array(5,6,7,8);
        $headline = "GoFlex";
    }
    ob_start();
    
?>
<style>
    
.adminFtable img
{
    max-height: 128px;
    max-width: 128px;
}
</style>
<div>
    <img src="../../../images/admin/left_circular.png" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">

    <?php 
        //hvis der ikke er valgt nogen bil gruppe
        if(!isset($_GET['kurer'])) {
        
        //henter billede af bilerne
        $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
        $sqlcmd = "SELECT billedBil FROM kurertransportPris WHERE id=$databaseId[0] or id=$databaseId[1] or id=$databaseId[2] or id=$databaseId[3]";
        $result = mysqli_query($sqlConnect, $sqlcmd);

        $billedeName = array();
        while ($row = mysqli_fetch_array($result)) {
            $billedeName[] = $row[0];
        }
        mysqli_close($sqlConnect);

    ?>
    <table class="adminFtable">
        <tr>
            <th colspan="4"><?php echo "Rediger priser for $headline"; ?></th>
        </tr>
        <tr>
            <td style="border-bottom: 0px;"><img src='../../../images/<?php echo "$billedeName[0]";?>'></td>
            <td style="border-bottom: 0px;"><img src='../../../images/<?php echo "$billedeName[1]";?>'></td>
            <td style="border-bottom: 0px;"><img src='../../../images/<?php echo "$billedeName[2]";?>'></td>
            <td style="border-bottom: 0px;"><img src='../../../images/<?php echo "$billedeName[3]";?>'></td>
        </tr>
        <tr>
            <td style="border-top: 0px;"><button class="adminFbutton adminFbuttonActive" onclick="window.location.href='<?php echo "?type=$biltype&kurer=$databaseId[0]"; ?>'">Rediger Gruppe 1</button></td>
            <td style="border-top: 0px;"><button class="adminFbutton adminFbuttonActive" onclick="window.location.href='<?php echo "?type=$biltype&kurer=$databaseId[1]"; ?>'">Rediger Gruppe 2</button></td>
            <td style="border-top: 0px;"><button class="adminFbutton adminFbuttonActive" onclick="window.location.href='<?php echo "?type=$biltype&kurer=$databaseId[2]"; ?>'">Rediger Gruppe 3</button></td>
            <td style="border-top: 0px;"><button class="adminFbutton adminFbuttonActive" onclick="window.location.href='<?php echo "?type=$biltype&kurer=$databaseId[3]"; ?>'">Rediger Gruppe 4</button></td>
        </tr>
    </table>
<?php 
    }
    else { 
        $kurerNumber = $_GET['kurer'];
        
        //tekst
        $tekst = array("ID","Billede af bilen", "Overskrift", "Billede af kg", "Vægt", "Ladmål (l x b x h)", 
            "Læsseåbning (b x h)", "Lift (max vægt)","Guvareal (m²)", "Rumindhold (m³)", 
            "Minimun pris pr. tur", "Startgebyr", "Kilometertakst. pr.kørte km",
            "Tidsforbrug (ekstra læssetid/ ventetid) taxametre pr. min.", 
            "Tidsforbrug textmetre pr. påbegyndt 30min.","Chaufførmedhjælper pr. påbegynft 30min.", 
            "Flytte tilæg, (enheder over 90 kg) pr. mand/pr. enhed","ADR-tilæg (Fagligt gods) pr. forsendelese",
            "Aften- og nattillæg (18:00-06:00) pr. forsendelse", "Weekendtillæg (lørdag-søndag) pr. forsendelse", 
            "Yderzonetillæg beregnes af nettofragt", "Byttepalletillæg (franko/ufranko) pr. Palle",
            "SMS servicetillæg pr. advisering","Adresse korrektion pr. forsendelse",
            "Brændstofgebyr Beregnes af nettofragt", "Miljagebyr beregnes af nettofragt", 
            "Adminnistrationsgebyr pr. faktura");

        //henter priser fra database
        $bilId = $_GET['kurer'];
        //billedBil,gruppe,billedVaegt,vaegt,ladmaal,laesseaabning,lift,guvareal,rumindhold,prisPrTur,startgebyr,kilometertaksst,tidsForbrug,tidsForbrug2,chauffoermedhjaelper,flytteTilaeg,ADR-tilaeg,natTillaeg,weekendTillaeg,yderzoneTillaeg,yderzoneTillaeg,,,,,,,
        $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
        $sqlcmd = "SELECT * FROM kurertransportPris WHERE id=$bilId";
        //echo $sqlcmd;
        $result = mysqli_query($sqlConnect, $sqlcmd);
        
        $pristlist = mysqli_fetch_array($result);
        
        mysqli_close($sqlConnect);
        
?>
    <style>
        .adminNkurerP input[type='button']
        {
            width: 200px;
            color: #fff;
            background: #0D4AE7;
            border: 2px outset #275CE6;
        }
        .minimaxsize
        {
            max-width: 100px;
            max-height: 40px;
        }
        .adminChangePris td
        {
            border-bottom: 1px solid black;
        }
    </style>
    
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
    
    <div id='changePicture' class="centerWindow">
        <form class="adminForm adminChangePris" style="margin-top: 80px; background: #fff;">
            <input name="changePicFor" type="hidden" value="">
            <img class="closePictureImg" src="../../../images/icons/close.png">
            <div class="adminNbox"><h2>Skift billede</h2></div>
            
            <div id='loader' class="loader" style="background: none; display: none;">
                <img src='../../../images/icons/loader.gif' style="background: #3975F0;border-radius: 10px;">
            </div>
            
            <table id="pickPicpage">
            <?php 
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

                    $sider = ceil(($pictureCount / 15));

                }
            }
            
            for($i = (15 * (0));$i < $pictureCount && $i < (15 * 1);$i++)
            {
                echo "<tr id=pic_$billedeName[$i]>"
                        . "<td><button id='picture_$billedeName[$i]' class='pictureSettings' onclick='return false;'>Skift</button></td>"
                        . "<td><img class='minimaxsize' src='$dir/$billedeName[$i]'></td>"
                    . "</tr>";
            }
            ?>
            </table>
            <div class="adminNbox" style="text-align: inherit;">
                <input id="backPicPage_none" type="button" value="Forrige side" class='buttonInactive changePicPage backpage' onclick='return false;' style="margin-left: 10px;">
                <span id="nowPicPage" style="margin-left: 70px;"><?php echo "1/$sider";?></span>
                <input id="nextPicPage_2" type="button" value="Næste side" class="changePicPage nextpage" onclick='return false;' style="float: right; margin-right: 10px;">
            </div>
        </form>
    </div>
    <script type="text/javascript">
        
    $(document).ready(function (){
        $(".adminNkurerP input[type='submit']").click(function (){
            
            if(!$(this).hasClass('buttonInactive')){
                $('#adminSucces').show();
                $("#loader").show();
                var values = [];
                
                $(".adminNkurerP input[name*='editPrice_']").each(function (){
                    values.push($(this).val());
                });
                
                $.post("../php/PHPkurersave.php",{values:JSON.stringify(values)},function (data){
                    var status = data.substr(7);
                    var text;
                    var img = 'sad.png';
                    
                    switch(status){
                        case '1':
                            text = "Kunne ikke gemme priserne. Tjek log.";
                            break;
                        default :
                            text = "Priserne er nu gemt.";
                            img = "happy.png";
                            break;
                    }
                    $("#succuesTextStatus").html(text);
                    $("#succuesImgStatus").attr("src" ,"../../../images/admin/" + img);
                    $("#loader").hide();
                });
            }
        });
        
        
        $(".adminNkurerP input").change(function (){
            $(".removeInactive").removeClass('buttonInactive');
        });
        
        $(".kurerChangePic").click(function (){
            $("input[name='changePicFor']").val(($(this).attr('id')));
        });
        
        $(".pictureSettings").click(function (){
            var changeFor = $("input[name='changePicFor']").val();
            var billedeLink = $(this).attr('id');
            
            $("input[name='"+changeFor+"']").val(billedeLink.substr(8));
            
            $(".removeInactive").removeClass('buttonInactive');
            
            alert("Du har nu skiftet billede.");
        });
        
        $(".changePicPage").click(function (){
            if(!$(this).hasClass('buttonInactive')){
                $("#loader").show();
                
                //henter id på den knap man har klikket på
                var pageId = $(this).attr('id');

                $.post("../php/PHPkurerbilleder.php",{page:pageId.substr(12)},function (data){
                    var json = JSON.parse(data);
                    
                    $(".backpage").attr('id',"backPicPage_" +json.backpage).removeClass('buttonInactive');
                    $(".nextpage").attr('id',"nextPicPage_" +json.nextpage).removeClass('buttonInactive');
                    $("#nowPicPage").html(json.nowpage);
                    $("#pickPicpage").html(json.table);
                    
                    //deaktivere knapperne hvis de ikke har nogen næste side
                    if(json.nextpage === 'none'){
                        $(".nextpage").addClass('buttonInactive');
                    }
                    if(json.backpage === 'none'){
                        $(".backpage").addClass('buttonInactive');
                    }
                    
                    $("#loader").hide();
                });
            } 
        });
    });
    
    function reloadPage(){
        if(!$(this).hasClass('buttonInactive')){
            location.reload();
        }
    }
    </script>
    <form class="adminNkurerP adminForm" style="width: 700px;">
        <div class="adminNbox"><h2>Rediger pris for <?php echo $headline;?></h2></div>
        <table>
            <tr class="userCrow1 userCrowRemove">
                <td style="padding-left: 10px;"><?php echo $tekst[1]; ?></td>
                <td>
                    <input id="editPrice_1" type="button" class="kurerChangePic" value="Skift billede" onclick="$('#changePicture').toggle();">
                    <input type='hidden' name="editPrice_1" value="<?php echo $pristlist[1]; ?>">
                </td>
            </tr>
            <tr class="userCrow2 userCrowRemove">
                <td style="padding-left: 10px;"><?php echo $tekst[2]; ?></td>
                <td><input id="userCrow2" name="editPrice_2" type="text" value="<?php echo $pristlist[2]; ?>"></td>
            </tr>
            <tr class="userCrow3 userCrowRemove">
                <td style="padding-left: 10px;"><?php echo $tekst[3]; ?></td>
                <td>
                    <input id="editPrice_3" type="button" class="kurerChangePic kurerChangeKiloPic" value="Skift billede" onclick="$('#changePicture').toggle();">
                    <input type='hidden' name="editPrice_3" value="<?php echo $pristlist[3]; ?>">
                </td>
            </tr>
            <?php
                for($i = 4; $i < count($tekst); $i++) {
                    echo "<tr class='userCrow$i userCrowRemove'>"
                            . "<td  style='padding-left: 10px;'>$tekst[$i]</td>"
                            . "<td><input id='userCrow$i' name='editPrice_$i' type='text' value='$pristlist[$i]'></td>"
                            . "</tr>";
                }
            ?>
        </table>
        
        <input type="hidden" name="editPrice_00" value="<?php echo $biltype;?>">
        <input type="hidden" name="editPrice_01" value="<?php echo $kurerNumber;?>">
        <div class="adminNbox" style="text-align: inherit;">
            <input type="reset" class="buttonInactive removeInactive" onclick="reloadPage();" style="margin-left: 10px;">
            <input type="submit"class="buttonInactive removeInactive" onclick="return false;" value="Gem" style="float: right; margin-right: 8px;">
        </div>
    </form>
<?php } ?>
</div>

<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';