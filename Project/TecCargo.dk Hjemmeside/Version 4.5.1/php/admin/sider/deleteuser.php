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
    $sqlcmd = "SELECT id,username,rank FROM members";
    $result = mysqli_query($sqlConnect, $sqlcmd);
    
    $userlist = array();
    $userIdList = array();
    $userRankList = array();
    
    while ($row = mysqli_fetch_array($result)) {
        $userIdList[] = $row[0];
        $userlist[] = $row[1];
        $userRankList[] = $row[2];
    }
    
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
    
    <form class="adminNdeleteU adminForm">
        <div class="adminNbox"><h2>Slet Brugere</h2></div>
        <table>
            <tr>
                <th>Slet</th>
                <th>Brugernavn</th>
                <th>Brugerniveau</th>
            </tr>
            <?php 
            for ($i = 0; $i < count($userlist); $i++) {
                echo "<tr id='selectId_$userIdList[$i]' class='deleteUrow'>"
                . "<td><input name='deleteuser' type='checkbox' value='$userIdList[$i]'></td>"
                . "<td>$userlist[$i]</td>"
                . "<td>$userRankList[$i]</td>"
                . "</tr>";
            }
            ?>
        </table>

        <div class="adminNbox" style="text-align: inherit;">
            <input type="reset" class="buttonInactive removeInactive" style="margin-left: 10px;" onclick="return false;">
            <input type="submit" class="buttonInactive removeInactive" value="Slet" style="float: right; margin-right: 8px;"  onclick="return false;">
        </div>
    </form>
</div>

<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../../themes/indexT.php';

