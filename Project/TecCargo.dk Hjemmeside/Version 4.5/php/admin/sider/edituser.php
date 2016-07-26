<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    //henter brugere
    $sqlConnect = mysqli_connect(DB_host, DB_username, DB_password, DB_database);
    $sqlcmd = "SELECT id,Username FROM members";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    $userlist = array();
    $userIdList = array();
    while ($row = mysqli_fetch_array($result)) {
        $userIdList[] = $row[0];
        $userlist[] = $row[1];
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
    
    
    <div class="adminNdropDown">
        <div id="dropdownuserId" class="adminNdropDownArrow">
            <span style="display: block;font-size: 25px;margin-left: 12px;margin-top: 10px;color: #fff;">&#9660;</span>
        </div>
        <h4 id="nowDropdownEdit" style="margin: 0px; padding: 0px;">Vælg Bruger</h4>
        <ul id="dropdownUserUl" class="adminNdropDownSelector" style="margin: 0px; padding: 0px; max-height: 200px; overflow: scroll;">
            <?php 
            for ($i = 0; $i < count($userlist); $i++) {
                echo "<li id='editUserId_$userIdList[$i]' class='edituserS'>$userlist[$i]</li>";
            }
            ?>
        </ul>
        
    </div>
    <div style="padding-top: 80px;">
        <form class="adminNeditU adminForm">
            <input name="userId" type="hidden">
            <div class="adminNbox"><h2>Rediger Bruger</h2></div>
            <table>
                <tr class="userCrow1 userCrowRemove">
                    <td>Brugernavn:</td>
                    <td colspan="2"><input id="userCrow1" name="username" type="text"></td>
                </tr>
                <tr class="userCrow2 userCrowRemove">
                    <td>Kodeord:</td>
                    <td colspan="2"><input id="userCrow2" name="password" type="password"></td>
                </tr>
                <tr class="userCrow3 userCrowRemove">
                    <td>Kodeord igen:</td>
                    <td colspan="2"><input id="userCrow3" name="passwordC" type="password"></td>
                </tr>
                <tr class="tablespace"><td colspan="2"></td></tr>
                <tr class="userCrow4 userCrowRemove">
                    <td>Fornavn:</td>
                    <td colspan="2"><input id="userCrow4" name="name" type="text"></td>
                </tr>
                <tr class="userCrow5 userCrowRemove">
                    <td>Efternavn:</td>
                    <td colspan="2"><input id="userCrow5" name="lastname" type="text"></td>
                </tr>
                <tr class="userCrow6 userCrowRemove">
                    <td>Email:</td>
                    <td colspan="2"><input id="userCrow6" name="email" type="email"></td>
                </tr>
                <tr class="tablespace">
                    <td rowspan="2">Brugerniveau</td>
                    <td><input id="radioUser" name="rank" type="radio" value="User" checked="true">Bruger</td>
                </tr>
                <tr>
                    <td><input id="radioAdmin" name="rank" type="radio" value="Admin">Admin</td>
                </tr>
            </table>

            <div class="adminNbox" style="text-align: inherit;">
                <input type="reset" class="buttonInactive removeInactive" style="margin-left: 10px;">
                <input type="submit" class="buttonInactive removeInactive" value="Gem" onclick="return false;" style="float: right; margin-right: 8px;">
            </div>
        </form>
    </div>
</div>

<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';

