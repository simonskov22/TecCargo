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
    
    if($pristype == "green") {
        $headline = "GoGreen";
    }
    else {
        $headline = "GoPlus";
    }
    
    //henter priserne
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "SELECT * FROM pakkkePrisList WHERE type = '$pristype'";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    $pristlist = mysqli_fetch_array($result);
    mysqli_close($sqlConnect);
    
    ob_start();
?>
<style>
    .adminNcreateU th
    {
        padding-left: 50px;
        text-align: left;
    }
</style>
<div>
    <script type="text/javascript">
    $(document).ready(function (){
        
        $(".adminNkurerP input").change(function (){
            $(".removeInactive").removeClass('buttonInactive');
        });
        
        $(".adminNsavePakk input[type=submit]").click(function (){
            if(!$(this).hasClass('buttonInactive')){
                $('#adminSucces').show();
                $('#loader').show();
                
                var values = [];
                
                $("input[name*='pakk_']").each(function (){
                    values.push($(this).val());
                });
                
                $.post("../php/PHPpakkesave.php",{values:JSON.stringify(values)},function (data){
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
    });
    </script>
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
    
    <form class="adminNsavePakk adminForm adminNkurerP">
        <div class="adminNbox"><h2>Rediger pris for <?php echo $headline; ?></h2></div>
        <table>
            <tr class="userCrow1 userCrowRemove">
                <th>XS</th>
                <td><input id="userCrow1" name="pakk_xs" type="text" value="<?php echo $pristlist[2]; ?>"></td>
            </tr>
            <tr class="userCrow2 userCrowRemove">
                <th>S</th>
                <td><input id="userCrow2" name="pakk_s" type="text" value="<?php echo $pristlist[3]; ?>"></td>
            </tr>
            <tr class="userCrow3 userCrowRemove">
                <th>M</th>
                <td><input id="userCrow3" name="pakk_m" type="text" value="<?php echo $pristlist[4]; ?>"></td>
            </tr>
            <tr class="userCrow4 userCrowRemove">
                <th>L</th>
                <td><input id="userCrow4" name="pakk_l" type="text" value="<?php echo $pristlist[5]; ?>"></td>
            </tr>
            <tr class="userCrow5 userCrowRemove">
                <th>XL</th>
                <td><input id="userCrow5" name="pakk_xl" type="text" value="<?php echo $pristlist[6]; ?>"></td>
            </tr>
            <tr class="userCrow6 userCrowRemove">
                <th>2XL</th>
                <td><input id="userCrow6" name="pakk_2xl" type="text" value="<?php echo $pristlist[7]; ?>"></td>
            </tr>
            <tr class="userCrow7 userCrowRemove">
                <th>3XL</th>
                <td><input id="userCrow7" name="pakk_3xl" type="text" value="<?php echo $pristlist[8]; ?>"></td>
            </tr>
            <tr class="userCrow8 userCrowRemove">
                <th>Gebyr</th>
                <td><input id="userCrow8" name="pakk_gebyr" type="text" value="<?php echo $pristlist[9]; ?>"></td>
            </tr>
        </table>
        
        <input type="hidden" name="pakk_00" value="<?php echo $pristype;?>">
        <div class="adminNbox" style="text-align: inherit;">
            <input type="reset" class="buttonInactive removeInactive" onclick="reloadPage();" style="margin-left: 10px;">
            <input type="submit" class="buttonInactive removeInactive" onclick="return false" value="Gem" style="float: right; margin-right: 8px;">
        </div>
    </form>
</div>
<?php

$Index = ob_get_contents();
ob_end_clean();


include_once '../../../themes/indexT.php';


