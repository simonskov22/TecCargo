<?php

    include_once '../../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    ob_start();
?>
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

<div>
    <img src="../../../images/admin/left_circular.png" style="position: absolute; cursor: pointer; height: 50px;" onclick="previousPage();">
    <h2 id='testdd'></h2>
    <form class="adminNcreateU adminForm">
        <div class="adminNbox"><h2>Opret Bruger</h2></div>
        <table>
            <tr class="userCrow1 userCrowRemove">
                <td>Brugernavn:</td>
                <td><input id="userCrow1" name="username" type="text"><br></td>
                <td class="tdUserneed"><span id='usernameOK' class="resetUserNeed adminUserNeed">&#42;</span></td>
            </tr>
            <tr class="userCrow2 userCrowRemove">
                <td>Kodeord:</td>
                <td><input id="userCrow2" name="password" type="password"></td>
                <td class="tdUserneed"><span id='passwordOK' class="resetUserNeed adminUserNeed">&#42;</span></td>
            </tr>
            <tr class="userCrow3 userCrowRemove">
                <td>Kodeord igen:</td>
                <td><input id="userCrow3" name="passwordC" type="password"></td>
                <td class="tdUserneed"><span id='passwordCK' class="resetUserNeed adminUserNeed">&#42;</span></td>
            </tr>
            <tr class="tablespace"><td colspan="3"></td></tr>
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
                <td colspan="2"><input name="rank" type="radio" value="User" checked="true">Bruger</td>
            </tr>
            <tr>
                <td><input name="rank" type="radio" value="Admin">Admin</td>
            </tr>
        </table>
        
        <div class="adminNbox" style="text-align: inherit;">
            <input class="buttonInactive removeInactive" onclick="return false;" type="reset" style="margin-left: 10px;">
            <input class="buttonInactive removeInactive" onclick="return false;" type="submit" value="Opret" style="float: right; margin-right: 8px;">
        </div>
    </form>
</div>


<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';

