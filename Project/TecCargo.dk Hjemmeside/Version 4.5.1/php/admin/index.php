<?php
    include_once '../config.php';
    getUserRole(TRUE, TRUE);
    
    //henter menu
    
    ob_start();
    include '../../themes/navbarAdminT.php';
    $NavBar = ob_get_contents();
    ob_end_clean();
    
    ob_start();
?>
<style>
    .adminMenuHead
    {
        background: #162D5C;
        color: #fff;
    }
</style>
<table class="adminFtable">
    <tr class="adminMenuHead">
        <th colspan="3">Brugerindstillinger</th>
    </tr>
    <tr>
        <td>
            <img src="../../images/admin/add_user.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/createuser.php'">Opret bruger</button>
        </td>
        <td>
            <img src="../../images/admin/edit_user.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/edituser.php'">Rediger bruger</button>
        </td>
        <td>
            <img src="../../images/admin/remove_user.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/deleteuser.php'">Slet bruger</button>
        </td>
    </tr>
    <tr class="adminMenuHead">
        <th colspan="3">Kurertransport Priser</th>
    </tr>
    <tr>
        <td>
            <img src="../../images/admin/file.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/kurerpris.php?type=rush'">GoRush</button>
        </td>
        <td>
            <img src="../../images/admin/file.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/kurerpris.php?type=flex'">GoFlex</button>
        </td>
        <td></td>
    </tr>
    <tr class="adminMenuHead">
        <th colspan="3">Pakketransport Priser</th>
    </tr>
    <tr>
        <td>
            <img src="../../images/admin/file.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/pakkepris.php?type=plus'">GoPlus</button>
        </td>
        <td>
            <img src="../../images/admin/file.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/pakkepris.php?type=green'">GoGreen</button>
        </td>
        <td></td>
    </tr>
    <tr class="adminMenuHead">
        <th colspan="3">Godstransport Priser</th>
    </tr>
    <tr>
        <td>
            <img src="../../images/admin/file.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/godspris.php?type=part'">GoPart</button>
        </td>
        <td>
            <img src="../../images/admin/file.png"><br>
            <button class="adminFbutton buttonInactive" onclick="return false;">GoFull</button>
        </td>
        <td></td>
    </tr>
    
    <tr class="adminMenuHead">
        <th colspan="3">Andet</th>
    </tr>
    <tr>
        <td>
            <img src="../../images/admin/calculator.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/beregnzone.php'">Beregner</button>
        </td>
        <td>
            <img src="../../images/admin/photos.png"><br>
            <button class="adminFbutton adminFbuttonActive" onclick="window.location.href='sider/billeder.php'">Billeder</button>
        </td>
        <td>
            <img src="../../images/admin/logs.png"><br>
            <button class="adminFbutton buttonInactive" onclick="return false;">Logs</button>
        </td>
    </tr>
</table>
<?php
$Index = ob_get_contents();
ob_end_clean();

include_once '../../themes/indexT.php';
